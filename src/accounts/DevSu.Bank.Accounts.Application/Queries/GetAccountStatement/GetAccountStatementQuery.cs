using DevSu.Bank.Accounts.Application.DTOs;
using MediatR;

namespace DevSu.Bank.Accounts.Application.Queries.GetAccountStatement
{
    public record GetAccountStatementQuery(int ClientId, DateTime? From, DateTime? To)
        : IRequest<IEnumerable<AccountStatementResponse>>;
}
