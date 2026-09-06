using System.Linq.Expressions;

namespace DevSu.Bank.Customers.Domain.SeedWork
{
    public interface IReadOnlyRepository<in TKey, TEntity>
    {
        Task<TEntity?> GetSingleByIdAsync(TKey id);
        Task<TResult?> GetSingleAsync<TResult>(Expression<Func<TEntity, bool>> expression,
            Expression<Func<TEntity, TResult>> selector);
        Task<IEnumerable<TResult>> ListAsync<TResult>(Specification<TEntity> specification,
            Expression<Func<TEntity, TResult>> selector);
        Task<int> CountAsync(Specification<TEntity> specification);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> expression);
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> expression);
        Task<bool> ExistsAsync(Specification<TEntity> specification);
    }
}
