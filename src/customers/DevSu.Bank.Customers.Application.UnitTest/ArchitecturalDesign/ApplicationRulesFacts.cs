using DevSu.Bank.Customers.Application.SeedWork;
using DevSu.Bank.Customers.Domain.SeedWork;
using MediatR;
using NetArchTest.Rules;
using Xunit;

namespace DevSu.Bank.Customers.Application.UnitTest.ArchitecturalDesign
{
    public class ApplicationRulesFacts
    {
        [Fact]
        public void EventHandlersShouldBeNamedCorrectlyAndImplementNotificationHandler()
        {
            //Arrange - Act
            var domainEventHandlersResult = Types.InAssembly(typeof(QueryOptions).Assembly)
                .That()
                .ResideInNamespaceStartingWith("DevSu.Bank.Customers.Application.DomainEventHandlers")
                .Should()
                .HaveNameEndingWith("EventHandler")
                .And()
                .ImplementInterface(typeof(INotificationHandler<>))
                .GetResult();


            var applicationEventHandlersResult = Types.InAssembly(typeof(QueryOptions).Assembly)
                .That()
                .ResideInNamespaceStartingWith("DevSu.Bank.Customers.Application.Events")
                .And()
                .ImplementInterface(typeof(INotificationHandler<>))
                .Should()
                .HaveNameEndingWith("EventHandler")
                .GetResult();

            //Assert
            Assert.True(domainEventHandlersResult.IsSuccessful);
            Assert.True(applicationEventHandlersResult.IsSuccessful);
        }

        [Fact]
        public void CommandsShouldBeNamedCorrectly()
        {
            //Arrange - Act
            var resultGeneric = Types.InAssembly(typeof(QueryOptions).Assembly)
                .That()
                .ResideInNamespaceStartingWith("DevSu.Bank.Customers.Application.Commands")
                .And()
                .ImplementInterface(typeof(IRequest<>))
                .Should()
                .HaveNameEndingWith("Command")
                .GetResult();

            var result = Types.InAssembly(typeof(QueryOptions).Assembly)
                .That()
                .ResideInNamespaceStartingWith("DevSu.Bank.Customers.Application.Commands")
                .And()
                .ImplementInterface(typeof(IRequest))
                .Should()
                .HaveNameEndingWith("Command")
                .GetResult();

            //Assert
            Assert.True(resultGeneric.IsSuccessful);
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void CommandHandlersShouldBeNamedCorrectly()
        {
            //Arrange - Act
            var resultGeneric = Types.InAssembly(typeof(QueryOptions).Assembly)
                .That()
                .ResideInNamespaceStartingWith("DevSu.Bank.Customers.Application.Commands")
                .And()
                .ImplementInterface(typeof(IRequestHandler<>))
                .Should()
                .HaveNameEndingWith("CommandHandler")
                .GetResult();

            var resultGeneric2 = Types.InAssembly(typeof(QueryOptions).Assembly)
                .That()
                .ResideInNamespaceStartingWith("DevSu.Bank.Customers.Application.Commands")
                .And()
                .ImplementInterface(typeof(IRequestHandler<,>))
                .Should()
                .HaveNameEndingWith("CommandHandler")
                .GetResult();

            //Assert
            Assert.True(resultGeneric.IsSuccessful);
            Assert.True(resultGeneric2.IsSuccessful);
        }

        [Fact]
        public void ExceptionsShouldBeNamedCorrectly()
        {
            //Arrange - Act
            var resultGeneric = Types.InAssembly(typeof(QueryOptions).Assembly)
                .That()
                .ResideInNamespaceStartingWith("DevSu.Bank.Customers.Application.Exceptions")
                .And()
                .Inherit(typeof(BaseException))
                .Should()
                .HaveNameEndingWith("Exception")
                .GetResult();

            //Assert
            Assert.True(resultGeneric.IsSuccessful);
        }


        [Fact]
        public void ServicesClassesShouldBeNamedCorrectly()
        {
            //Arrange - Act
            var result = Types.InAssembly(typeof(QueryOptions).Assembly)
                .That()
                .ResideInNamespace("DevSu.Bank.Customers.Application.Services")
                .Should().HaveNameEndingWith("Service")
                .GetResult();

            //Assert
            Assert.True(result.IsSuccessful);
        }


        [Fact]
        public void InfrastructureServicesFolderShouldOnlyContainInterfaces()
        {
            //Arrange - Act
            var result = Types.InAssembly(typeof(QueryOptions).Assembly)
                .That()
                .ResideInNamespace("DevSu.Bank.Customers.Application.Services.Infrastructure")
                .Should()
                .BeInterfaces()
                .GetResult();

            //Assert
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public void ExtensionsClassesShouldBeNamedCorrectly()
        {
            //Arrange - Act
            var result = Types.InAssembly(typeof(QueryOptions).Assembly)
                .That()
                .ResideInNamespace("DevSu.Bank.Customers.Application.Extensions")
                .Should().HaveNameEndingWith("Extensions")
                .GetResult();

            //Assert
            Assert.True(result.IsSuccessful);
        }
    }
}
