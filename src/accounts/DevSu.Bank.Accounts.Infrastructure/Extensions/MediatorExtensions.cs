using DevSu.Bank.Accounts.Domain.SeedWork;
using DevSu.Bank.Accounts.Infrastructure.AggregateDataContext;
using MediatR;

namespace DevSu.Bank.Accounts.Infrastructure.Extensions
{
    internal static class MediatorExtensions
    {
        public static async Task DispatchDomainEventsAsync(this IMediator mediator, AggregateContext context)
        {
            var domainEntities = context.ChangeTracker
                .Entries<Entity>()
                .Where(entity => entity.Entity.DomainEvents.Any())
                .ToList();

            var domainEvents = domainEntities
                .SelectMany(entity => entity.Entity.DomainEvents)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.ClearDomainEvents());

            foreach (var domainEvent in domainEvents)
                await mediator.Publish(domainEvent);
        }
    }
}
