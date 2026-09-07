using DevSu.Bank.Accounts.Application.DTOs;
using DevSu.Bank.Accounts.Application.SeedWork;
using MediatR;

namespace DevSu.Bank.Accounts.Application.Queries.GetTransactions
{
    public record GetTransactionsQuery(QueryOptions Options, string? AccountNumber, DateTime? From, DateTime? To)
        : IRequest<PagedList<TransactionResponse>>;
}
