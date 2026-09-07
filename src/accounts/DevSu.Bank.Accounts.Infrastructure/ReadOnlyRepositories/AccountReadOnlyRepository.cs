using DevSu.Bank.Accounts.Application.ReadOnlyModels;
using DevSu.Bank.Accounts.Application.ReadOnlyRepositories;
using DevSu.Bank.Accounts.Infrastructure.ReadOnlyDataContext;
using DevSu.Bank.Accounts.Infrastructure.SeedWork;

namespace DevSu.Bank.Accounts.Infrastructure.ReadOnlyRepositories
{
    public class AccountReadOnlyRepository(ReadOnlyContext readOnlyContext)
        : BaseReadOnlyRepository<int, Account>(readOnlyContext), IAccountReadOnlyRepository
    {
    }
}
