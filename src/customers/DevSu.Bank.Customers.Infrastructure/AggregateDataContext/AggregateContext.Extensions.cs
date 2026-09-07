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
            var saved = await base.SaveChangesAsync(cancellationToken) > 0;

            await _mediator.DispatchDomainEventsAsync(this);

            return saved;
        }
    }
}
