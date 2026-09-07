using DevSu.Bank.Customers.Application.SeedWork;
using DevSu.Bank.Customers.Application.Services.Infrastructure;
using DevSu.Bank.Customers.Domain.AggregateModels.ClientAggregate;
using DevSu.Bank.Customers.Domain.Resources;
using MediatR;

namespace DevSu.Bank.Customers.Application.Commands.CreateClient
{
    public class CreateClientCommandHandler(
        IClientRepository clientRepository,
        IPasswordHasherService passwordHasherService)
        : IRequestHandler<CreateClientCommand, int>
    {
        public async Task<int> Handle(CreateClientCommand request, CancellationToken cancellationToken)
        {
            if (await clientRepository.ExistsAsync(client => client.ClientId == request.ClientId))
            {
                throw new ApplicationValidationException(Generals.ClientIdAlreadyRegistered);
            }

            if (await clientRepository.ExistsAsync(client => client.Identification == request.Identification))
            {
                throw new ApplicationValidationException(Generals.IdentificationAlreadyRegistered);
            }

            var client = new Client(request.Name, request.Gender, request.Age, request.Identification,
                request.Address, request.Phone, request.ClientId, passwordHasherService.Hash(request.Password));

            await clientRepository.CreateAsync(client);
            await clientRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return client.Id;
        }
    }
}
