using DevSu.Bank.Accounts.Application.Commands.CreateTransaction;
using DevSu.Bank.Accounts.Application.SeedWork;
using DevSu.Bank.Accounts.Domain.AggregateModels.AccountAggregate;
using DevSu.Bank.Accounts.Domain.SeedWork;
using DevSu.Bank.Accounts.Domain.ValueObjects;
using Moq;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DevSu.Bank.Accounts.Application.UnitTest.Commands.CreateTransaction
{
    public class CreateTransactionCommandHandlerFacts
    {
        private const string AccountNumber = "225487";

        private readonly Mock<IAccountRepository> _accountRepository = new();
        private readonly Mock<IUnitOfWork> _unitOfWork = new();

        public CreateTransactionCommandHandlerFacts()
        {
            _accountRepository.Setup(repository => repository.UnitOfWork).Returns(_unitOfWork.Object);
        }

        private CreateTransactionCommandHandler CreateHandler()
        {
            return new CreateTransactionCommandHandler(_accountRepository.Object);
        }

        private static CreateTransactionCommand CreateCommand(decimal amount = 600)
        {
            return new CreateTransactionCommand(AccountNumber, amount);
        }

        private void SetupExistingAccount(Account? account)
        {
            _accountRepository.Setup(repository => repository.GetSingleAsync(
                    It.IsAny<Expression<Func<Account, bool>>>(),
                    It.IsAny<Expression<Func<Account, Account>>>()))
                .ReturnsAsync(account);
        }

        private static Account CreateExistingAccount(decimal initialBalance = 100)
        {
            return new Account(AccountNumber, AccountType.Checking, initialBalance, 2);
        }

        [Fact]
        public async Task HandleShouldRegisterTheTransactionInTheAccount()
        {
            //Arrange
            var account = CreateExistingAccount();
            SetupExistingAccount(account);

            //Act
            await CreateHandler().Handle(CreateCommand(600), CancellationToken.None);

            //Assert
            Assert.Single(account.Transactions);
            Assert.Equal(TransactionType.Deposit, account.Transactions.Single().TransactionType);
            Assert.Equal(600, account.Transactions.Single().Amount);
        }

        [Fact]
        public async Task HandleShouldUpdateTheAvailableBalance()
        {
            //Arrange
            var account = CreateExistingAccount(100);
            SetupExistingAccount(account);

            //Act
            await CreateHandler().Handle(CreateCommand(600), CancellationToken.None);

            //Assert
            Assert.Equal(700, account.CurrentBalance);
        }

        [Fact]
        public async Task HandleShouldReturnTheIdentifierAssignedOnSave()
        {
            //Arrange
            var account = CreateExistingAccount();
            SetupExistingAccount(account);
            _unitOfWork.Setup(unitOfWork => unitOfWork.SaveEntitiesAsync(It.IsAny<CancellationToken>()))
                .Callback(() => typeof(Transaction).GetProperty(nameof(Transaction.Id))!
                    .GetSetMethod(true)!.Invoke(account.Transactions.Single(), [55L]))
                .ReturnsAsync(true);

            //Act
            var identifier = await CreateHandler().Handle(CreateCommand(), CancellationToken.None);

            //Assert
            Assert.Equal(55, identifier);
        }

        [Fact]
        public async Task HandleShouldPersistThroughTheUnitOfWork()
        {
            //Arrange
            var account = CreateExistingAccount();
            SetupExistingAccount(account);

            //Act
            await CreateHandler().Handle(CreateCommand(), CancellationToken.None);

            //Assert
            _accountRepository.Verify(repository => repository.Update(account), Times.Once);
            _unitOfWork.Verify(unitOfWork => unitOfWork.SaveEntitiesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task HandleShouldFailWhenTheAccountDoesNotExist()
        {
            //Arrange
            SetupExistingAccount(null);

            //Act - Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => CreateHandler().Handle(CreateCommand(), CancellationToken.None));
        }

        [Fact]
        public async Task HandleShouldNotPersistWhenTheAccountDoesNotExist()
        {
            //Arrange
            SetupExistingAccount(null);

            //Act
            await Assert.ThrowsAsync<NotFoundException>(
                () => CreateHandler().Handle(CreateCommand(), CancellationToken.None));

            //Assert
            _accountRepository.Verify(repository => repository.Update(It.IsAny<Account>()), Times.Never);
            _unitOfWork.Verify(unitOfWork => unitOfWork.SaveEntitiesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task HandleShouldPropagateTheInsufficientBalanceRule()
        {
            //Arrange
            var account = CreateExistingAccount(100);
            SetupExistingAccount(account);

            //Act
            var exception = await Assert.ThrowsAsync<InsufficientBalanceException>(
                () => CreateHandler().Handle(CreateCommand(-500), CancellationToken.None));

            //Assert
            Assert.Equal("Saldo no disponible", exception.Message);
            Assert.Equal(100, account.CurrentBalance);
            _unitOfWork.Verify(unitOfWork => unitOfWork.SaveEntitiesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
