using DevSu.Bank.Customers.Presentation.Api.Extensions;
using NetArchTest.Rules;
using Xunit;

namespace DevSu.Bank.Customers.Presentation.Api.UnitTest.ArchitecturalDesign
{
    public class PresentationApiLayerRulesFacts
    {
        [Fact]
        public void ExtensionsClassesShouldBeNamedCorrectly()
        {
            //Arrange - Act
            var result = Types.InAssembly(typeof(DependencyInjectionExtensions).Assembly)
                .That()
                .ResideInNamespace("DevSu.Bank.Customers.Presentation.Api.Controllers")
                .Should().HaveNameEndingWith("Controller")
                .GetResult();

            //Assert
            Assert.True(result.IsSuccessful);
        }
    }
}
