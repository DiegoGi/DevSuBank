using DevSu.Bank.Customers.Infrastructure.Extensions;
using NetArchTest.Rules;
using Xunit;

namespace DevSu.Bank.Customers.Infrastructure.UnitTest.ArchitecturalDesign
{
    public class InfrastructureLayerDependencyFacts
    {
        [Fact]
        public void NoReferencesFromInfrastructureToPresentation()
        {
            //Arrange - Act
            var result = Types.InAssembly(typeof(DependencyInjectionExtensions).Assembly)
               .That()
               .ResideInNamespace("DevSu.Bank.Customers.Infrastructure")
               .ShouldNot()
               .HaveDependencyOn("DevSu.Bank.Customers.Presentation")
               .GetResult();

            //Assert
            Assert.True(result.IsSuccessful);
        }
    }
}
