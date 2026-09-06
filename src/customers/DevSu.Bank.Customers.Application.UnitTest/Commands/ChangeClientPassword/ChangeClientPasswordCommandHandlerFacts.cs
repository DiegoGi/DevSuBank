using DevSu.Bank.Customers.Application.Commands.ChangeClientPassword;
using DevSu.Bank.Customers.Application.SeedWork;
using DevSu.Bank.Customers.Application.Services.Infrastructure;
using DevSu.Bank.Customers.Domain.AggregateModels.ClientAggregate;
using DevSu.Bank.Customers.Domain.SeedWork;
using DevSu.Bank.Customers.Domain.ValueObjects;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DevSu.Bank.Customers.Application.UnitTest.Commands.ChangeClientPassword
{
    public class ChangeClientPasswordCommandHandlerFacts
    {
        private const int ClientIdentifier = 1;
        private const string CurrentPasswordHash = "current-hashed-password";
        private const string NewPlainPassword = "new-plain-password";
        private const string NewPasswordHash = "new-hashed-password";

        private readonly Mock<IClientRepository> _clientRepository = new();
        private readonly Mock<IPasswordHasherService> _passwordHasherService = new();
        private readonly Mock<IUnitOfWork> _unitOfWork = new();

        public ChangeClientPasswordCommandHandlerFacts()
        {
            _clientRepository.Setup(repository => repository.UnitOfWork).Returns(_unitOfWork.Object);
            _passwordHasherService.Setup(hasher => hasher.Hash(It.IsAny<string>())).Returns(NewPasswordHash);
        }

        private ChangeClientPasswordCommandHandler CreateHandler()
        {
            return new ChangeClientPasswordCommandHandler(_clientRepository.Object, _passwordHasherService.Object);
        }

        private static Client CreateExistingClient()
        {
            return new Client("John Doe", Gender.Male, 41, "0102030405", "742 Evergreen Terrace",
                "5551234567", "CLI-001", CurrentPasswordHash);
        }

        private static ChangeClientPasswordCommand CreateCommand()
        {
            return new ChangeClientPasswordCommand(ClientIdentifier, NewPlainPassword);
        }

        private void SetupExistingClient(Client? client)
        {
            _clientRepository.Setup(repository => repository.GetSingleByIdAsync(ClientIdentifier))
                .ReturnsAsync(client);
        }

        [Fact]
        public async Task HandleShouldReplaceThePasswordHash()
        {
            //Arrange
            var client = CreateExistingClient();
            SetupExistingClient(client);

            //Act
            await CreateHandler().Handle(CreateCommand(), CancellationToken.None);

            //Assert
            Assert.Equal(NewPasswordHash, client.PasswordHash);
            Assert.NotEqual(CurrentPasswordHash, client.PasswordHash);
        }

        [Fact]
        public async Task HandleShouldHashThePlainPasswordBeforeStoringIt()
        {
            //Arrange
            var client = CreateExistingClient();
            SetupExistingClient(client);

            //Act
            await CreateHandler().Handle(CreateCommand(), CancellationToken.None);

            //Assert
            _passwordHasherService.Verify(hasher => hasher.Hash(NewPlainPassword), Times.Once);
            Assert.DoesNotContain(NewPlainPassword, client.PasswordHash);
        }

        [Fact]
        public async Task HandleShouldKeepEveryOtherValue()
        {
            //Arrange
            var client = CreateExistingClient();
            SetupExistingClient(client);

            //Act
            await CreateHandler().Handle(CreateCommand(), CancellationToken.None);

            //Assert
            Assert.Equal("John Doe", client.Name);
            Assert.Equal("0102030405", client.Identification);
            Assert.Equal("CLI-001", client.ClientId);
            Assert.True(client.Status);
        }

        [Fact]
        public async Task HandleShouldPersistThroughTheUnitOfWork()
        {
            //Arrange
            var client = CreateExistingClient();
            SetupExistingClient(client);

            //Act
            await CreateHandler().Handle(CreateCommand(), CancellationToken.None);

            //Assert
            _clientRepository.Verify(repository => repository.Update(client), Times.Once);
            _unitOfWork.Verify(unitOfWork => unitOfWork.SaveEntitiesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task HandleShouldFailWhenTheClientDoesNotExist()
        {
            //Arrange
            SetupExistingClient(null);

            //Act - Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => CreateHandler().Handle(CreateCommand(), CancellationToken.None));
        }

        [Fact]
        public async Task HandleShouldNotPersistWhenTheClientDoesNotExist()
        {
            //Arrange
            SetupExistingClient(null);

            //Act
            await Assert.ThrowsAsync<NotFoundException>(
                () => CreateHandler().Handle(CreateCommand(), CancellationToken.None));

            //Assert
            _clientRepository.Verify(repository => repository.Update(It.IsAny<Client>()), Times.Never);
            _unitOfWork.Verify(unitOfWork => unitOfWork.SaveEntitiesAsync(It.IsAny<CancellationToken>()), Times.Never);
            _passwordHasherService.Verify(hasher => hasher.Hash(It.IsAny<string>()), Times.Never);
        }
    }
}
