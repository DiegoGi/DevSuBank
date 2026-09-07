using DevSu.Bank.Customers.Application.SeedWork;
using DevSu.Bank.Customers.Application.Services.Infrastructure;
using DevSu.Bank.Customers.Domain.AggregateModels.ClientAggregate;
using DevSu.Bank.Customers.Domain.Resources;
using MediatR;
using System.Linq.Expressions;

namespace DevSu.Bank.Customers.Application.Commands.CreateClient
{
    public class CreateClientCommandHandler(
        IClientRepository clientRepository,
        IPasswordHasherService passwordHasherService)
        : IRequestHandler<CreateClientCommand, int>
    {
        public async Task<int> Handle(CreateClientCommand request, CancellationToken cancellationToken)
        {
            await EnsureIsNotRegisteredAsync(client => client.ClientId == request.ClientId, nameof(request.ClientId));
            await EnsureIsNotRegisteredAsync(client => client.Identification == request.Identification, nameof(request.Identification));

            var client = new Client(request.Name, request.Gender, request.Age, request.Identification,
                request.Address, request.Phone, request.ClientId, passwordHasherService.Hash(request.Password));

            await clientRepository.CreateAsync(client);
            await clientRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return client.Id;
        }

        private async Task EnsureIsNotRegisteredAsync(
            Expression<Func<Client, bool>> expression, string parameterName)
        {
            if (await clientRepository.ExistsAsync(expression))
            {
                throw new ApplicationValidationException(string.Format(Generals.ValueAlreadyRegistered, parameterName));
            }
        }
    }
}
