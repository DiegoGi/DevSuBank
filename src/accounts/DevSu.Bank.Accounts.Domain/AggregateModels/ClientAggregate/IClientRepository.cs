using DevSu.Bank.Accounts.Domain.SeedWork;

namespace DevSu.Bank.Accounts.Domain.AggregateModels.ClientAggregate
{
    public interface IClientRepository : IAggregateRepository<int, Client>
    {
    }
}
