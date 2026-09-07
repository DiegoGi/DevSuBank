using DevSu.Bank.Accounts.Application.DTOs;
using MediatR;

namespace DevSu.Bank.Accounts.Application.Queries.GetTransactionById
{
    public record GetTransactionByIdQuery(long Id) : IRequest<TransactionResponse>;
}
