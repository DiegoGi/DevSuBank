using DevSu.Bank.Accounts.Application.DTOs;
using MediatR;

namespace DevSu.Bank.Accounts.Application.Queries.GetAccountById
{
    public record GetAccountByIdQuery(int Id) : IRequest<AccountResponse>;
}
