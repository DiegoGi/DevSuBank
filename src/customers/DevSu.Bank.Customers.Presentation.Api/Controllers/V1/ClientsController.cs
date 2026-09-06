using Asp.Versioning;
using DevSu.Bank.Customers.Application.Commands.ChangeClientPassword;
using DevSu.Bank.Customers.Application.Commands.CreateClient;
using DevSu.Bank.Customers.Application.Commands.DeleteClient;
using DevSu.Bank.Customers.Application.Commands.UpdateClient;
using DevSu.Bank.Customers.Application.DTOs;
using DevSu.Bank.Customers.Application.Queries.GetClientById;
using DevSu.Bank.Customers.Application.Queries.GetClients;
using DevSu.Bank.Customers.Application.SeedWork;
using DevSu.Bank.Customers.Domain.SeedWork;
using DevSu.Bank.Customers.Presentation.Api.SeedWork;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DevSu.Bank.Customers.Presentation.Api.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/clientes")]
    public sealed class ClientsController(IMediator mediator) : ApiControllerBase
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAsync([FromBody] CreateClientRequest request,
            CancellationToken cancellationToken)
        {
            var command = new CreateClientCommand(request.Name, request.Gender, request.Age,
                request.Identification, request.Address, request.Phone, request.ClientId, request.Password);

            var id = await mediator.Send(command, cancellationToken);

            return Created($"{Request.Path}/{id}", new { id });
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateClientRequest request,
            CancellationToken cancellationToken)
        {
            var command = new UpdateClientCommand(id, request.Name, request.Gender, request.Age,
                request.Address, request.Phone);

            await mediator.Send(command, cancellationToken);

            return NoContent();
        }

        [HttpPatch("{id:int}/contrasena")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ChangePasswordAsync(int id,
            [FromBody] ChangeClientPasswordRequest request, CancellationToken cancellationToken)
        {
            await mediator.Send(new ChangeClientPasswordCommand(id, request.Password), cancellationToken);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            await mediator.Send(new DeleteClientCommand(id), cancellationToken);

            return NoContent();
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ClientResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return Ok(await mediator.Send(new GetClientByIdQuery(id), cancellationToken));
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedList<ClientResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAsync([FromQuery] string? busqueda, [FromQuery] int pagina = 1,
            [FromQuery] int tamanoPagina = 20, [FromQuery] string? ordenarPor = null,
            [FromQuery] SortOrder orden = SortOrder.Ascending, CancellationToken cancellationToken = default)
        {
            var options = new QueryOptions
            {
                Search = busqueda,
                Page = pagina,
                PageSize = tamanoPagina,
                SortBy = ordenarPor,
                SortOrder = orden
            };

            return Ok(await mediator.Send(new GetClientsQuery(options), cancellationToken));
        }
    }
}
