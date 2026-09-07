namespace DevSu.Bank.Customers.Application.Services.Infrastructure
{
    public interface IEventBusService
    {
        Task PublishClientRegisteredAsync(int id, string name, bool status,
            CancellationToken cancellationToken = default);

        Task PublishClientUpdatedAsync(int id, string name, bool status,
            CancellationToken cancellationToken = default);

        Task PublishClientDeletedAsync(int id, CancellationToken cancellationToken = default);
    }
}
