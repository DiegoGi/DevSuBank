using DevSu.Bank.Accounts.Application.Commands.SynchronizeClient;
using DevSu.Bank.Accounts.Infrastructure.IntegrationEvents;
using MassTransit;
using MediatR;

namespace DevSu.Bank.Accounts.Infrastructure.Consumers
{
    public class ClientRegisteredConsumer(IMediator mediator) : IConsumer<ClientRegisteredIntegrationEvent>
    {
        public Task Consume(ConsumeContext<ClientRegisteredIntegrationEvent> context)
        {
            var message = context.Message;

            return mediator.Send(new SynchronizeClientCommand(message.Id, message.Name, message.Status),
                context.CancellationToken);
        }
    }
}
