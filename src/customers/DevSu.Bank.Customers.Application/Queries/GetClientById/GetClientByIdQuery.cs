using DevSu.Bank.Customers.Application.DTOs;
using MediatR;

namespace DevSu.Bank.Customers.Application.Queries.GetClientById
{
    public record GetClientByIdQuery(int Id) : IRequest<ClientResponse>;
}
