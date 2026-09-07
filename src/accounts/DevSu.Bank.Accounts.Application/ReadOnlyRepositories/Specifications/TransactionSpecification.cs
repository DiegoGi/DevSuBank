using DevSu.Bank.Accounts.Application.ReadOnlyModels;
using DevSu.Bank.Accounts.Application.SeedWork;
using DevSu.Bank.Accounts.Domain.SeedWork;
using System.Linq.Expressions;

namespace DevSu.Bank.Accounts.Application.ReadOnlyRepositories.Specifications
{
    public class TransactionSpecification : Specification<Transaction>
    {
        private readonly string? _accountNumber;
        private readonly DateTime? _from;
        private readonly DateTime? _to;

        public TransactionSpecification(QueryOptions options, string? accountNumber, DateTime? from, DateTime? to)
        {
            _accountNumber = accountNumber?.Trim();
            _from = from?.Date;
            _to = to?.Date.AddDays(1).AddTicks(-1);

            PageNumber = options.Page;
            PageSize = options.PageSize;

            Includes.Add(transaction => transaction.Account);

            Orders.Add(new OrderBy
            {
                SortField = string.IsNullOrWhiteSpace(options.SortBy)
                    ? nameof(Transaction.TransactionDate)
                    : options.SortBy,
                Type = options.SortOrder
            });
        }

        public override Expression<Func<Transaction, bool>> ToExpression()
        {
            return transaction =>
                (_accountNumber == null || transaction.Account.AccountNumber == _accountNumber)
                && (_from == null || transaction.TransactionDate >= _from)
                && (_to == null || transaction.TransactionDate <= _to);
        }
    }
}
