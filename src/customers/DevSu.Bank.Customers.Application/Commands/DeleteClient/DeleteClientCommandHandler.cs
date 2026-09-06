using DevSu.Bank.Customers.Application.SeedWork;
using DevSu.Bank.Customers.Domain.AggregateModels.ClientAggregate;
using DevSu.Bank.Customers.Domain.Resources;
using MediatR;

namespace DevSu.Bank.Customers.Application.Commands.DeleteClient
{
    public class DeleteClientCommandHandler(IClientRepository clientRepository)
        : IRequestHandler<DeleteClientCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteClientCommand request, CancellationToken cancellationToken)
        {
            var client = await clientRepository.GetSingleByIdAsync(request.Id)
                ?? throw new NotFoundException(Generals.ClientNotFound);

            client.Delete();

            clientRepository.Update(client);

            await clientRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
