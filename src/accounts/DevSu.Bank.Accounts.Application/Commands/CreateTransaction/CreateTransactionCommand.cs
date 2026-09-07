using MediatR;

namespace DevSu.Bank.Accounts.Application.Commands.CreateTransaction
{
    public record CreateTransactionCommand(string AccountNumber, decimal Amount) : IRequest<long>;
}
