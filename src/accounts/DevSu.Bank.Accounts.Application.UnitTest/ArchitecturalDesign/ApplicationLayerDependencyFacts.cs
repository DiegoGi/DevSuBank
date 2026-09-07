using DevSu.Bank.Accounts.Application.SeedWork;
using NetArchTest.Rules;
using Xunit;

namespace DevSu.Bank.Accounts.Application.UnitTest.ArchitecturalDesign
{
    public class ApplicationLayerDependencyFacts
    {
        [Fact]
        public void NoReferencesFromApplicationToInfrastructure()
        {
            //Arrange - Act
            var result = Types.InAssembly(typeof(QueryOptions).Assembly)
               .That()
               .ResideInNamespace("DevSu.Bank.Accounts.Application")
               .ShouldNot()
               .HaveDependencyOn("DevSu.Bank.Accounts.Infrastructure")
               .GetResult();

            //Assert
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void NoReferencesFromApplicationToPresentation()
        {
            //Arrange - Act
            var result = Types.InAssembly(typeof(QueryOptions).Assembly)
               .That()
               .ResideInNamespace("DevSu.Bank.Accounts.Application")
               .ShouldNot()
               .HaveDependencyOn("DevSu.Bank.Accounts.Presentation")
               .GetResult();

            //Assert
            Assert.True(result.IsSuccessful);
        }
    }
}
