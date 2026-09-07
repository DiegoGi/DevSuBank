using DevSu.Bank.Accounts.Domain.Resources;
using MediatR;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevSu.Bank.Accounts.Domain.SeedWork
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

        protected static string EnsureNotEmpty(string value, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new DomainValidationException(string.Format(Generals.NotEmptyOrNullParameter, parameterName));
            }

            return value.Trim();
        }

        protected static decimal EnsureNotNegative(decimal value, string parameterName)
        {
            if (value < 0)
            {
                throw new DomainValidationException(string.Format(Generals.GreaterOrEqualTo, parameterName, 0));
            }

            return value;
        }
    }
}
