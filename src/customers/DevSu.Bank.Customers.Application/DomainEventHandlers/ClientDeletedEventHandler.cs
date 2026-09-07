using DevSu.Bank.Customers.Application.Services.Infrastructure;
using DevSu.Bank.Customers.Domain.AggregateModels.ClientAggregate.Events;
using MediatR;

namespace DevSu.Bank.Customers.Application.DomainEventHandlers
{
    public class ClientDeletedEventHandler(IEventBusService eventBusService)
        : INotificationHandler<ClientDeletedEvent>
    {
        public Task Handle(ClientDeletedEvent notification, CancellationToken cancellationToken)
        {
            return eventBusService.PublishClientDeletedAsync(notification.Client.Id, cancellationToken);
        }
    }
}
