using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DevSu.Bank.Accounts.Domain.AggregateModels.AccountAggregate;
using DevSu.Bank.Accounts.Domain.AggregateModels.ClientAggregate;
using DevSu.Bank.Accounts.Infrastructure.AggregateDataContext;
using DevSu.Bank.Accounts.Application.ReadOnlyRepositories;
using DevSu.Bank.Accounts.Infrastructure.AggregateRepositories;
using DevSu.Bank.Accounts.Infrastructure.ReadOnlyRepositories;
using DevSu.Bank.Accounts.Infrastructure.ReadOnlyDataContext;

namespace DevSu.Bank.Accounts.Infrastructure.Extensions
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
            services.AddDbContext<AggregateContext>(options => options.UseSqlServer(connectionString));

            services.AddDbContext<ReadOnlyContext>(options =>
                                                 options
                                                 .UseLazyLoadingProxies()
                                                 .UseSqlServer(connectionString));

            return services;
        }

        private static IServiceCollection AddAggregateRepositories(this IServiceCollection services)
        {
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IClientRepository, ClientRepository>();

            return services;
        }

        private static IServiceCollection AddReadOnlyRepositories(this IServiceCollection services)
        {
            services.AddScoped<IAccountReadOnlyRepository, AccountReadOnlyRepository>();
            services.AddScoped<ITransactionReadOnlyRepository, TransactionReadOnlyRepository>();

            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services;
        }
    }
}
