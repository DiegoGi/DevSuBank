using DevSu.Bank.Accounts.Application.DTOs;
using DevSu.Bank.Accounts.Application.SeedWork;
using MediatR;

namespace DevSu.Bank.Accounts.Application.Queries.GetAccounts
{
    public record GetAccountsQuery(QueryOptions Options) : IRequest<PagedList<AccountResponse>>;
}
