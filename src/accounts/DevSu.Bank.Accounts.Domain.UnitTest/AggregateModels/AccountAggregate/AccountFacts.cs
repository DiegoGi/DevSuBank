using DevSu.Bank.Accounts.Domain.AggregateModels.AccountAggregate;
using DevSu.Bank.Accounts.Domain.SeedWork;
using DevSu.Bank.Accounts.Domain.ValueObjects;
using System.Linq;
using Xunit;

namespace DevSu.Bank.Accounts.Domain.UnitTest.AggregateModels.AccountAggregate
{
    public class AccountFacts
    {
        private const string AccountNumber = "225487";
        private const int ClientId = 2;
        private const decimal InitialBalance = 100;

        private static Account CreateAccount(string accountNumber = AccountNumber,
            AccountType accountType = AccountType.Checking, decimal initialBalance = InitialBalance,
            int clientId = ClientId)
        {
            return new Account(accountNumber, accountType, initialBalance, clientId);
        }

        [Fact]
        public void ConstructorShouldAssignEveryValue()
        {
            //Arrange - Act
            var account = CreateAccount();

            //Assert
            Assert.Equal(AccountNumber, account.AccountNumber);
            Assert.Equal(AccountType.Checking, account.AccountType);
            Assert.Equal(InitialBalance, account.InitialBalance);
            Assert.Equal(ClientId, account.ClientId);
        }

        [Fact]
        public void ConstructorShouldStartWithTheInitialBalanceAsCurrentBalance()
        {
            //Arrange - Act
            var account = CreateAccount(initialBalance: 2000);

            //Assert
            Assert.Equal(2000, account.CurrentBalance);
        }

        [Fact]
        public void ConstructorShouldCreateAnActiveAccountWithoutTransactions()
        {
            //Arrange - Act
            var account = CreateAccount();

            //Assert
            Assert.True(account.Status);
            Assert.Empty(account.Transactions);
        }

        [Fact]
        public void ConstructorShouldTrimTheAccountNumber()
        {
            //Arrange - Act
            var account = CreateAccount(accountNumber: "  225487  ");

            //Assert
            Assert.Equal(AccountNumber, account.AccountNumber);
        }

        [Fact]
        public void AccountShouldBeAnAggregateRoot()
        {
            //Arrange - Act
            var account = CreateAccount();

            //Assert
            Assert.IsAssignableFrom<IAggregateRoot>(account);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void ConstructorShouldFailWhenTheAccountNumberIsEmpty(string accountNumber)
        {
            //Arrange - Act
            var exception = Assert.Throws<DomainValidationException>(
                () => CreateAccount(accountNumber: accountNumber));

            //Assert
            Assert.Contains(nameof(Account.AccountNumber), exception.Message);
        }

        [Fact]
        public void ConstructorShouldFailWhenTheInitialBalanceIsNegative()
        {
            //Arrange - Act
            var exception = Assert.Throws<DomainValidationException>(() => CreateAccount(initialBalance: -1));

            //Assert
            Assert.Contains(nameof(Account.InitialBalance), exception.Message);
        }

        [Fact]
        public void ConstructorShouldAcceptAZeroInitialBalance()
        {
            //Arrange - Act
            var account = CreateAccount(initialBalance: 0);

            //Assert
            Assert.Equal(0, account.CurrentBalance);
        }

        [Fact]
        public void RegisterTransactionShouldIncreaseTheBalanceOnADeposit()
        {
            //Arrange
            var account = CreateAccount(initialBalance: 100);

            //Act
            var transaction = account.RegisterTransaction(600);

            //Assert
            Assert.Equal(700, account.CurrentBalance);
            Assert.Equal(TransactionType.Deposit, transaction.TransactionType);
            Assert.Equal(600, transaction.Amount);
            Assert.Equal(700, transaction.Balance);
        }

        [Fact]
        public void RegisterTransactionShouldDecreaseTheBalanceOnAWithdrawal()
        {
            //Arrange
            var account = CreateAccount(initialBalance: 2000);

            //Act
            var transaction = account.RegisterTransaction(-575);

            //Assert
            Assert.Equal(1425, account.CurrentBalance);
            Assert.Equal(TransactionType.Withdrawal, transaction.TransactionType);
            Assert.Equal(-575, transaction.Amount);
            Assert.Equal(1425, transaction.Balance);
        }

        [Fact]
        public void RegisterTransactionShouldKeepTheInitialBalanceUnchanged()
        {
            //Arrange
            var account = CreateAccount(initialBalance: 100);

            //Act
            account.RegisterTransaction(600);

            //Assert
            Assert.Equal(100, account.InitialBalance);
        }

        [Fact]
        public void RegisterTransactionShouldAddTheTransactionToTheAggregate()
        {
            //Arrange
            var account = CreateAccount();

            //Act
            var transaction = account.RegisterTransaction(50);

            //Assert
            Assert.Single(account.Transactions);
            Assert.Same(transaction, account.Transactions.Single());
        }

        [Fact]
        public void RegisterTransactionShouldAccumulateEveryMovement()
        {
            //Arrange
            var account = CreateAccount(initialBalance: 1000);

            //Act
            account.RegisterTransaction(500);
            account.RegisterTransaction(-200);
            account.RegisterTransaction(-300);

            //Assert
            Assert.Equal(1000, account.CurrentBalance);
            Assert.Equal(3, account.Transactions.Count);
            Assert.Equal([1500, 1300, 1000], account.Transactions.Select(transaction => transaction.Balance));
        }

        [Fact]
        public void RegisterTransactionShouldAllowLeavingTheBalanceAtZero()
        {
            //Arrange
            var account = CreateAccount(initialBalance: 540);

            //Act
            account.RegisterTransaction(-540);

            //Assert
            Assert.Equal(0, account.CurrentBalance);
        }

        [Fact]
        public void RegisterTransactionShouldFailWhenThereIsNotEnoughBalance()
        {
            //Arrange
            var account = CreateAccount(initialBalance: 100);

            //Act
            var exception = Assert.Throws<InsufficientBalanceException>(
                () => account.RegisterTransaction(-101));

            //Assert
            Assert.Equal("Saldo no disponible", exception.Message);
        }

        [Fact]
        public void RegisterTransactionShouldNotChangeAnythingWhenThereIsNotEnoughBalance()
        {
            //Arrange
            var account = CreateAccount(initialBalance: 100);

            //Act
            Assert.Throws<InsufficientBalanceException>(() => account.RegisterTransaction(-500));

            //Assert
            Assert.Equal(100, account.CurrentBalance);
            Assert.Empty(account.Transactions);
        }

        [Fact]
        public void RegisterTransactionShouldFailWhenTheAmountIsZero()
        {
            //Arrange
            var account = CreateAccount();

            //Act
            var exception = Assert.Throws<DomainValidationException>(() => account.RegisterTransaction(0));

            //Assert
            Assert.Contains(nameof(Transaction.Amount), exception.Message);
            Assert.Empty(account.Transactions);
        }

        [Fact]
        public void RegisterTransactionShouldFailWhenTheAccountIsInactive()
        {
            //Arrange
            var account = CreateAccount();
            account.Deactivate();

            //Act
            var exception = Assert.Throws<DomainValidationException>(() => account.RegisterTransaction(100));

            //Assert
            Assert.Contains(nameof(Account.Status), exception.Message);
            Assert.Empty(account.Transactions);
        }

        [Fact]
        public void ChangeAccountTypeShouldReplaceTheType()
        {
            //Arrange
            var account = CreateAccount(accountType: AccountType.Savings);

            //Act
            account.ChangeAccountType(AccountType.Checking);

            //Assert
            Assert.Equal(AccountType.Checking, account.AccountType);
        }

        [Fact]
        public void ChangeAccountTypeShouldWorkOnAnInactiveAccount()
        {
            //Arrange
            var account = CreateAccount(accountType: AccountType.Savings);
            account.Deactivate();

            //Act
            account.ChangeAccountType(AccountType.Checking);

            //Assert
            Assert.Equal(AccountType.Checking, account.AccountType);
            Assert.False(account.Status);
        }

        [Fact]
        public void DeactivateShouldTurnStatusInactive()
        {
            //Arrange
            var account = CreateAccount();

            //Act
            account.Deactivate();

            //Assert
            Assert.False(account.Status);
        }

        [Fact]
        public void ActivateShouldTurnStatusActive()
        {
            //Arrange
            var account = CreateAccount();
            account.Deactivate();

            //Act
            account.Activate();

            //Assert
            Assert.True(account.Status);
        }

        [Fact]
        public void DeactivateShouldKeepTheBalanceAndTheTransactions()
        {
            //Arrange
            var account = CreateAccount(initialBalance: 100);
            account.RegisterTransaction(600);

            //Act
            account.Deactivate();

            //Assert
            Assert.Equal(700, account.CurrentBalance);
            Assert.Single(account.Transactions);
        }
    }
}
