# DevSu Bank

Solución para la prueba técnica de Devsu: una arquitectura de microservicios para un
sistema bancario simple (clientes, cuentas y movimientos).

## Estructura del repositorio

Es un monorepo. Cada microservicio es una solución `.sln` independiente, con su propio
ciclo de vida y su propia base de datos.

```
.
├── docker-compose.yml      Levanta todo el entorno local
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

Eso levanta SQL Server, crea el esquema ejecutando `BaseDatos.sql` y arranca las dos
APIs. No hay que ejecutar nada a mano. El script es idempotente, así que se puede
volver a levantar cuantas veces se quiera sin perder datos ni provocar errores.

| Servicio | URL |
|---|---|
| Customers API | http://localhost:5001 |
| Accounts API | http://localhost:5002 |
| Health check | `/status` en cada API |
| Swagger UI | `/swagger` en cada API |
| SQL Server | `localhost:1433` |

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

- [src/customers](src/customers/README.md) — arquitectura, capas y reglas de dependencia.
- [src/accounts](src/accounts/README.md) — misma arquitectura, aplicada a cuentas y movimientos.
