using DevSu.Bank.Customers.Infrastructure.Services;
using System;
using System.Linq;
using Xunit;

namespace DevSu.Bank.Customers.Infrastructure.UnitTest.Services
{
    public class PasswordHasherServiceFacts
    {
        private const string Password = "plain-password";
        private const char Separator = '.';
        private const int SaltSizeInBytes = 16;
        private const int KeySizeInBytes = 32;
        private const int DatabaseColumnLength = 200;

        private readonly PasswordHasherService _passwordHasherService = new();

        [Fact]
        public void HashShouldNotReturnThePlainPassword()
        {
            //Arrange - Act
            var hash = _passwordHasherService.Hash(Password);

            //Assert
            Assert.NotEqual(Password, hash);
            Assert.DoesNotContain(Password, hash);
        }

        [Fact]
        public void HashShouldReturnSaltAndKeySeparatedOnce()
        {
            //Arrange - Act
            var hash = _passwordHasherService.Hash(Password);

            //Assert
            Assert.Equal(1, hash.Count(character => character == Separator));
        }

        [Fact]
        public void HashShouldReturnBothPartsAsBase64()
        {
            //Arrange - Act
            var parts = _passwordHasherService.Hash(Password).Split(Separator);

            //Assert
            Assert.Equal(SaltSizeInBytes, Convert.FromBase64String(parts[0]).Length);
            Assert.Equal(KeySizeInBytes, Convert.FromBase64String(parts[1]).Length);
        }

        [Fact]
        public void HashShouldReturnADifferentValueForTheSamePassword()
        {
            //Arrange - Act
            var firstHash = _passwordHasherService.Hash(Password);
            var secondHash = _passwordHasherService.Hash(Password);

            //Assert
            Assert.NotEqual(firstHash, secondHash);
        }

        [Fact]
        public void HashShouldGenerateADifferentSaltOnEveryCall()
        {
            //Arrange - Act
            var firstSalt = _passwordHasherService.Hash(Password).Split(Separator)[0];
            var secondSalt = _passwordHasherService.Hash(Password).Split(Separator)[0];

            //Assert
            Assert.NotEqual(firstSalt, secondSalt);
        }

        [Fact]
        public void HashShouldReturnADifferentValueForADifferentPassword()
        {
            //Arrange - Act
            var firstHash = _passwordHasherService.Hash(Password);
            var secondHash = _passwordHasherService.Hash("another-plain-password");

            //Assert
            Assert.NotEqual(firstHash, secondHash);
        }

        [Fact]
        public void HashShouldFitTheDatabaseColumn()
        {
            //Arrange - Act
            var hash = _passwordHasherService.Hash(Password);

            //Assert
            Assert.True(hash.Length <= DatabaseColumnLength);
        }

        [Theory]
        [InlineData("1234")]
        [InlineData("a")]
        [InlineData("password with spaces and accents áéí")]
        [InlineData("0123456789012345678901234567890123456789012345678901234567890123456789")]
        public void HashShouldSupportAnyPasswordLengthAndCharacters(string password)
        {
            //Arrange - Act
            var parts = _passwordHasherService.Hash(password).Split(Separator);

            //Assert
            Assert.Equal(2, parts.Length);
            Assert.Equal(SaltSizeInBytes, Convert.FromBase64String(parts[0]).Length);
            Assert.Equal(KeySizeInBytes, Convert.FromBase64String(parts[1]).Length);
        }
    }
}
