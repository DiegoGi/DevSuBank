using DevSu.Bank.Customers.Application.DTOs;
using DevSu.Bank.Customers.Application.SeedWork;
using MediatR;

namespace DevSu.Bank.Customers.Application.Queries.GetClients
{
    public record GetClientsQuery(QueryOptions Options) : IRequest<PagedList<ClientResponse>>;
}
