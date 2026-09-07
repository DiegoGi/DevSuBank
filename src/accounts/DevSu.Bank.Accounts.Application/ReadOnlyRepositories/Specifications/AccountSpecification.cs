using DevSu.Bank.Accounts.Application.ReadOnlyModels;
using DevSu.Bank.Accounts.Application.SeedWork;
using DevSu.Bank.Accounts.Domain.SeedWork;
using System.Linq.Expressions;

namespace DevSu.Bank.Accounts.Application.ReadOnlyRepositories.Specifications
{
    public class AccountSpecification : Specification<Account>
    {
        private readonly string? _search;

        public AccountSpecification(QueryOptions options)
        {
            _search = options.Search?.Trim();

            PageNumber = options.Page;
            PageSize = options.PageSize;

            Includes.Add(account => account.Client);

            Orders.Add(new OrderBy
            {
                SortField = string.IsNullOrWhiteSpace(options.SortBy) ? nameof(Account.AccountNumber) : options.SortBy,
                Type = options.SortOrder
            });
        }

        public override Expression<Func<Account, bool>> ToExpression()
        {
            if (string.IsNullOrWhiteSpace(_search))
            {
                return account => true;
            }

            return account => account.AccountNumber.Contains(_search) || account.Client.Name.Contains(_search);
        }
    }
}
