using DevSu.Bank.Customers.Application.Commands.UpdateClient;
using DevSu.Bank.Customers.Application.SeedWork;
using DevSu.Bank.Customers.Domain.AggregateModels.ClientAggregate;
using DevSu.Bank.Customers.Domain.SeedWork;
using DevSu.Bank.Customers.Domain.ValueObjects;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DevSu.Bank.Customers.Application.UnitTest.Commands.UpdateClient
{
    public class UpdateClientCommandHandlerFacts
    {
        private const int ClientIdentifier = 1;
        private const string Identification = "0102030405";
        private const string ClientId = "CLI-001";
        private const string PasswordHash = "hashed-password";

        private readonly Mock<IClientRepository> _clientRepository = new();
        private readonly Mock<IUnitOfWork> _unitOfWork = new();

        public UpdateClientCommandHandlerFacts()
        {
            _clientRepository.Setup(repository => repository.UnitOfWork).Returns(_unitOfWork.Object);
        }

        private UpdateClientCommandHandler CreateHandler()
        {
            return new UpdateClientCommandHandler(_clientRepository.Object);
        }

        private static Client CreateExistingClient()
        {
            return new Client("John Doe", Gender.Male, 41, Identification, "742 Evergreen Terrace",
                "5551234567", ClientId, PasswordHash);
        }

        private static UpdateClientCommand CreateCommand()
        {
            return new UpdateClientCommand(ClientIdentifier, "Jane Smith", Gender.Female, 34,
                "221B Baker Street", "5559876543");
        }

        private void SetupExistingClient(Client? client)
        {
            _clientRepository.Setup(repository => repository.GetSingleByIdAsync(ClientIdentifier))
                .ReturnsAsync(client);
        }

        [Fact]
        public async Task HandleShouldReplaceThePersonalValues()
        {
            //Arrange
            var client = CreateExistingClient();
            SetupExistingClient(client);

            //Act
            await CreateHandler().Handle(CreateCommand(), CancellationToken.None);

            //Assert
            Assert.Equal("Jane Smith", client.Name);
            Assert.Equal(Gender.Female, client.Gender);
            Assert.Equal(34, client.Age);
            Assert.Equal("221B Baker Street", client.Address);
            Assert.Equal("5559876543", client.Phone);
        }

        [Fact]
        public async Task HandleShouldKeepTheIdentityValues()
        {
            //Arrange
            var client = CreateExistingClient();
            SetupExistingClient(client);

            //Act
            await CreateHandler().Handle(CreateCommand(), CancellationToken.None);

            //Assert
            Assert.Equal(Identification, client.Identification);
            Assert.Equal(ClientId, client.ClientId);
            Assert.Equal(PasswordHash, client.PasswordHash);
        }

        [Fact]
        public async Task HandleShouldNotModifyTheStatus()
        {
            //Arrange
            var client = CreateExistingClient();
            SetupExistingClient(client);

            //Act
            await CreateHandler().Handle(CreateCommand(), CancellationToken.None);

            //Assert
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
        }
    }
}
