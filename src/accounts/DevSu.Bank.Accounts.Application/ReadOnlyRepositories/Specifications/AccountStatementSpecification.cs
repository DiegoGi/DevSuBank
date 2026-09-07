using DevSu.Bank.Accounts.Application.ReadOnlyModels;
using DevSu.Bank.Accounts.Domain.SeedWork;
using System.Linq.Expressions;

namespace DevSu.Bank.Accounts.Application.ReadOnlyRepositories.Specifications
{
    public class AccountStatementSpecification : Specification<Transaction>
    {
        private readonly int _clientId;
        private readonly DateTime _from;
        private readonly DateTime _to;

        public AccountStatementSpecification(int clientId, DateTime from, DateTime to)
        {
            _clientId = clientId;
            _from = from.Date;
            _to = to.Date.AddDays(1).AddTicks(-1);

            DisablePagination();

            Orders.Add(new OrderBy
            {
                SortField = nameof(Transaction.TransactionDate),
                Type = SortOrder.Descending
            });
        }

        public override Expression<Func<Transaction, bool>> ToExpression()
        {
            return transaction => transaction.Account.ClientId == _clientId
                                  && transaction.TransactionDate >= _from
                                  && transaction.TransactionDate <= _to;
        }
    }
}
