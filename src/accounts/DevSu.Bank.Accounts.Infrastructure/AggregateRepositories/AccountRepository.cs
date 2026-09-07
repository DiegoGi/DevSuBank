using DevSu.Bank.Accounts.Domain.AggregateModels.AccountAggregate;
using DevSu.Bank.Accounts.Infrastructure.AggregateDataContext;
using DevSu.Bank.Accounts.Infrastructure.SeedWork;

namespace DevSu.Bank.Accounts.Infrastructure.AggregateRepositories
{
    public class AccountRepository(AggregateContext mainContext)
        : BaseAggregateRepository<int, Account>(mainContext), IAccountRepository
    {
    }
}
