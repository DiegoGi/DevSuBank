using DevSu.Bank.Customers.Domain.ValueObjects;
using MediatR;

namespace DevSu.Bank.Customers.Application.Commands.CreateClient
{
    public record CreateClientCommand(string Name, Gender Gender, int Age, string Identification,
        string? Address, string? Phone, string ClientId, string Password) : IRequest<int>;
}
