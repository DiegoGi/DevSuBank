using DevSu.Bank.Customers.Application.Services.Infrastructure;
using DevSu.Bank.Customers.Domain.AggregateModels.ClientAggregate.Events;
using MediatR;

namespace DevSu.Bank.Customers.Application.DomainEventHandlers
{
    public class ClientCreatedEventHandler(IEventBusService eventBusService)
        : INotificationHandler<ClientCreatedEvent>
    {
        public Task Handle(ClientCreatedEvent notification, CancellationToken cancellationToken)
        {
            var client = notification.Client;

            return eventBusService.PublishClientRegisteredAsync(client.Id, client.Name, client.Status, cancellationToken);
        }
    }
}
