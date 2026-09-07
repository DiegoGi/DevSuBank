using DevSu.Bank.Customers.Application.ReadOnlyRepositories;
using DevSu.Bank.Customers.Application.Services.Infrastructure;
using DevSu.Bank.Customers.Domain.AggregateModels.ClientAggregate;
using DevSu.Bank.Customers.Infrastructure.AggregateDataContext;
using DevSu.Bank.Customers.Infrastructure.AggregateRepositories;
using DevSu.Bank.Customers.Infrastructure.ReadOnlyDataContext;
using DevSu.Bank.Customers.Infrastructure.ReadOnlyRepositories;
using DevSu.Bank.Customers.Infrastructure.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DevSu.Bank.Customers.Infrastructure.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddDatabaseContext(configuration)
                .AddAggregateRepositories()
                .AddReadOnlyRepositories()
                .AddServices()
                .AddEventBus(configuration);

            return services;
        }

        private static IServiceCollection AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
        {
            var host = configuration.GetValue<string>("Host");
            var database = configuration.GetValue<string>("Database");
            var user = configuration.GetValue<string>("User");
            var password = configuration.GetValue<string>("Password");

            var connectionString = $"Server=tcp:{host};Initial Catalog={database};Persist Security Info=False;User ID={user};Password={password};MultipleActiveResultSets=False;Connection Timeout=30;TrustServerCertificate=True;";
            services.AddDbContext<AggregateContext>(options =>
                                                  options
                                                  .UseLazyLoadingProxies()
                                                  .UseSqlServer(connectionString));

            services.AddDbContext<ReadOnlyContext>(options =>
                                                 options
                                                 .UseLazyLoadingProxies()
                                                 .UseSqlServer(connectionString));

            return services;
        }

        private static IServiceCollection AddAggregateRepositories(this IServiceCollection services)
        {
            services.AddScoped<IClientRepository, ClientRepository>();

            return services;
        }

        private static IServiceCollection AddReadOnlyRepositories(this IServiceCollection services)
        {
            services.AddScoped<IClientReadOnlyRepository, ClientReadOnlyRepository>();

            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IPasswordHasherService, PasswordHasherService>();
            services.AddScoped<IEventBusService, EventBusService>();

            return services;
        }

        private static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            var host = configuration.GetValue<string>("MessageBrokerHost");
            var user = configuration.GetValue<string>("MessageBrokerUser");
            var password = configuration.GetValue<string>("MessageBrokerPassword");

            services.AddMassTransit(busConfigurator =>
            {
                busConfigurator.SetKebabCaseEndpointNameFormatter();

                if (string.IsNullOrWhiteSpace(host))
                {
                    busConfigurator.UsingInMemory((context, busFactoryConfigurator) =>
                        busFactoryConfigurator.ConfigureEndpoints(context));

                    return;
                }

                busConfigurator.UsingRabbitMq((context, busFactoryConfigurator) =>
                {
                    busFactoryConfigurator.Host(host, hostConfigurator =>
                    {
                        hostConfigurator.Username(user!);
                        hostConfigurator.Password(password!);
                    });

                    busFactoryConfigurator.MessageTopology.SetEntityNameFormatter(
                        new MessageUrnEntityNameFormatter());

                    busFactoryConfigurator.ConfigureEndpoints(context);
                });
            });

            return services;
        }
    }
}
