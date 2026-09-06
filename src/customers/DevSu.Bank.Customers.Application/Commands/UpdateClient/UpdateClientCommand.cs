using DevSu.Bank.Customers.Domain.ValueObjects;
using MediatR;

namespace DevSu.Bank.Customers.Application.Commands.UpdateClient
{
    public record UpdateClientCommand(int Id, string Name, Gender Gender, int Age, string? Address,
        string? Phone) : IRequest<Unit>;
}
