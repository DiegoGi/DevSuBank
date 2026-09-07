using Asp.Versioning;
using DevSu.Bank.Accounts.Application.Commands.CreateTransaction;
using DevSu.Bank.Accounts.Application.DTOs;
using DevSu.Bank.Accounts.Application.Queries.GetTransactionById;
using DevSu.Bank.Accounts.Application.Queries.GetTransactions;
using DevSu.Bank.Accounts.Application.SeedWork;
using DevSu.Bank.Accounts.Domain.SeedWork;
using DevSu.Bank.Accounts.Presentation.Api.SeedWork;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DevSu.Bank.Accounts.Presentation.Api.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/movimientos")]
    public sealed class TransactionsController(IMediator mediator) : ApiControllerBase
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateAsync([FromBody] CreateTransactionRequest request,
            CancellationToken cancellationToken)
        {
            var command = new CreateTransactionCommand(request.AccountNumber, request.Amount);

            var id = await mediator.Send(command, cancellationToken);

            return Created($"{Request.Path}/{id}", new { id });
        }
        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync(long id, CancellationToken cancellationToken)
        {
            return Ok(await mediator.Send(new GetTransactionByIdQuery(id), cancellationToken));
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedList<TransactionResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAsync([FromQuery] string? numeroCuenta, [FromQuery] DateTime? desde,
            [FromQuery] DateTime? hasta, [FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20,
            [FromQuery] string? ordenarPor = null, [FromQuery] SortOrder orden = SortOrder.Descending,
            CancellationToken cancellationToken = default)
        {
            var options = new QueryOptions
            {
                Page = pagina,
                PageSize = tamanoPagina,
                SortBy = ordenarPor,
                SortOrder = orden
            };

            var query = new GetTransactionsQuery(options, numeroCuenta, desde, hasta);

            return Ok(await mediator.Send(query, cancellationToken));
        }
    }
}