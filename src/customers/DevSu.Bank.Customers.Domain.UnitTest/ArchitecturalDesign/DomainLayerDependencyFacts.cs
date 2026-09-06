using DevSu.Bank.Customers.Domain.SeedWork;
using NetArchTest.Rules;
using Xunit;

namespace DevSu.Bank.Customers.Domain.UnitTest.ArchitecturalDesign
{
    public class DomainLayerDependencyFacts
    {
        [Fact]
        public void NoReferencesFromDomainToApplication()
        {
            //Arrange - Act
            var result = Types.InAssembly(typeof(BaseException).Assembly)
               .That()
               .ResideInNamespace("DevSu.Bank.Customers.Domain")
               .ShouldNot()
               .HaveDependencyOn("DevSu.Bank.Customers.Application")
               .GetResult();

            //Assert
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void NoReferencesFromDomainToInfrastructure()
        {
            //Arrange - Act
            var result = Types.InAssembly(typeof(BaseException).Assembly)
               .That()
               .ResideInNamespace("DevSu.Bank.Customers.Domain")
               .ShouldNot()
               .HaveDependencyOn("DevSu.Bank.Customers.Infrastructure")
               .GetResult();

            //Assert
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void NoReferencesFromDomainToPresentation()
        {
            //Arrange - Act
            var result = Types.InAssembly(typeof(BaseException).Assembly)
               .That()
               .ResideInNamespace("DevSu.Bank.Customers.Domain")
               .ShouldNot()
               .HaveDependencyOn("DevSu.Bank.Customers.Presentation")
               .GetResult();

            //Assert
            Assert.True(result.IsSuccessful);
        }
    }
}
