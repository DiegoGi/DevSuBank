using DevSu.Bank.Customers.Domain.SeedWork;
using DevSu.Bank.Customers.Domain.ValueObjects;

namespace DevSu.Bank.Customers.Domain.AggregateModels.ClientAggregate
{
    public class Client : Person, IAggregateRoot
    {
        public string ClientId { get; private set; } = null!;

        public string PasswordHash { get; private set; } = null!;

        public bool Status { get; private set; }

        // Required for ORM
        protected Client()
        {
        }

        public Client(string name, Gender gender, int age, string identification, string? address,
            string? phone, string clientId, string passwordHash)
            : base(name, gender, age, identification, address, phone)
        {
            ClientId = EnsureNotEmpty(clientId, nameof(ClientId));
            PasswordHash = EnsureNotEmpty(passwordHash, nameof(PasswordHash));
            Status = true;
        }

        public void ChangePassword(string passwordHash)
        {
            PasswordHash = EnsureNotEmpty(passwordHash, nameof(PasswordHash));
        }

        public void Delete()
        {
            Status = false;
        }
    }
}
