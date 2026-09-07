using DevSu.Bank.Accounts.Domain.ValueObjects;
using MediatR;

namespace DevSu.Bank.Accounts.Application.Commands.CreateAccount
{
    public record CreateAccountCommand(string AccountNumber, AccountType AccountType, decimal InitialBalance,
        int ClientId) : IRequest<int>;
}
