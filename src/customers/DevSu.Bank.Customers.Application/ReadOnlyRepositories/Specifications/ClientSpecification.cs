using DevSu.Bank.Customers.Application.ReadOnlyModels;
using DevSu.Bank.Customers.Application.SeedWork;
using DevSu.Bank.Customers.Domain.SeedWork;
using System.Linq.Expressions;

namespace DevSu.Bank.Customers.Application.ReadOnlyRepositories.Specifications
{
    public class ClientSpecification : Specification<Client>
    {
        private readonly string? _search;

        public ClientSpecification(QueryOptions options)
        {
            _search = options.Search?.Trim();

            PageNumber = options.Page;
            PageSize = options.PageSize;

            Orders.Add(new OrderBy
            {
                SortField = string.IsNullOrWhiteSpace(options.SortBy) ? nameof(Client.Name) : options.SortBy,
                Type = options.SortOrder
            });
        }

        public override Expression<Func<Client, bool>> ToExpression()
        {
            if (string.IsNullOrWhiteSpace(_search))
            {
                return client => client.Status;
            }

            return client => client.Status
                             && (client.Name.Contains(_search)
                                 || client.Identification.Contains(_search)
                                 || client.ClientId.Contains(_search));
        }
    }
}
