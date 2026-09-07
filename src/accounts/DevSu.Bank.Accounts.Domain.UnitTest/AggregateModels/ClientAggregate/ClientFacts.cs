using DevSu.Bank.Accounts.Domain.AggregateModels.ClientAggregate;
using DevSu.Bank.Accounts.Domain.SeedWork;
using Xunit;

namespace DevSu.Bank.Accounts.Domain.UnitTest.AggregateModels.ClientAggregate
{
    public class ClientFacts
    {
        private const int Id = 2;
        private const string Name = "Jane Smith";

        private static Client CreateClient(int id = Id, string name = Name, bool status = true)
        {
            return new Client(id, name, status);
        }

        [Fact]
        public void ConstructorShouldAssignEveryValue()
        {
            //Arrange - Act
            var client = CreateClient();

            //Assert
            Assert.Equal(Id, client.Id);
            Assert.Equal(Name, client.Name);
            Assert.True(client.Status);
        }

        [Fact]
        public void ConstructorShouldKeepTheIdentifierComingFromTheSource()
        {
            //Arrange - Act
            var client = CreateClient(id: 99);

            //Assert
            Assert.Equal(99, client.Id);
        }

        [Fact]
        public void ConstructorShouldTrimTheName()
        {
            //Arrange - Act
            var client = CreateClient(name: "  Jane Smith  ");

            //Assert
            Assert.Equal(Name, client.Name);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void ConstructorShouldFailWhenTheNameIsEmpty(string name)
        {
            //Arrange - Act
            var exception = Assert.Throws<DomainValidationException>(() => CreateClient(name: name));

            //Assert
            Assert.Contains(nameof(Client.Name), exception.Message);
        }

        [Fact]
        public void UpdateShouldReplaceNameAndStatus()
        {
            //Arrange
            var client = CreateClient();

            //Act
            client.Update("John Doe", false);

            //Assert
            Assert.Equal("John Doe", client.Name);
            Assert.False(client.Status);
        }

        [Fact]
        public void UpdateShouldKeepTheIdentifier()
        {
            //Arrange
            var client = CreateClient();

            //Act
            client.Update("John Doe", false);

            //Assert
            Assert.Equal(Id, client.Id);
        }

        [Fact]
        public void UpdateShouldFailWhenTheNameIsEmpty()
        {
            //Arrange
            var client = CreateClient();

            //Act
            var exception = Assert.Throws<DomainValidationException>(() => client.Update(string.Empty, true));

            //Assert
            Assert.Contains(nameof(Client.Name), exception.Message);
            Assert.Equal(Name, client.Name);
        }
    }
}
