using DevSu.Bank.Accounts.Domain.AggregateModels.ClientAggregate;
using MediatR;

namespace DevSu.Bank.Accounts.Application.Commands.SynchronizeClient
{
    public class SynchronizeClientCommandHandler(IClientRepository clientRepository)
        : IRequestHandler<SynchronizeClientCommand, Unit>
    {
        public async Task<Unit> Handle(SynchronizeClientCommand request, CancellationToken cancellationToken)
        {
            var client = await clientRepository.GetSingleByIdAsync(request.Id);

            if (client is null)
            {
                await clientRepository.CreateAsync(new Client(request.Id, request.Name, request.Status));
            }
            else
            {
                client.Update(request.Name, request.Status);
                clientRepository.Update(client);
            }

            await clientRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
