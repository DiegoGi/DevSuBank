using DevSu.Bank.Customers.Application.SeedWork;
using NetArchTest.Rules;
using Xunit;

namespace DevSu.Bank.Customers.Application.UnitTest.ArchitecturalDesign
{
    public class ApplicationLayerDependencyFacts
    {
        [Fact]
        public void NoReferencesFromApplicationToInfrastructure()
        {
            //Arrange - Act
            var result = Types.InAssembly(typeof(QueryOptions).Assembly)
               .That()
               .ResideInNamespace("DevSu.Bank.Customers.Application")
               .ShouldNot()
               .HaveDependencyOn("DevSu.Bank.Customers.Infrastructure")
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
               .ResideInNamespace("DevSu.Bank.Customers.Application")
               .ShouldNot()
               .HaveDependencyOn("DevSu.Bank.Customers.Presentation")
               .GetResult();

            //Assert
            Assert.True(result.IsSuccessful);
        }
    }
}
