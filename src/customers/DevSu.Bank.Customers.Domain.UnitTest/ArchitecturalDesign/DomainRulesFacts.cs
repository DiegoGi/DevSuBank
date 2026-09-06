using DevSu.Bank.Customers.Domain.SeedWork;
using MediatR;
using NetArchTest.Rules;
using Xunit;

namespace DevSu.Bank.Customers.Domain.UnitTest.ArchitecturalDesign
{
    public class DomainRulesFacts
    {
        private const string DomainNamespace = "DevSu.Bank.Customers.Domain";

        [Fact]
        public void EventsShouldBeNamedCorrectlyAndImplementNotification()
        {
            var namespacePattern = $@"^{DomainNamespace}\.AggregateModels\..*\.Events$";

            //Arrange - Act
            var result = Types.InAssembly(typeof(BaseException).Assembly)
                .That()
                .ResideInNamespaceMatching(namespacePattern)
                .Should()
                .HaveNameEndingWith("Event")
                .And()
                .ImplementInterface(typeof(INotification))
                .GetResult();

            //Assert
            Assert.True(result.IsSuccessful);
        }


        [Fact]
        public void SpecificationsShouldBeNamedCorrectlyAndImplementNotification()
        {
            var namespacePattern = $@"^{DomainNamespace}\.AggregateModels\..*\.Specifications";

            //Arrange - Act
            var result = Types.InAssembly(typeof(BaseException).Assembly)
                .That()
                .ResideInNamespaceMatching(namespacePattern)
                .Should()
                .HaveNameEndingWith("Specification")
                .And()
                .Inherit(typeof(Specification<>))
                .GetResult();

            //Assert
            Assert.True(result.IsSuccessful);
        }
    }
}
