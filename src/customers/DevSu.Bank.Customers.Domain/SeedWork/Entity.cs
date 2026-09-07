using MediatR;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevSu.Bank.Customers.Domain.SeedWork
{
    public abstract class Entity
    {
        private List<INotification> _domainEvents = [];

        [NotMapped]
        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

        public void AddDomainEvent(INotification eventItem)
        {
            _domainEvents ??= [];

            _domainEvents.Add(eventItem);
        }

        public void RemoveDomainEvent(INotification eventItem)
        {
            _domainEvents.Remove(eventItem);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
