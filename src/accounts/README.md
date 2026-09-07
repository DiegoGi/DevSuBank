# DevSu.Bank.Accounts

Microservicio de **Cuentas**. Maneja los agregados `Account` y `Transaction`.

Expone los endpoints `/cuentas`, `/movimientos` y `/reportes`. Se entera de los cambios
en los clientes escuchando los eventos que publica el microservicio de Clientes, sin
llamarlo por HTTP.

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
cambian aunque cambie la tecnología. Aquí vive, por ejemplo, la regla de que no se puede
retirar de una cuenta sin saldo suficiente.

**Application** — Los casos de uso: registrar un movimiento, consultar una cuenta,
generar el estado de cuenta. Define *qué* necesita el sistema mediante interfaces (por
ejemplo, un repositorio), pero no sabe *cómo* se resuelven.

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
DevSu.Bank.Accounts.Domain                  entidades, value objects, SeedWork
DevSu.Bank.Accounts.Application             casos de uso, DTOs, behaviors
DevSu.Bank.Accounts.Infrastructure          EF Core, repositorios, servicios externos
DevSu.Bank.Accounts.Presentation.Api        API REST, middleware, versionado

+ un proyecto *.UnitTest por cada capa
```

## Endpoints

Se acceden por el gateway (`http://localhost:5000`) o directo contra el servicio
(`http://localhost:5002`).

| Verbo | Ruta | Qué hace |
|---|---|---|
| `POST` | `/cuentas` | Crea una cuenta. Valida que el cliente exista y esté activo |
| `GET` | `/cuentas` | Lista cuentas con el nombre de su cliente. Acepta `busqueda`, `pagina`, `tamanoPagina`, `ordenarPor`, `orden` |
| `GET` | `/cuentas/{id}` | Una cuenta |
| `PUT` | `/cuentas/{id}` | Actualiza tipo y estado |
| `POST` | `/movimientos` | Registra un movimiento. El valor va con signo: positivo deposita, negativo retira |
| `GET` | `/movimientos` | Lista movimientos. Acepta `numeroCuenta`, `desde`, `hasta` y los de paginación |
| `GET` | `/movimientos/{id}` | Un movimiento |
| `GET` | `/reportes` | Estado de cuenta. Requiere `fechaInicial`, `fechaFinal` y `cliente` |

Del movimiento solo se puede crear y consultar. **No hay actualización ni borrado**: un
libro contable no se edita. Si un movimiento está mal, se registra otro que lo revierte.

El movimiento se registra contra el **número de cuenta**, no contra el id interno, que es
como funciona en banca y como lo plantea el enunciado.

### Reglas que aplica

- Un retiro que deje el saldo en negativo se rechaza con **`400 "Saldo no disponible"`**
- Un movimiento de valor cero se rechaza
- Una cuenta inactiva no acepta movimientos
- El tipo de movimiento (depósito o retiro) se **deriva del signo del valor**, así es
  imposible guardar un depósito con monto negativo
- `InitialBalance` nunca cambia; solo se mueve `CurrentBalance`

## Eventos que consume

Este servicio mantiene una copia local de los clientes, alimentada por RabbitMQ:

| Evento | Reacción |
|---|---|
| `devsu-bank:client-registered` | Inserta el cliente en la réplica |
| `devsu-bank:client-updated` | Actualiza nombre y estado |
| `devsu-bank:client-deleted` | Marca el cliente inactivo y **desactiva todas sus cuentas** |

Gracias a esa réplica, el servicio sigue operando por completo aunque Clientes esté caído:
crea cuentas, registra movimientos y genera reportes con el nombre del cliente.

## Cómo correrlo

Lo normal es levantarlo con el resto del entorno desde la raíz del repositorio:

```bash
docker compose up -d --build
```

Queda en http://localhost:5002 (health check en `/status`, Swagger en `/swagger`).

Para ejecutarlo suelto durante el desarrollo:

```bash
dotnet run --project DevSu.Bank.Accounts.Presentation.Api
```

Necesita una instancia de SQL Server accesible. La conexión se arma con cuatro variables
de configuración: `Host`, `Database`, `User` y `Password`.

## Notas técnicas

- **.NET 10.** Todos los proyectos apuntan a `net10.0`.
- **Sin `appsettings.json`.** Toda la configuración entra por variables de entorno, se usa secrec.json de acuerdo a buenas practicas de desarrollo local. Además de la base de
  datos, necesita `MessageBrokerHost`, `MessageBrokerUser` y `MessageBrokerPassword`.
- **Las fechas se guardan en UTC.** Los filtros por rango deben usar fechas UTC.
