using DevSu.Bank.Accounts.Domain.SeedWork;
using NetArchTest.Rules;
using Xunit;

namespace DevSu.Bank.Accounts.Domain.UnitTest.ArchitecturalDesign
{
    public class DomainLayerDependencyFacts
    {
        [Fact]
        public void NoReferencesFromDomainToApplication()
        {
            //Arrange - Act
            var result = Types.InAssembly(typeof(BaseException).Assembly)
               .That()
               .ResideInNamespace("DevSu.Bank.Accounts.Domain")
               .ShouldNot()
               .HaveDependencyOn("DevSu.Bank.Accounts.Application")
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
               .ResideInNamespace("DevSu.Bank.Accounts.Domain")
               .ShouldNot()
               .HaveDependencyOn("DevSu.Bank.Accounts.Infrastructure")
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
               .ResideInNamespace("DevSu.Bank.Accounts.Domain")
               .ShouldNot()
               .HaveDependencyOn("DevSu.Bank.Accounts.Presentation")
               .GetResult();

            //Assert
            Assert.True(result.IsSuccessful);
        }
    }
}
