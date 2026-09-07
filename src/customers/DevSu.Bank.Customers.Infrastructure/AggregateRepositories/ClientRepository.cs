using DevSu.Bank.Customers.Domain.AggregateModels.ClientAggregate;
using DevSu.Bank.Customers.Infrastructure.AggregateDataContext;
using DevSu.Bank.Customers.Infrastructure.SeedWork;

namespace DevSu.Bank.Customers.Infrastructure.AggregateRepositories
{
    public class ClientRepository(AggregateContext mainContext)
        : BaseAggregateRepository<int, Client>(mainContext), IClientRepository
    {
    }
}
