using System.Reflection;
using DevSu.Bank.Accounts.Application.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace DevSu.Bank.Accounts.Application.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddMediatr()
                .AddValidators()
                .AddBehaviors(configuration);

            return services;
        }

        private static IServiceCollection AddMediatr(this IServiceCollection services)
        {
           return services.AddMediatR(mediatrServiceConfiguration => mediatrServiceConfiguration.RegisterServicesFromAssembly(typeof(DependencyInjectionExtensions).Assembly));
        }

        private static IServiceCollection AddValidators(this IServiceCollection services)
        {
            return services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        }

        private static IServiceCollection AddBehaviors(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
        }
    }
}
