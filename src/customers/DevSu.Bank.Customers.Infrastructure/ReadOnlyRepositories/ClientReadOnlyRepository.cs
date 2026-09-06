using DevSu.Bank.Customers.Application.ReadOnlyModels;
using DevSu.Bank.Customers.Application.ReadOnlyRepositories;
using DevSu.Bank.Customers.Infrastructure.ReadOnlyDataContext;
using DevSu.Bank.Customers.Infrastructure.SeedWork;

namespace DevSu.Bank.Customers.Infrastructure.ReadOnlyRepositories
{
    public class ClientReadOnlyRepository(ReadOnlyContext readOnlyContext)
        : BaseReadOnlyRepository<int, Client>(readOnlyContext), IClientReadOnlyRepository
    {
    }
}
