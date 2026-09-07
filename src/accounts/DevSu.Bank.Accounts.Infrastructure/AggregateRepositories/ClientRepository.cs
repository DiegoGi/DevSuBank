using DevSu.Bank.Accounts.Domain.AggregateModels.ClientAggregate;
using DevSu.Bank.Accounts.Infrastructure.AggregateDataContext;
using DevSu.Bank.Accounts.Infrastructure.SeedWork;

namespace DevSu.Bank.Accounts.Infrastructure.AggregateRepositories
{
    public class ClientRepository(AggregateContext mainContext)
        : BaseAggregateRepository<int, Client>(mainContext), IClientRepository
    {
    }
}
