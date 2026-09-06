using MediatR;

namespace DevSu.Bank.Customers.Application.Commands.DeleteClient
{
    public record DeleteClientCommand(int Id) : IRequest<Unit>;
}
