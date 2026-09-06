using DevSu.Bank.Customers.Application.SeedWork;
using DevSu.Bank.Customers.Domain.AggregateModels.ClientAggregate;
using DevSu.Bank.Customers.Domain.Resources;
using MediatR;

namespace DevSu.Bank.Customers.Application.Commands.UpdateClient
{
    public class UpdateClientCommandHandler(IClientRepository clientRepository)
        : IRequestHandler<UpdateClientCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
        {
            var client = await clientRepository.GetSingleByIdAsync(request.Id)
                ?? throw new NotFoundException(Generals.ClientNotFound);

            client.UpdatePersonalInformation(request.Name, request.Gender, request.Age, request.Address,
                request.Phone);

            clientRepository.Update(client);

            await clientRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
