using DevSu.Bank.Accounts.Application.Commands.SynchronizeClient;
using DevSu.Bank.Accounts.Infrastructure.IntegrationEvents;
using MassTransit;
using MediatR;

namespace DevSu.Bank.Accounts.Infrastructure.Consumers
{
    public class ClientUpdatedConsumer(IMediator mediator) : IConsumer<ClientUpdatedIntegrationEvent>
    {
        public Task Consume(ConsumeContext<ClientUpdatedIntegrationEvent> context)
        {
            var message = context.Message;

            return mediator.Send(new SynchronizeClientCommand(message.Id, message.Name, message.Status),
                context.CancellationToken);
        }
    }
}
