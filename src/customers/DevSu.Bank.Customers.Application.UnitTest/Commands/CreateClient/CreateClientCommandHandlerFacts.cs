using DevSu.Bank.Customers.Application.Commands.CreateClient;
using DevSu.Bank.Customers.Application.SeedWork;
using DevSu.Bank.Customers.Application.Services.Infrastructure;
using DevSu.Bank.Customers.Domain.AggregateModels.ClientAggregate;
using DevSu.Bank.Customers.Domain.SeedWork;
using DevSu.Bank.Customers.Domain.ValueObjects;
using Moq;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DevSu.Bank.Customers.Application.UnitTest.Commands.CreateClient
{
    public class CreateClientCommandHandlerFacts
    {
        private const string PlainPassword = "plain-password";
        private const string HashedPassword = "hashed-password";

        private readonly Mock<IClientRepository> _clientRepository = new();
        private readonly Mock<IPasswordHasherService> _passwordHasherService = new();
        private readonly Mock<IUnitOfWork> _unitOfWork = new();

        public CreateClientCommandHandlerFacts()
        {
            _clientRepository.Setup(repository => repository.UnitOfWork).Returns(_unitOfWork.Object);
            _passwordHasherService.Setup(hasher => hasher.Hash(It.IsAny<string>())).Returns(HashedPassword);
        }

        private CreateClientCommandHandler CreateHandler()
        {
            return new CreateClientCommandHandler(_clientRepository.Object, _passwordHasherService.Object);
        }

        private static CreateClientCommand CreateCommand()
        {
            return new CreateClientCommand("John Doe", Gender.Male, 41, "0102030405",
                "742 Evergreen Terrace", "5551234567", "CLI-001", PlainPassword);
        }

        private void SetupExistingClients(params bool[] results)
        {
            var sequence = _clientRepository.SetupSequence(
                repository => repository.ExistsAsync(It.IsAny<Expression<Func<Client, bool>>>()));

            foreach (var result in results)
            {
                sequence = sequence.ReturnsAsync(result);
            }
        }

        private static void SimulateGeneratedIdentifier(Client client, int identifier)
        {
            typeof(Client).GetProperty(nameof(Client.Id))!.GetSetMethod(true)!.Invoke(client, [identifier]);
        }

        [Fact]
        public async Task HandleShouldReturnTheIdentifierOfTheCreatedClient()
        {
            //Arrange
            SetupExistingClients(false, false);
            _clientRepository.Setup(repository => repository.CreateAsync(It.IsAny<Client>()))
                .Callback<Client>(client => SimulateGeneratedIdentifier(client, 99))
                .ReturnsAsync((Client client) => client);

            //Act
            var identifier = await CreateHandler().Handle(CreateCommand(), CancellationToken.None);

            //Assert
            Assert.Equal(99, identifier);
        }

        [Fact]
        public async Task HandleShouldHashThePasswordBeforeStoringIt()
        {
            //Arrange
            SetupExistingClients(false, false);
            Client? createdClient = null;
            _clientRepository.Setup(repository => repository.CreateAsync(It.IsAny<Client>()))
                .Callback<Client>(client => createdClient = client)
                .ReturnsAsync((Client client) => client);

            //Act
            await CreateHandler().Handle(CreateCommand(), CancellationToken.None);

            //Assert
            _passwordHasherService.Verify(hasher => hasher.Hash(PlainPassword), Times.Once);
            Assert.Equal(HashedPassword, createdClient!.PasswordHash);
            Assert.DoesNotContain(PlainPassword, createdClient.PasswordHash);
        }

        [Fact]
        public async Task HandleShouldMapEveryCommandValueIntoTheAggregate()
        {
            //Arrange
            SetupExistingClients(false, false);
            Client? createdClient = null;
            _clientRepository.Setup(repository => repository.CreateAsync(It.IsAny<Client>()))
                .Callback<Client>(client => createdClient = client)
                .ReturnsAsync((Client client) => client);

            //Act
            await CreateHandler().Handle(CreateCommand(), CancellationToken.None);

            //Assert
            Assert.Equal("John Doe", createdClient!.Name);
            Assert.Equal(Gender.Male, createdClient.Gender);
            Assert.Equal(41, createdClient.Age);
            Assert.Equal("0102030405", createdClient.Identification);
            Assert.Equal("742 Evergreen Terrace", createdClient.Address);
            Assert.Equal("5551234567", createdClient.Phone);
            Assert.Equal("CLI-001", createdClient.ClientId);
        }

        [Fact]
        public async Task HandleShouldCreateTheClientWithActiveStatus()
        {
            //Arrange
            SetupExistingClients(false, false);
            Client? createdClient = null;
            _clientRepository.Setup(repository => repository.CreateAsync(It.IsAny<Client>()))
                .Callback<Client>(client => createdClient = client)
                .ReturnsAsync((Client client) => client);

            //Act
            await CreateHandler().Handle(CreateCommand(), CancellationToken.None);

            //Assert
            Assert.True(createdClient!.Status);
        }

        [Fact]
        public async Task HandleShouldPersistThroughTheUnitOfWork()
        {
            //Arrange
            SetupExistingClients(false, false);
            _clientRepository.Setup(repository => repository.CreateAsync(It.IsAny<Client>()))
                .ReturnsAsync((Client client) => client);

            //Act
            await CreateHandler().Handle(CreateCommand(), CancellationToken.None);

            //Assert
            _clientRepository.Verify(repository => repository.CreateAsync(It.IsAny<Client>()), Times.Once);
            _unitOfWork.Verify(unitOfWork => unitOfWork.SaveEntitiesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task HandleShouldFailWhenTheClientIdIsAlreadyRegistered()
        {
            //Arrange
            SetupExistingClients(true);

            //Act
            var exception = await Assert.ThrowsAsync<ApplicationValidationException>(
                () => CreateHandler().Handle(CreateCommand(), CancellationToken.None));

            //Assert
            Assert.Contains(nameof(CreateClientCommand.ClientId), exception.Message);
        }

        [Fact]
        public async Task HandleShouldFailWhenTheIdentificationIsAlreadyRegistered()
        {
            //Arrange
            SetupExistingClients(false, true);

            //Act
            var exception = await Assert.ThrowsAsync<ApplicationValidationException>(
                () => CreateHandler().Handle(CreateCommand(), CancellationToken.None));

            //Assert
            Assert.Contains(nameof(CreateClientCommand.Identification), exception.Message);
        }

        [Fact]
        public async Task HandleShouldNotPersistWhenTheClientIsAlreadyRegistered()
        {
            //Arrange
            SetupExistingClients(true);

            //Act
            await Assert.ThrowsAsync<ApplicationValidationException>(
                () => CreateHandler().Handle(CreateCommand(), CancellationToken.None));

            //Assert
            _clientRepository.Verify(repository => repository.CreateAsync(It.IsAny<Client>()), Times.Never);
            _unitOfWork.Verify(unitOfWork => unitOfWork.SaveEntitiesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task HandleShouldNotHashThePasswordWhenTheClientIsAlreadyRegistered()
        {
            //Arrange
            SetupExistingClients(true);

            //Act
            await Assert.ThrowsAsync<ApplicationValidationException>(
                () => CreateHandler().Handle(CreateCommand(), CancellationToken.None));

            //Assert
            _passwordHasherService.Verify(hasher => hasher.Hash(It.IsAny<string>()), Times.Never);
        }
    }
}
