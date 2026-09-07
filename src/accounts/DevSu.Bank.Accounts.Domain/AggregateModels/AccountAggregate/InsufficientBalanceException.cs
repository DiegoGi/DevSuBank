using DevSu.Bank.Accounts.Domain.Resources;
using DevSu.Bank.Accounts.Domain.SeedWork;

namespace DevSu.Bank.Accounts.Domain.AggregateModels.AccountAggregate
{
    public class InsufficientBalanceException() : DomainValidationException(Generals.InsufficientBalance);
}
