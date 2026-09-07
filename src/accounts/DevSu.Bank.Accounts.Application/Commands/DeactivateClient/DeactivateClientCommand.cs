using MediatR;

namespace DevSu.Bank.Accounts.Application.Commands.DeactivateClient
{
    public record DeactivateClientCommand(int Id) : IRequest<Unit>;
}
