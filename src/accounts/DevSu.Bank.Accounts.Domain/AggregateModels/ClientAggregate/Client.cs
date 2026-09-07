using DevSu.Bank.Accounts.Domain.SeedWork;

namespace DevSu.Bank.Accounts.Domain.AggregateModels.ClientAggregate
{
    public class Client : Entity, IAggregateRoot
    {
        public int Id { get; private set; }

        public string Name { get; private set; } = null!;

        public bool Status { get; private set; }

        // Required for ORM
        protected Client()
        {
        }

        public Client(int id, string name, bool status)
        {
            Id = id;
            Name = EnsureNotEmpty(name, nameof(Name));
            Status = status;
        }

        public void Update(string name, bool status)
        {
            Name = EnsureNotEmpty(name, nameof(Name));
            Status = status;
        }
    }
}
