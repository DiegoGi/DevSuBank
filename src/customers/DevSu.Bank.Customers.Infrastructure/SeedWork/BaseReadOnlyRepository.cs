using DevSu.Bank.Customers.Domain.SeedWork;
using DevSu.Bank.Customers.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DevSu.Bank.Customers.Infrastructure.SeedWork;

public abstract class BaseReadOnlyRepository<TKey, TEntity>(DbContext mainContext)
    : IReadOnlyRepository<TKey, TEntity>
    where TEntity : class
{
    protected readonly DbContext MainContext = mainContext;
    protected DbSet<TEntity> DbSet => MainContext.Set<TEntity>();

    public virtual async Task<TEntity?> GetSingleByIdAsync(TKey id)
    {
        return await DbSet.FindAsync(id);
    }

    public virtual async Task<TResult?> GetSingleAsync<TResult>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TResult>> selector)
    {
        return await DbSet.AsQueryable().Where(expression).Select(selector).FirstOrDefaultAsync();
    }

    public virtual async Task<IEnumerable<TResult>> ListAsync<TResult>(Specification<TEntity> specification, Expression<Func<TEntity, TResult>> selector)
    {
        var queryable = GetPaginatedIQueryable(specification);

        return await queryable.Select(selector).ToListAsync();
    }

    public virtual async Task<int> CountAsync(Specification<TEntity> specification)
    {
        return await GetIQueryable(specification).CountAsync();
    }

    public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> expression)
    {
        return await DbSet.AsQueryable().CountAsync(expression);
    }

    public virtual async Task<bool> ExistsAsync(Specification<TEntity> specification)
    {
        return await DbSet.AsQueryable().AnyAsync(specification.ToExpression());
    }

    public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> expression)
    {
        return await DbSet.AsQueryable().AnyAsync(expression);
    }

    protected virtual IQueryable<TEntity> GetIQueryable(Specification<TEntity> specification)
    {
        var queryableResultWithIncludes = specification.Includes
            .Aggregate(DbSet.AsQueryable(),
                (current, include) => current.Include(include));

        var secondaryResult = specification.IncludeStrings
            .Aggregate(queryableResultWithIncludes,
                (current, include) => current.Include(include));

        return secondaryResult
            .Where(specification.ToExpression());
    }

    protected virtual IQueryable<TEntity> GetPaginatedIQueryable(Specification<TEntity> specification)
    {
        var queryable = GetIQueryable(specification)
            .OrderBy(specification.Orders);

        if (specification.PageSize >= 0)
            queryable = queryable.Skip((specification.PageNumber - 1) * specification.PageSize)
                .Take(specification.PageSize);

        return queryable;
    }
}
