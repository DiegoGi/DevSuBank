using Asp.Versioning;
using DevSu.Bank.Accounts.Application.DTOs;
using DevSu.Bank.Accounts.Application.Queries.GetAccountStatement;
using DevSu.Bank.Accounts.Presentation.Api.SeedWork;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DevSu.Bank.Accounts.Presentation.Api.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/reportes")]
    public sealed class ReportsController(IMediator mediator) : ApiControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AccountStatementResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAccountStatementAsync([FromQuery] DateTime? fechaInicial,
            [FromQuery] DateTime? fechaFinal, [FromQuery] int cliente, CancellationToken cancellationToken)
        {
            var query = new GetAccountStatementQuery(cliente, fechaInicial, fechaFinal);

            return Ok(await mediator.Send(query, cancellationToken));
        }
    }
}
