using DevSu.Bank.Accounts.Application.Commands.DeactivateClient;
using DevSu.Bank.Accounts.Infrastructure.IntegrationEvents;
using MassTransit;
using MediatR;

namespace DevSu.Bank.Accounts.Infrastructure.Consumers
{
    public class ClientDeletedConsumer(IMediator mediator) : IConsumer<ClientDeletedIntegrationEvent>
    {
        public Task Consume(ConsumeContext<ClientDeletedIntegrationEvent> context)
        {
            return mediator.Send(new DeactivateClientCommand(context.Message.Id), context.CancellationToken);
        }
    }
}
