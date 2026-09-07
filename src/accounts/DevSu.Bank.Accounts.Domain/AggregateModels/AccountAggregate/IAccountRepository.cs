using DevSu.Bank.Accounts.Domain.SeedWork;

namespace DevSu.Bank.Accounts.Domain.AggregateModels.AccountAggregate
{
    public interface IAccountRepository : IAggregateRepository<int, Account>
    {
    }
}
