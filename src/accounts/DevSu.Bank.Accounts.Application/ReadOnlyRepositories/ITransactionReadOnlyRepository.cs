using DevSu.Bank.Accounts.Application.ReadOnlyModels;
using DevSu.Bank.Accounts.Domain.SeedWork;

namespace DevSu.Bank.Accounts.Application.ReadOnlyRepositories
{
    public interface ITransactionReadOnlyRepository : IReadOnlyRepository<long, Transaction>
    {
    }
}
