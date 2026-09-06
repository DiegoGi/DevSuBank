using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DevSu.Bank.Customers.Domain.AggregateModels.ClientAggregate;
using DevSu.Bank.Customers.Infrastructure.AggregateDataContext;
using DevSu.Bank.Customers.Application.ReadOnlyRepositories;
using DevSu.Bank.Customers.Application.Services.Infrastructure;
using DevSu.Bank.Customers.Infrastructure.AggregateRepositories;
using DevSu.Bank.Customers.Infrastructure.ReadOnlyRepositories;
using DevSu.Bank.Customers.Infrastructure.Services;
using DevSu.Bank.Customers.Infrastructure.ReadOnlyDataContext;

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
                .AddServices();

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

            return services;
        }
    }
}
