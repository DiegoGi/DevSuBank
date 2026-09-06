# DevSu.Bank.Customers

Microservicio de **Clientes**. Maneja los agregados `Person` y `Customer`.

Expone el endpoint `/clientes` y publica eventos cuando un cliente cambia, para que el
microservicio de Cuentas se entere sin necesidad de llamarlo por HTTP.

## Arquitectura

La solución sigue **arquitectura limpia (Onion Architecture)**. La idea de fondo es
simple: el dominio está en el centro y no depende de nada; las capas de afuera dependen
de las de adentro, nunca al revés.

```
        ┌─────────────────────────────────────────┐
        │            Presentation                 │   API REST, funciones serverless
        │   ┌─────────────────────────────────┐   │
        │   │        Infrastructure           │   │   EF Core, SQL Server, broker
        │   │   ┌─────────────────────────┐   │   │
        │   │   │      Application        │   │   │   casos de uso, DTOs, contratos
        │   │   │   ┌─────────────────┐   │   │   │
        │   │   │   │     Domain      │   │   │   │   entidades y reglas de negocio
        │   │   │   └─────────────────┘   │   │   │
        │   │   └─────────────────────────┘   │   │
        │   └─────────────────────────────────┘   │
        └─────────────────────────────────────────┘

                 las flechas apuntan hacia adentro
```

### Las capas

**Domain** — El corazón. Entidades, objetos de valor y las reglas de negocio que no
cambian aunque cambie la tecnología. Aquí vive, por ejemplo, la regla de que un cliente
hereda de persona.

**Application** — Los casos de uso: crear un cliente, consultarlo, actualizarlo. Define
*qué* necesita el sistema mediante interfaces (por ejemplo, un repositorio), pero no
sabe *cómo* se resuelven.

**Infrastructure** — Las implementaciones concretas de lo que Application declaró.
Aquí está EF Core, el acceso a SQL Server, el cliente del broker de mensajes y
cualquier librería externa.

**Presentation** — El punto de entrada. Hoy es una API REST; podría ser también una
función serverless o un worker. Recibe peticiones, delega en Application y traduce el
resultado al contrato público.

### Reglas de dependencia

Esta es la parte que hay que respetar sin excepciones:

| Capa | Puede referenciar | Notas |
|---|---|---|
| **Domain** | Nada | No referencia ningún otro proyecto de la solución. Sin dependencias externas. |
| **Application** | Domain | Solo Domain. Aquí se *definen* los servicios y repositorios como interfaces. |
| **Infrastructure** | Application | Conoce Domain por referencia transitiva. Aquí se *implementan* esas interfaces y se usan los NuGets externos y todo lo que esté fuera del framework base. |
| **Presentation** | Infrastructure | Conoce el resto por transitividad. Aloja la API, servicios serverless, etc. |

En corto: **Domain no sabe que existe una base de datos.** Si mañana cambiamos SQL Server
por otra cosa, solo se toca Infrastructure.

Cada capa tiene su proyecto de pruebas espejo (`*.UnitTest`), y esas pruebas incluyen
validaciones automáticas de arquitectura con NetArchTest, que fallan el build si alguien
rompe una de las reglas de arriba.

## Proyectos

```
DevSu.Bank.Customers.Domain                  entidades, value objects, SeedWork
DevSu.Bank.Customers.Application             casos de uso, DTOs, behaviors
DevSu.Bank.Customers.Infrastructure          EF Core, repositorios, servicios externos
DevSu.Bank.Customers.Presentation.Api        API REST, middleware, versionado

+ un proyecto *.UnitTest por cada capa
```

## Cómo correrlo

Lo normal es levantarlo con el resto del entorno desde la raíz del repositorio:

```bash
docker compose up -d --build
```

Queda en http://localhost:5001 (health check en `/status`, Swagger en `/swagger`).

Para ejecutarlo suelto durante el desarrollo:

```bash
dotnet run --project DevSu.Bank.Customers.Presentation.Api
```

Necesita una instancia de SQL Server accesible. La conexión se arma con cuatro variables
de configuración: `Host`, `Database`, `User` y `Password`.

## Notas técnicas

- **.NET 10.** Todos los proyectos apuntan a `net10.0`.
- **Sin `appsettings.json`.** Toda la configuración entra por variables de entorno, que
  es lo que corresponde para un servicio que corre en contenedor.
