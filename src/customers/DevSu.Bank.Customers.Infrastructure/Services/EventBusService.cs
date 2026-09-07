using DevSu.Bank.Customers.Application.Services.Infrastructure;
using DevSu.Bank.Customers.Infrastructure.IntegrationEvents;
using MassTransit;

namespace DevSu.Bank.Customers.Infrastructure.Services
{
    public class EventBusService(IPublishEndpoint publishEndpoint) : IEventBusService
    {
        public Task PublishClientRegisteredAsync(int id, string name, bool status,
            CancellationToken cancellationToken = default)
        {
            return publishEndpoint.Publish(new ClientRegisteredIntegrationEvent(id, name, status),
                cancellationToken);
        }

        public Task PublishClientUpdatedAsync(int id, string name, bool status,
            CancellationToken cancellationToken = default)
        {
            return publishEndpoint.Publish(new ClientUpdatedIntegrationEvent(id, name, status), cancellationToken);
        }

        public Task PublishClientDeletedAsync(int id, CancellationToken cancellationToken = default)
        {
            return publishEndpoint.Publish(new ClientDeletedIntegrationEvent(id), cancellationToken);
        }
    }
}
