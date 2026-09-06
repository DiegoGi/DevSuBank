using DevSu.Bank.Customers.Domain.AggregateModels.ClientAggregate;
using DevSu.Bank.Customers.Domain.SeedWork;
using DevSu.Bank.Customers.Domain.ValueObjects;
using Xunit;

namespace DevSu.Bank.Customers.Domain.UnitTest.AggregateModels.ClientAggregate
{
    public class ClientFacts
    {
        private const string Name = "John Doe";
        private const string Identification = "0102030405";
        private const string Address = "742 Evergreen Terrace";
        private const string Phone = "5551234567";
        private const string ClientId = "CLI-001";
        private const string PasswordHash = "hashed-password";

        private static Client CreateClient(string name = Name, Gender gender = Gender.Male, int age = 41,
            string identification = Identification, string? address = Address, string? phone = Phone,
            string clientId = ClientId, string passwordHash = PasswordHash)
        {
            return new Client(name, gender, age, identification, address, phone, clientId, passwordHash);
        }

        [Fact]
        public void ConstructorShouldAssignEveryValue()
        {
            //Arrange - Act
            var client = CreateClient();

            //Assert
            Assert.Equal(Name, client.Name);
            Assert.Equal(Gender.Male, client.Gender);
            Assert.Equal(41, client.Age);
            Assert.Equal(Identification, client.Identification);
            Assert.Equal(Address, client.Address);
            Assert.Equal(Phone, client.Phone);
            Assert.Equal(ClientId, client.ClientId);
            Assert.Equal(PasswordHash, client.PasswordHash);
        }

        [Fact]
        public void ConstructorShouldCreateClientWithActiveStatus()
        {
            //Arrange - Act
            var client = CreateClient();

            //Assert
            Assert.True(client.Status);
        }

        [Fact]
        public void ConstructorShouldTrimTextValues()
        {
            //Arrange - Act
            var client = CreateClient(name: "  John Doe  ", identification: "  0102030405  ",
                address: "  742 Evergreen Terrace  ", phone: "  5551234567  ", clientId: "  CLI-001  ");

            //Assert
            Assert.Equal(Name, client.Name);
            Assert.Equal(Identification, client.Identification);
            Assert.Equal(Address, client.Address);
            Assert.Equal(Phone, client.Phone);
            Assert.Equal(ClientId, client.ClientId);
        }

        [Fact]
        public void ConstructorShouldAllowOptionalValuesAsNull()
        {
            //Arrange - Act
            var client = CreateClient(address: null, phone: null);

            //Assert
            Assert.Null(client.Address);
            Assert.Null(client.Phone);
        }

        [Fact]
        public void ClientShouldBeAnAggregateRootAndAPerson()
        {
            //Arrange - Act
            var client = CreateClient();

            //Assert
            Assert.IsAssignableFrom<IAggregateRoot>(client);
            Assert.IsAssignableFrom<Person>(client);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void ConstructorShouldFailWhenNameIsEmpty(string name)
        {
            //Arrange - Act
            var exception = Assert.Throws<DomainValidationException>(() => CreateClient(name: name));

            //Assert
            Assert.Contains(nameof(Client.Name), exception.Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void ConstructorShouldFailWhenIdentificationIsEmpty(string identification)
        {
            //Arrange - Act
            var exception = Assert.Throws<DomainValidationException>(
                () => CreateClient(identification: identification));

            //Assert
            Assert.Contains(nameof(Client.Identification), exception.Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void ConstructorShouldFailWhenClientIdIsEmpty(string clientId)
        {
            //Arrange - Act
            var exception = Assert.Throws<DomainValidationException>(() => CreateClient(clientId: clientId));

            //Assert
            Assert.Contains(nameof(Client.ClientId), exception.Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void ConstructorShouldFailWhenPasswordHashIsEmpty(string passwordHash)
        {
            //Arrange - Act
            var exception = Assert.Throws<DomainValidationException>(
                () => CreateClient(passwordHash: passwordHash));

            //Assert
            Assert.Contains(nameof(Client.PasswordHash), exception.Message);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(121)]
        public void ConstructorShouldFailWhenAgeIsOutOfRange(int age)
        {
            //Arrange - Act
            var exception = Assert.Throws<DomainValidationException>(() => CreateClient(age: age));

            //Assert
            Assert.Contains(nameof(Client.Age), exception.Message);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(120)]
        public void ConstructorShouldAcceptAgeBoundaries(int age)
        {
            //Arrange - Act
            var client = CreateClient(age: age);

            //Assert
            Assert.Equal(age, client.Age);
        }

        [Fact]
        public void UpdatePersonalInformationShouldReplacePersonalValues()
        {
            //Arrange
            var client = CreateClient();

            //Act
            client.UpdatePersonalInformation("Jane Smith", Gender.Female, 34, "221B Baker Street",
                "5559876543");

            //Assert
            Assert.Equal("Jane Smith", client.Name);
            Assert.Equal(Gender.Female, client.Gender);
            Assert.Equal(34, client.Age);
            Assert.Equal("221B Baker Street", client.Address);
            Assert.Equal("5559876543", client.Phone);
        }

        [Fact]
        public void UpdatePersonalInformationShouldKeepIdentityValues()
        {
            //Arrange
            var client = CreateClient();

            //Act
            client.UpdatePersonalInformation("Jane Smith", Gender.Female, 34, null, null);

            //Assert
            Assert.Equal(Identification, client.Identification);
            Assert.Equal(ClientId, client.ClientId);
            Assert.Equal(PasswordHash, client.PasswordHash);
        }

        [Fact]
        public void UpdatePersonalInformationShouldFailWhenNameIsEmpty()
        {
            //Arrange
            var client = CreateClient();

            //Act
            var exception = Assert.Throws<DomainValidationException>(
                () => client.UpdatePersonalInformation(string.Empty, Gender.Female, 34, null, null));

            //Assert
            Assert.Contains(nameof(Client.Name), exception.Message);
        }

        [Fact]
        public void ChangePasswordShouldReplaceThePasswordHash()
        {
            //Arrange
            var client = CreateClient();

            //Act
            client.ChangePassword("new-hashed-password");

            //Assert
            Assert.Equal("new-hashed-password", client.PasswordHash);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void ChangePasswordShouldFailWhenPasswordHashIsEmpty(string passwordHash)
        {
            //Arrange
            var client = CreateClient();

            //Act
            var exception = Assert.Throws<DomainValidationException>(() => client.ChangePassword(passwordHash));

            //Assert
            Assert.Contains(nameof(Client.PasswordHash), exception.Message);
            Assert.Equal(PasswordHash, client.PasswordHash);
        }

        [Fact]
        public void DeleteShouldTurnStatusInactive()
        {
            //Arrange
            var client = CreateClient();

            //Act
            client.Delete();

            //Assert
            Assert.False(client.Status);
        }

        [Fact]
        public void DeleteShouldKeepEveryOtherValue()
        {
            //Arrange
            var client = CreateClient();

            //Act
            client.Delete();

            //Assert
            Assert.Equal(Name, client.Name);
            Assert.Equal(Identification, client.Identification);
            Assert.Equal(ClientId, client.ClientId);
        }
    }
}
