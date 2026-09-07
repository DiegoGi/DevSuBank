using DevSu.Bank.Accounts.Application.Commands.CreateAccount;
using DevSu.Bank.Accounts.Application.SeedWork;
using DevSu.Bank.Accounts.Domain.AggregateModels.AccountAggregate;
using DevSu.Bank.Accounts.Domain.AggregateModels.ClientAggregate;
using DevSu.Bank.Accounts.Domain.Resources;
using DevSu.Bank.Accounts.Domain.SeedWork;
using DevSu.Bank.Accounts.Domain.ValueObjects;
using Moq;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DevSu.Bank.Accounts.Application.UnitTest.Commands.CreateAccount
{
    public class CreateAccountCommandHandlerFacts
    {
        private const string AccountNumber = "225487";
        private const int ClientId = 2;

        private readonly Mock<IAccountRepository> _accountRepository = new();
        private readonly Mock<IClientRepository> _clientRepository = new();
        private readonly Mock<IUnitOfWork> _unitOfWork = new();

        public CreateAccountCommandHandlerFacts()
        {
            _accountRepository.Setup(repository => repository.UnitOfWork).Returns(_unitOfWork.Object);
            _accountRepository.Setup(repository => repository.ExistsAsync(It.IsAny<Expression<Func<Account, bool>>>()))
                .ReturnsAsync(false);
            _clientRepository.Setup(repository => repository.GetSingleByIdAsync(ClientId))
                .ReturnsAsync(new Client(ClientId, "Jane Smith", true));
        }

        private CreateAccountCommandHandler CreateHandler()
        {
            return new CreateAccountCommandHandler(_accountRepository.Object, _clientRepository.Object);
        }

        private static CreateAccountCommand CreateCommand()
        {
            return new CreateAccountCommand(AccountNumber, AccountType.Checking, 100, ClientId);
        }

        [Fact]
        public async Task HandleShouldReturnTheIdentifierOfTheCreatedAccount()
        {
            //Arrange
            _accountRepository.Setup(repository => repository.CreateAsync(It.IsAny<Account>()))
                .Callback<Account>(account =>
                    typeof(Account).GetProperty(nameof(Account.Id))!.GetSetMethod(true)!.Invoke(account, [77]))
                .ReturnsAsync((Account account) => account);

            //Act
            var identifier = await CreateHandler().Handle(CreateCommand(), CancellationToken.None);

            //Assert
            Assert.Equal(77, identifier);
        }

        [Fact]
        public async Task HandleShouldMapEveryCommandValueIntoTheAggregate()
        {
            //Arrange
            Account? created = null;
            _accountRepository.Setup(repository => repository.CreateAsync(It.IsAny<Account>()))
                .Callback<Account>(account => created = account)
                .ReturnsAsync((Account account) => account);

            //Act
            await CreateHandler().Handle(CreateCommand(), CancellationToken.None);

            //Assert
            Assert.Equal(AccountNumber, created!.AccountNumber);
            Assert.Equal(AccountType.Checking, created.AccountType);
            Assert.Equal(100, created.InitialBalance);
            Assert.Equal(100, created.CurrentBalance);
            Assert.Equal(ClientId, created.ClientId);
            Assert.True(created.Status);
        }

        [Fact]
        public async Task HandleShouldPersistThroughTheUnitOfWork()
        {
            //Arrange
            _accountRepository.Setup(repository => repository.CreateAsync(It.IsAny<Account>()))
                .ReturnsAsync((Account account) => account);

            //Act
            await CreateHandler().Handle(CreateCommand(), CancellationToken.None);

            //Assert
            _accountRepository.Verify(repository => repository.CreateAsync(It.IsAny<Account>()), Times.Once);
            _unitOfWork.Verify(unitOfWork => unitOfWork.SaveEntitiesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task HandleShouldFailWhenTheAccountNumberIsAlreadyRegistered()
        {
            //Arrange
            _accountRepository.Setup(repository => repository.ExistsAsync(It.IsAny<Expression<Func<Account, bool>>>()))
                .ReturnsAsync(true);

            //Act
            var exception = await Assert.ThrowsAsync<ApplicationValidationException>(
                () => CreateHandler().Handle(CreateCommand(), CancellationToken.None));

            //Assert
            Assert.Equal(Generals.AccountNumberAlreadyRegistered, exception.Message);
        }

        [Fact]
        public async Task HandleShouldFailWhenTheClientDoesNotExist()
        {
            //Arrange
            _clientRepository.Setup(repository => repository.GetSingleByIdAsync(ClientId))
                .ReturnsAsync((Client?)null);

            //Act - Assert
            await Assert.ThrowsAsync<ApplicationValidationException>(
                () => CreateHandler().Handle(CreateCommand(), CancellationToken.None));
        }

        [Fact]
        public async Task HandleShouldFailWhenTheClientIsInactive()
        {
            //Arrange
            _clientRepository.Setup(repository => repository.GetSingleByIdAsync(ClientId))
                .ReturnsAsync(new Client(ClientId, "Jane Smith", false));

            //Act - Assert
            await Assert.ThrowsAsync<ApplicationValidationException>(
                () => CreateHandler().Handle(CreateCommand(), CancellationToken.None));
        }

        [Fact]
        public async Task HandleShouldNotPersistWhenTheClientIsNotUsable()
        {
            //Arrange
            _clientRepository.Setup(repository => repository.GetSingleByIdAsync(ClientId))
                .ReturnsAsync((Client?)null);

            //Act
            await Assert.ThrowsAsync<ApplicationValidationException>(
                () => CreateHandler().Handle(CreateCommand(), CancellationToken.None));

            //Assert
            _accountRepository.Verify(repository => repository.CreateAsync(It.IsAny<Account>()), Times.Never);
            _unitOfWork.Verify(unitOfWork => unitOfWork.SaveEntitiesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
