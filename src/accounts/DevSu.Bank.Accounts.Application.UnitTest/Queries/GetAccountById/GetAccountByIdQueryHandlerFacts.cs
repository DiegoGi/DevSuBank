using DevSu.Bank.Accounts.Application.DTOs;
using DevSu.Bank.Accounts.Application.Queries.GetAccountById;
using DevSu.Bank.Accounts.Application.ReadOnlyModels;
using DevSu.Bank.Accounts.Application.ReadOnlyRepositories;
using DevSu.Bank.Accounts.Application.SeedWork;
using DevSu.Bank.Accounts.Domain.ValueObjects;
using Moq;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DevSu.Bank.Accounts.Application.UnitTest.Queries.GetAccountById
{
    public class GetAccountByIdQueryHandlerFacts
    {
        private const int AccountId = 2;

        private readonly Mock<IAccountReadOnlyRepository> _accountReadOnlyRepository = new();

        private GetAccountByIdQueryHandler CreateHandler()
        {
            return new GetAccountByIdQueryHandler(_accountReadOnlyRepository.Object);
        }

        private void SetupResult(AccountResponse? response)
        {
            _accountReadOnlyRepository.Setup(repository => repository.GetSingleAsync(
                    It.IsAny<Expression<Func<Account, bool>>>(),
                    It.IsAny<Expression<Func<Account, AccountResponse>>>()))
                .ReturnsAsync(response);
        }

        private static AccountResponse CreateResponse()
        {
            return new AccountResponse(AccountId, "225487", AccountType.Checking, 100, 700, true, 2, "Jane Smith");
        }

        [Fact]
        public async Task HandleShouldReturnTheAccount()
        {
            //Arrange
            SetupResult(CreateResponse());

            //Act
            var response = await CreateHandler().Handle(new GetAccountByIdQuery(AccountId), CancellationToken.None);

            //Assert
            Assert.Equal(AccountId, response.Id);
            Assert.Equal("225487", response.AccountNumber);
            Assert.Equal(AccountType.Checking, response.AccountType);
            Assert.Equal(100, response.InitialBalance);
            Assert.Equal(700, response.CurrentBalance);
            Assert.Equal("Jane Smith", response.ClientName);
        }

        [Fact]
        public async Task HandleShouldQueryTheReadOnlyRepositoryOnce()
        {
            //Arrange
            SetupResult(CreateResponse());

            //Act
            await CreateHandler().Handle(new GetAccountByIdQuery(AccountId), CancellationToken.None);

            //Assert
            _accountReadOnlyRepository.Verify(repository => repository.GetSingleAsync(
                It.IsAny<Expression<Func<Account, bool>>>(),
                It.IsAny<Expression<Func<Account, AccountResponse>>>()), Times.Once);
        }

        [Fact]
        public async Task HandleShouldFailWhenTheAccountDoesNotExist()
        {
            //Arrange
            SetupResult(null);

            //Act - Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => CreateHandler().Handle(new GetAccountByIdQuery(AccountId), CancellationToken.None));
        }
    }
}
