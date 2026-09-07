namespace DevSu.Bank.Accounts.Domain.SeedWork
{
    public interface IAggregateRepository<in TKey, TEntity> : IReadOnlyRepository<TKey, TEntity> where TEntity : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
        Task<TEntity> CreateAsync(TEntity entity);
        TEntity Create(TEntity entity);
        void Remove(TEntity entity);
        TEntity Update(TEntity entity);
    }
}
