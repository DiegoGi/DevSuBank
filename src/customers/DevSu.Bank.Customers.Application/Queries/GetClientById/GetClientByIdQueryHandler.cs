using DevSu.Bank.Customers.Application.DTOs;
using DevSu.Bank.Customers.Application.ReadOnlyRepositories;
using DevSu.Bank.Customers.Application.SeedWork;
using DevSu.Bank.Customers.Domain.Resources;
using MediatR;

namespace DevSu.Bank.Customers.Application.Queries.GetClientById
{
    public class GetClientByIdQueryHandler(IClientReadOnlyRepository clientReadOnlyRepository)
        : IRequestHandler<GetClientByIdQuery, ClientResponse>
    {
        public async Task<ClientResponse> Handle(GetClientByIdQuery request, CancellationToken cancellationToken)
        {
            var client = await clientReadOnlyRepository.GetSingleAsync(
                client => client.Id == request.Id && client.Status, ClientResponse.Projection);

            return client ?? throw new NotFoundException(Generals.ClientNotFound);
        }
    }
}
