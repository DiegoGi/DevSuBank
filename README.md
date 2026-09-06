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

| Servicio | URL |
|---|---|
| Customers API | http://localhost:5001 |
| Accounts API | http://localhost:5002 |
| Health check | `/status` en cada API |
| Swagger UI | `/swagger` en cada API |
| SQL Server | `localhost:1433` (usuario `sa`) |

Para bajar todo:

```bash
docker compose down          # conserva los datos
docker compose down -v       # borra también la base de datos
```

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
presentación, usando `[JsonPropertyName]`. Así el dominio nunca contiene texto en
español y la traducción es explícita y fácil de encontrar.

Dicho eso: **lo ideal sería que los contratos también estuvieran en inglés**. Si esto
fuera un proyecto real, propondría negociar ese punto antes de construir, porque un
contrato en español obliga a mantener una capa de traducción para siempre.

La documentación (este README y los demás) está en español por practicidad.

## Documentación por microservicio

- [src/customers](src/customers/README.md) — arquitectura, capas y reglas de dependencia.
- [src/accounts](src/accounts/README.md) — misma arquitectura, aplicada a cuentas y movimientos.
