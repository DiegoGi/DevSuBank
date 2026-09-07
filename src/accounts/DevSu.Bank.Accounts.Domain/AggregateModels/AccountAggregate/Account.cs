using DevSu.Bank.Accounts.Domain.Resources;
using DevSu.Bank.Accounts.Domain.SeedWork;
using DevSu.Bank.Accounts.Domain.ValueObjects;

namespace DevSu.Bank.Accounts.Domain.AggregateModels.AccountAggregate
{
    public class Account : Entity, IAggregateRoot
    {
        private readonly List<Transaction> _transactions = [];

        public int Id { get; private set; }

        public string AccountNumber { get; private set; } = null!;

        public AccountType AccountType { get; private set; }

        public decimal InitialBalance { get; private set; }

        public decimal CurrentBalance { get; private set; }

        public bool Status { get; private set; }

        public int ClientId { get; private set; }

        public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();

        // Required for ORM
        protected Account()
        {
        }

        public Account(string accountNumber, AccountType accountType, decimal initialBalance, int clientId)
        {
            AccountNumber = EnsureNotEmpty(accountNumber, nameof(AccountNumber));
            AccountType = accountType;
            InitialBalance = EnsureNotNegative(initialBalance, nameof(InitialBalance));
            CurrentBalance = InitialBalance;
            ClientId = clientId;
            Status = true;
        }

        public Transaction RegisterTransaction(decimal amount)
        {
            EnsureIsActive();

            if (amount == 0)
            {
                throw new DomainValidationException(
                    string.Format(Generals.InvalidValueForParameter, nameof(Transaction.Amount)));
            }

            var balance = CurrentBalance + amount;

            if (balance < 0)
            {
                throw new InsufficientBalanceException();
            }

            var transactionType = amount > 0 ? TransactionType.Deposit : TransactionType.Withdrawal;
            var transaction = new Transaction(transactionType, amount, balance);

            _transactions.Add(transaction);
            CurrentBalance = balance;

            return transaction;
        }

        public void ChangeAccountType(AccountType accountType)
        {
            AccountType = accountType;
        }

        public void Activate()
        {
            Status = true;
        }

        public void Deactivate()
        {
            Status = false;
        }

        private void EnsureIsActive()
        {
            if (!Status)
            {
                throw new DomainValidationException(
                    string.Format(Generals.InvalidValueForParameter, nameof(Status)));
            }
        }
    }
}
