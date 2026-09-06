using DevSu.Bank.Customers.Application.SeedWork;
using DevSu.Bank.Customers.Application.Services.Infrastructure;
using DevSu.Bank.Customers.Domain.AggregateModels.ClientAggregate;
using DevSu.Bank.Customers.Domain.Resources;
using MediatR;

namespace DevSu.Bank.Customers.Application.Commands.ChangeClientPassword
{
    public class ChangeClientPasswordCommandHandler(
        IClientRepository clientRepository,
        IPasswordHasherService passwordHasherService)
        : IRequestHandler<ChangeClientPasswordCommand, Unit>
    {
        public async Task<Unit> Handle(ChangeClientPasswordCommand request, CancellationToken cancellationToken)
        {
            var client = await clientRepository.GetSingleByIdAsync(request.Id)
                ?? throw new NotFoundException(Generals.ClientNotFound);

            client.ChangePassword(passwordHasherService.Hash(request.Password));

            clientRepository.Update(client);

            await clientRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
