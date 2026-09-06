using DevSu.Bank.Customers.Infrastructure.Extensions;
using NetArchTest.Rules;
using Xunit;

namespace DevSu.Bank.Customers.Infrastructure.UnitTest.ArchitecturalDesign
{
    public class InfrastructureLayerRulesFacts
    {
        [Fact]
        public void RepositoriesClassesShouldBeNamedCorrectly()
        {
            //Arrange - Act
            var result = Types.InAssembly(typeof(DependencyInjectionExtensions).Assembly)
                .That()
                .ResideInNamespace("DevSu.Bank.Customers.Infrastructure.AggregateRepositories")
                .Should().HaveNameEndingWith("Repository")
                .GetResult();

            //Assert
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void ReadOnlyRepositoriesClassesShouldBeNamedCorrectly()
        {
            //Arrange - Act
            var result = Types.InAssembly(typeof(DependencyInjectionExtensions).Assembly)
                .That()
                .ResideInNamespace("DevSu.Bank.Customers.Infrastructure.ReadOnlyRepositories")
                .Should().HaveNameEndingWith("ReadOnlyRepository")
                .GetResult();

            //Assert
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void ServicesClassesShouldBeNamedCorrectly()
        {
            //Arrange - Act
            var result = Types.InAssembly(typeof(DependencyInjectionExtensions).Assembly)
                .That()
                .ResideInNamespace("DevSu.Bank.Customers.Infrastructure.Services")
                .Should().HaveNameEndingWith("Service")
                .GetResult();

            //Assert
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void ExtensionsClassesShouldBeNamedCorrectly()
        {
            //Arrange - Act
            var result = Types.InAssembly(typeof(DependencyInjectionExtensions).Assembly)
                .That()
                .ResideInNamespace("DevSu.Bank.Customers.Infrastructure.Extensions")
                .Should().HaveNameEndingWith("Extensions")
                .GetResult();

            //Assert
            Assert.True(result.IsSuccessful);
        }
    }
}
