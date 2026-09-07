using DevSu.Bank.Customers.Application.Services.Infrastructure;
using DevSu.Bank.Customers.Domain.AggregateModels.ClientAggregate.Events;
using MediatR;

namespace DevSu.Bank.Customers.Application.DomainEventHandlers
{
    public class ClientUpdatedEventHandler(IEventBusService eventBusService)
        : INotificationHandler<ClientUpdatedEvent>
    {
        public Task Handle(ClientUpdatedEvent notification, CancellationToken cancellationToken)
        {
            var client = notification.Client;

            return eventBusService.PublishClientUpdatedAsync(client.Id, client.Name, client.Status, cancellationToken);
        }
    }
}
