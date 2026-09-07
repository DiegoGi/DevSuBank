using Asp.Versioning;
using DevSu.Bank.Accounts.Application.Commands.CreateAccount;
using DevSu.Bank.Accounts.Application.Commands.UpdateAccount;
using DevSu.Bank.Accounts.Application.DTOs;
using DevSu.Bank.Accounts.Application.Queries.GetAccountById;
using DevSu.Bank.Accounts.Application.Queries.GetAccounts;
using DevSu.Bank.Accounts.Application.SeedWork;
using DevSu.Bank.Accounts.Domain.SeedWork;
using DevSu.Bank.Accounts.Presentation.Api.SeedWork;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DevSu.Bank.Accounts.Presentation.Api.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/cuentas")]
    public sealed class AccountsController(IMediator mediator) : ApiControllerBase
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAsync([FromBody] CreateAccountRequest request,
            CancellationToken cancellationToken)
        {
            var command = new CreateAccountCommand(request.AccountNumber, request.AccountType,
                request.InitialBalance, request.ClientId);

            var id = await mediator.Send(command, cancellationToken);

            return Created($"{Request.Path}/{id}", new { id });
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateAccountRequest request,
            CancellationToken cancellationToken)
        {
            await mediator.Send(new UpdateAccountCommand(id, request.AccountType, request.Status), cancellationToken);

            return NoContent();
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(AccountResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return Ok(await mediator.Send(new GetAccountByIdQuery(id), cancellationToken));
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedList<AccountResponse>), StatusCodes.Status200OK)]
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

            return Ok(await mediator.Send(new GetAccountsQuery(options), cancellationToken));
        }
    }
}
