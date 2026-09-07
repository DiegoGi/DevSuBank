using DevSu.Bank.Accounts.Domain.SeedWork;
using System.Linq.Expressions;

namespace DevSu.Bank.Accounts.Domain.AggregateModels.AccountAggregate.Specifications
{
    public class AccountsByClientSpecification : Specification<Account>
    {
        private readonly int _clientId;

        public AccountsByClientSpecification(int clientId)
        {
            _clientId = clientId;

            DisablePagination();
        }

        public override Expression<Func<Account, bool>> ToExpression()
        {
            return account => account.ClientId == _clientId && account.Status;
        }
    }
}
