using DevSu.Bank.Customers.Application.DTOs;
using DevSu.Bank.Customers.Application.ReadOnlyRepositories;
using DevSu.Bank.Customers.Application.ReadOnlyRepositories.Specifications;
using DevSu.Bank.Customers.Application.SeedWork;
using MediatR;

namespace DevSu.Bank.Customers.Application.Queries.GetClients
{
    public class GetClientsQueryHandler(IClientReadOnlyRepository clientReadOnlyRepository)
        : IRequestHandler<GetClientsQuery, PagedList<ClientResponse>>
    {
        public async Task<PagedList<ClientResponse>> Handle(GetClientsQuery request, CancellationToken cancellationToken)
        {
            var specification = new ClientSpecification(request.Options);

            var total = await clientReadOnlyRepository.CountAsync(specification);
            var clients = await clientReadOnlyRepository.ListAsync(specification, ClientResponse.Projection);

            return new PagedList<ClientResponse>(total, clients);
        }
    }
}
