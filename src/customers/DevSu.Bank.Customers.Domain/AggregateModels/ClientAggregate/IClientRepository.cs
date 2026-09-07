using DevSu.Bank.Customers.Domain.SeedWork;

namespace DevSu.Bank.Customers.Domain.AggregateModels.ClientAggregate
{
    public interface IClientRepository : IAggregateRepository<int, Client>
    {
    }
}
