using DevSu.Bank.Customers.Domain.SeedWork;
using DevSu.Bank.Customers.Infrastructure.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DevSu.Bank.Customers.Infrastructure.AggregateDataContext
{
    public partial class AggregateContext : IUnitOfWork
    {
        private readonly IMediator _mediator;

        public AggregateContext(DbContextOptions<AggregateContext> options, IMediator mediator) : base(options)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new Configurations.ClientConfiguration());

            OnModelCreatingPartial(modelBuilder);
        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            // Dispatch Domain Events collection. 
            // Choices:
            // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
            // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
            // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions. 
            // You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers. 
            await _mediator.DispatchDomainEventsAsync(this);

            // After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
            // performed through the DbContext will be committed
            return await base.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
