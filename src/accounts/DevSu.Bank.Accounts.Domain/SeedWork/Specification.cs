using System.Linq.Expressions;

namespace DevSu.Bank.Accounts.Domain.SeedWork
{
    public abstract class Specification<T>
    {
        public List<Expression<Func<T, object>>> Includes { get; } = [];

        public List<string> IncludeStrings { get; } = [];

        public List<OrderBy> Orders { get; } = [];

        public int PageSize { get; set; } = 20;

        public int PageNumber { get; set; } = 1;

        public void DisablePagination()
        {
            PageSize = -1;
        }

        public abstract Expression<Func<T, bool>> ToExpression();
    }
}
