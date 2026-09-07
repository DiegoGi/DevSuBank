using DevSu.Bank.Accounts.Domain.ValueObjects;
using MediatR;

namespace DevSu.Bank.Accounts.Application.Commands.UpdateAccount
{
    public record UpdateAccountCommand(int Id, AccountType AccountType, bool Status) : IRequest<Unit>;
}
