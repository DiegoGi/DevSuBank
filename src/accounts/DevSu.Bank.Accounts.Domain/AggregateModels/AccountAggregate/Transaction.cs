using DevSu.Bank.Accounts.Domain.SeedWork;
using DevSu.Bank.Accounts.Domain.ValueObjects;

namespace DevSu.Bank.Accounts.Domain.AggregateModels.AccountAggregate
{
    public class Transaction : Entity
    {
        public long Id { get; private set; }

        public DateTime TransactionDate { get; private set; }

        public TransactionType TransactionType { get; private set; }

        public decimal Amount { get; private set; }

        public decimal Balance { get; private set; }

        public int AccountId { get; private set; }

        // Required for ORM
        protected Transaction()
        {
        }

        internal Transaction(TransactionType transactionType, decimal amount, decimal balance)
        {
            TransactionDate = DateTime.UtcNow;
            TransactionType = transactionType;
            Amount = amount;
            Balance = balance;
        }
    }
}
