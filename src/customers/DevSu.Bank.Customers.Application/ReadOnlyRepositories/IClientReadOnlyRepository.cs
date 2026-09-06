using DevSu.Bank.Customers.Application.ReadOnlyModels;
using DevSu.Bank.Customers.Domain.SeedWork;

namespace DevSu.Bank.Customers.Application.ReadOnlyRepositories
{
    public interface IClientReadOnlyRepository : IReadOnlyRepository<int, Client>
    {
    }
}
