using DevSu.Bank.Customers.Domain.SeedWork;
using System.Linq.Expressions;
using System.Reflection;

namespace DevSu.Bank.Customers.Infrastructure.Extensions
{
    internal static class PredicateBuilderExtensions
    {
        /// <summary>
        ///     Sort the elements of a sequence according to orders list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="orders">List with orders definitions</param>
        /// <returns></returns>
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> query, IEnumerable<OrderBy> orders)
        {
            var firstIteration = true;

            foreach (var order in orders)
            {
                query = order.Type switch
                {
                    SortOrder.Descending => firstIteration
                        ? query.OrderByDescending(order.SortField)
                        : query.ThenByDescending(order.SortField),
                    _ => firstIteration ? query.OrderBy(order.SortField) : query.ThenBy(order.SortField)
                };

                firstIteration = false;
            }

            return query;
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> items, string propertyName)
        {
            return BaseOrder(items, propertyName, "OrderBy");
        }

        private static IQueryable<T> BaseOrder<T>(IQueryable<T> items, string propertyName, string typeArgument)
        {
            var typeOfT = typeof(T);
            var parameter = Expression.Parameter(typeOfT, "parameter");
            var propertyType = typeOfT.GetNestedProperty(propertyName)?.PropertyType;
            var propertyAccess = propertyName.Split('.')
                .Aggregate<string, Expression>(parameter, Expression.PropertyOrField);
            var orderExpression = Expression.Lambda(propertyAccess, parameter);

            var expression = Expression.Call(typeof(Queryable), typeArgument, [typeOfT, propertyType!],
                items.Expression, Expression.Quote(orderExpression));
            return items.Provider.CreateQuery<T>(expression);
        }

        public static IQueryable<T> OrderByDescending<T>(this IQueryable<T> items, string propertyName)
        {
            return BaseOrder(items, propertyName, "OrderByDescending");
        }

        public static IQueryable<T> ThenBy<T>(this IQueryable<T> items, string propertyName)
        {
            return BaseOrder(items, propertyName, "ThenBy");
        }

        public static IQueryable<T> ThenByDescending<T>(this IQueryable<T> items, string propertyName)
        {
            return BaseOrder(items, propertyName, "ThenByDescending");
        }

        private static PropertyInfo GetNestedProperty(this Type type, string propertyName)
        {
            var properties = propertyName.Split(['.'], 2);
            var propertyInfo = type.GetProperty(properties[0]);
            if (propertyInfo == null)
                throw new ArgumentException($"{properties[0]} is not a property of {type.FullName}.");
            // ReSharper disable once TailRecursiveCall
            return properties.Length == 1 ? propertyInfo : propertyInfo.PropertyType.GetNestedProperty(properties[1]);
        }
    }
}
