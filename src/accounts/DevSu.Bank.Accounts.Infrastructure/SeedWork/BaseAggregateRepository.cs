using DevSu.Bank.Accounts.Domain.SeedWork;
using DevSu.Bank.Accounts.Infrastructure.AggregateDataContext;
using Microsoft.EntityFrameworkCore;

namespace DevSu.Bank.Accounts.Infrastructure.SeedWork
{
    public abstract class BaseAggregateRepository<TKey, TEntity>(AggregateContext mainContext)
        : BaseReadOnlyRepository<TKey, TEntity>(mainContext), IAggregateRepository<TKey, TEntity>
        where TEntity : class, IAggregateRoot
    {
        public IUnitOfWork UnitOfWork => (AggregateContext)MainContext;

        public virtual async Task<TEntity> CreateAsync(TEntity entity)
        {
            await DbSet.AddAsync(entity);
            return entity;
        }

        public virtual TEntity Create(TEntity entity)
        {
            DbSet.Add(entity);
            return entity;
        }

        public virtual void Remove(TEntity entity)
        {
            DbSet.Remove(entity);
        }

        public virtual TEntity Update(TEntity entity)
        {
            if (MainContext.Entry(entity).State == EntityState.Modified) return entity;
            MainContext.Entry(entity).State = EntityState.Modified;
            return entity;
        }
    }
}
