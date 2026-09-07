# DevSu Bank

Solución para la prueba técnica de Devsu: una arquitectura de microservicios para un
sistema bancario simple (clientes, cuentas y movimientos).

## Arquitectura

Dos microservicios independientes, cada uno con su propia base de datos, que se comunican
por mensajería y nunca se llaman entre sí por HTTP.

```
                                  ┌─────────────────┐
       una sola URL  ────────────►│     Gateway     │  nginx  :5000
                                  └────┬───────┬────┘
                      /clientes ◄──────┘       └──────► /cuentas
                                                        /movimientos
                                                        /reportes
                    ┌──────────────┐        ┌──────────────┐
                    │  Customers   │        │   Accounts   │
                    │     API      │        │     API      │
                    │    :5001     │        │    :5002     │
                    └──────┬───────┘        └───────┬──────┘
                           │                        │
                           ▼                        ▼
                  ┌─────────────────┐     ┌─────────────────┐
                  │ DevSuBank       │     │ DevSuBank       │
                  │ Customers       │     │ Accounts        │
                  │                 │     │                 │
                  │  Clients        │     │  Accounts       │
                  └─────────────────┘     │  Transactions   │
                           │              │  Clients ◄──────┼── réplica
                           │              └─────────────────┘
                           │                        ▲
       ClientCreated       │                        │  ClientRegistered
       ClientUpdated       │                        │  ClientUpdated
       ClientDeleted       │                        │  ClientDeleted
                           │   ┌────────────────┐   │
                           └──►│    RabbitMQ    │───┘
                               └────────────────┘
```

### El flujo

Cuando se crea, actualiza o elimina un cliente, el agregado `Client` registra un **evento
de dominio**. Al confirmarse la transacción, MediatR despacha ese evento y su handler
publica un **evento de integración** en RabbitMQ. Del otro lado, `Accounts` lo consume y
actualiza su copia local del cliente. Si el cliente fue eliminado, además **desactiva
todas sus cuentas** en cascada.

El cambio de contraseña no publica nada: es un dato que a `Accounts` no le sirve.

La mensajería usa **MassTransit sobre RabbitMQ**. Los dos servicios no comparten ningún
proyecto: cada uno declara su propia clase de contrato y ambas coinciden en el `MessageUrn`,
que es el acuerdo real entre ellos. Cambiar el nombre de una clase en un servicio no rompe
al otro.

### Por qué se duplican los datos del cliente

`Accounts` guarda una copia mínima de cada cliente: identificador, nombre y estado.

La alternativa era pedirle esos datos a `Customers` por HTTP cada vez que se necesitan, y
se necesitan seguido: al crear una cuenta hay que validar que el cliente exista, y el
reporte de estado de cuenta muestra el nombre del cliente en cada fila. Eso significaría
acoplar los dos servicios y agregar un punto de falla en operaciones cotidianas.

La copia es de solo lectura y `Customers` sigue siendo el dueño del dato. Se mantiene al
día por eventos, con consistencia eventual: un cambio tarda milisegundos en propagarse.

### Resiliencia

El enunciado pide *contemplar* rendimiento, escalabilidad y resiliencia. La resiliencia
aquí no quedó en el papel: es consecuencia directa de la decisión anterior y se puede
comprobar.

**Con `Customers` apagado, `Accounts` sigue operando por completo**: crea cuentas, registra
movimientos y genera reportes con el nombre del cliente, porque los datos que necesita
están en su propia base.

**Con `Accounts` apagado, `Customers` sigue atendiendo**: los eventos quedan encolados en
RabbitMQ y se procesan solos cuando el servicio vuelve. Nada se pierde.

Sobre los otros dos factores: el **rendimiento** se atendió con índices pensados para el
reporte (`IX_Accounts_ClientId`, `IX_Transactions_Account_Date`), proyecciones que resuelven
el filtrado y el ordenamiento en SQL en lugar de en memoria, y separación de lectura y
escritura (CQRS) para que las consultas no carguen agregados completos. La **escalabilidad**
se apoya en que los servicios no guardan estado en memoria y en que la mensajería desacopla
la escritura de la propagación: se pueden levantar varias instancias de cualquiera de los
dos y RabbitMQ reparte el trabajo entre los consumidores.

## Estructura del repositorio

Es un monorepo. Cada microservicio es una solución `.sln` independiente, con su propio
ciclo de vida y su propia base de datos.

```
.
├── docker-compose.yml      Levanta todo el entorno local
├── BaseDatos.sql           Esquema de las dos bases de datos
├── gateway/nginx.conf      Enrutamiento del gateway
├── postman/                Colección de validación de los endpoints
└── src
    ├── customers           Microservicio de Clientes (Persona, Cliente)
    └── accounts            Microservicio de Cuentas (Cuenta, Movimiento)
```

¿Por qué monorepo? Porque el entregable se evalúa clonando y ejecutando. Con un solo
repositorio, levantar la solución completa es un comando; con repositorios separados
habría que clonar dos veces y conectar las redes de Docker a mano.

Los dos microservicios se comunican de forma **asincrónica** a través de un broker de
mensajes. No se llaman entre sí por HTTP.

## Cómo levantarlo

Requisito: Docker Desktop.

```bash
docker compose up -d --build
```

Eso levanta SQL Server y RabbitMQ, crea el esquema ejecutando `BaseDatos.sql` y arranca
las dos APIs. No hay que ejecutar nada a mano. El script es idempotente, así que se puede
volver a levantar cuantas veces se quiera sin perder datos ni provocar errores.

| Servicio | URL |
|---|---|
| **API (gateway)** | **http://localhost:5000** |
| Customers API (directo) | http://localhost:5001 |
| Accounts API (directo) | http://localhost:5002 |
| Health check | `/status` en el gateway y en cada API |
| Swagger UI | `/swagger` en cada API |
| SQL Server | `localhost:1433` |
| RabbitMQ (panel) | http://localhost:15672 |

Para bajar todo:

```bash
docker compose down          # conserva los datos
docker compose down -v       # borra también la base de datos
```

## Conexión a la base de datos

SQL Server queda expuesto en el puerto 1433 del host. Se puede conectar con SSMS,
Azure Data Studio, DBeaver o cualquier cliente.

| Parámetro | Valor |
|---|---|
| Host | `localhost` |
| Puerto | `1433` |
| Usuario | `sa` |
| Contraseña | `DevSu*Bank2026` |
| Bases de datos | `DevSuBankCustomers`, `DevSuBankAccounts` |

Cadena de conexión:

```
Server=localhost,1433;Database=DevSuBankCustomers;User Id=sa;Password=DevSu*Bank2026;TrustServerCertificate=True;
```

También se puede consultar desde el contenedor:

```bash
docker exec -it devsubank-sqlserver /opt/mssql-tools18/bin/sqlcmd   -C -S localhost -U sa -P 'DevSu*Bank2026' -d DevSuBankCustomers
```

Desde adentro de la red de Docker el host no es `localhost` sino `sqlserver`, que es el
nombre del servicio. Por eso las APIs se conectan con `Host=sqlserver`.

RabbitMQ usa el usuario `devsubank` con la misma contraseña, y su panel de administración
queda en http://localhost:15672.

Estas credenciales son solo para el entorno local. En un despliegue real irían en un
gestor de secretos.

## Rutas de la API

Las rutas están versionadas:

```
POST /api/v1/clientes
```

El enunciado pide que el endpoint se llame `/clientes`, y ese nombre se respeta: el
recurso es `clientes`, no `clients`. Lo que se agregó alrededor es el prefijo `/api`
—que aparece en la propia instrucción de Postman del enunciado— y el segmento de
versión.

Versionar desde el primer día es barato; hacerlo después no. Una vez que hay clientes
consumiendo la API, cambiar un contrato sin versión rompe a todo el mundo a la vez. Con
`v1` en la ruta, una `v2` puede convivir con la anterior mientras los consumidores
migran a su ritmo.

La colección de Postman apunta a las rutas versionadas.

## Colección de Postman

En `postman/DevSuBank.postman_collection.json` hay una colección con **33 peticiones y 51
validaciones** que recorren los casos de uso del enunciado en orden: crea los tres clientes,
sus cinco cuentas, los cuatro movimientos, provoca el error de saldo, genera el reporte y
termina comprobando que al eliminar un cliente sus cuentas quedan desactivadas.

Se importa en Postman y se ejecuta con el Collection Runner, con un **delay de 500 ms**
entre peticiones porque la propagación entre microservicios es asincrónica.

También se puede correr desde la línea de comandos:

```bash
npx newman run postman/DevSuBank.postman_collection.json --delay-request 500
```

La colección **debe ejecutarse sobre un entorno limpio**. Usa los números de cuenta exactos
del enunciado, que son únicos en la base, así que una segunda corrida sobre los mismos datos
falla por duplicados. Para repetirla:

```bash
docker compose down -v && docker compose up -d --build
```

Todas las peticiones apuntan al gateway mediante las variables `{{servidor}}` y `{{puerto}}`.

## Sobre el idioma del código

**El código está en inglés y los contratos de la API en español.** No es una
inconsistencia, es una decisión consciente.

Escribir el código en inglés es una buena práctica establecida: es el idioma de los
frameworks, de las librerías y de la comunidad. Mezclar idiomas dentro del código
produce cosas como `ClienteRepository` o `saldoDisponible` conviviendo con `IEnumerable`
y `CancellationToken`, y eso se vuelve incómodo de leer y de mantener.

Sin embargo, el enunciado fija de forma literal ciertos elementos: los endpoints deben
llamarse `/clientes`, `/cuentas`, `/movimientos` y `/reportes`; el parámetro de consulta
es `fecha`; el mensaje de error debe decir exactamente "Saldo no disponible"; y el JSON
del reporte trae claves como `"Saldo Disponible"`. Un contrato de API es una interfaz
pública acordada: si el cliente la especificó así, no se cambia porque al desarrollador
le parezca mejor en inglés.

La solución fue dejar la frontera bien marcada. Todo lo que está adentro (entidades,
servicios, repositorios, variables, pruebas, tablas de base de datos) está en inglés.
Lo que sale hacia afuera se traduce en un único lugar: los DTOs de la capa de
aplicación, usando `[JsonPropertyName]`. Así el dominio nunca contiene texto en
español y la traducción es explícita y fácil de encontrar.

Dicho eso: **lo ideal sería que los contratos también estuvieran en inglés**. Si esto
fuera un proyecto real, propondría negociar ese punto antes de construir, porque un
contrato en español obliga a mantener una capa de traducción para siempre.

La documentación (este README y los demás) está en español por practicidad.

## Documentación por microservicio

- [src/customers](src/customers/README.md) — arquitectura, capas, reglas de dependencia,
  endpoints y los eventos que publica.
- [src/accounts](src/accounts/README.md) — misma arquitectura, endpoints de cuentas,
  movimientos y reportes, y los eventos que consume.
