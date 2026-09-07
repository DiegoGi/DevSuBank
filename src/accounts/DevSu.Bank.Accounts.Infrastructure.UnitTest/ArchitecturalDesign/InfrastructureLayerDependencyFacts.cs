using DevSu.Bank.Accounts.Infrastructure.Extensions;
using NetArchTest.Rules;
using Xunit;

namespace DevSu.Bank.Accounts.Infrastructure.UnitTest.ArchitecturalDesign
{
    public class InfrastructureLayerDependencyFacts
    {
        [Fact]
        public void NoReferencesFromInfrastructureToPresentation()
        {
            //Arrange - Act
            var result = Types.InAssembly(typeof(DependencyInjectionExtensions).Assembly)
               .That()
               .ResideInNamespace("DevSu.Bank.Accounts.Infrastructure")
               .ShouldNot()
               .HaveDependencyOn("DevSu.Bank.Accounts.Presentation")
               .GetResult();

            //Assert
            Assert.True(result.IsSuccessful);
        }
    }
}
