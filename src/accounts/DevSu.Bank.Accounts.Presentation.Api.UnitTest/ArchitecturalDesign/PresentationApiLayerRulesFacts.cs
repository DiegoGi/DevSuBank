using DevSu.Bank.Accounts.Presentation.Api.Extensions;
using NetArchTest.Rules;
using Xunit;

namespace DevSu.Bank.Accounts.Presentation.Api.UnitTest.ArchitecturalDesign
{
    public class PresentationApiLayerRulesFacts
    {
        [Fact]
        public void ExtensionsClassesShouldBeNamedCorrectly()
        {
            //Arrange - Act
            var result = Types.InAssembly(typeof(DependencyInjectionExtensions).Assembly)
                .That()
                .ResideInNamespace("DevSu.Bank.Accounts.Presentation.Api.Controllers")
                .Should().HaveNameEndingWith("Controller")
                .GetResult();

            //Assert
            Assert.True(result.IsSuccessful);
        }
    }
}
