using MediatR;

namespace DevSu.Bank.Customers.Application.Commands.ChangeClientPassword
{
    public record ChangeClientPasswordCommand(int Id, string Password) : IRequest<Unit>;
}
