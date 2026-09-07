using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Testcontainers.MsSql;

namespace DevSu.Bank.Customers.Presentation.Api.IntegrationTest
{
    public class CustomersApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private const string Database = "DevSuBankCustomers";
        private const string SqlServerImage = "mcr.microsoft.com/mssql/server:2022-latest";
        private const string SqlServerPassword = "DevSu*Bank2026";
        private const string SchemaScript = "BaseDatos.sql";
        private const string BatchSeparator = "GO";

        private readonly MsSqlContainer _sqlServer = new MsSqlBuilder()
            .WithImage(SqlServerImage)
            .WithPassword(SqlServerPassword)
            .Build();

        public async Task InitializeAsync()
        {
            await _sqlServer.StartAsync();
            await CreateSchemaAsync();
        }

        public new async Task DisposeAsync()
        {
            await base.DisposeAsync();
            await _sqlServer.DisposeAsync();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseSetting("Host", $"{_sqlServer.Hostname},{_sqlServer.GetMappedPublicPort(MsSqlBuilder.MsSqlPort)}");
            builder.UseSetting("Database", Database);
            builder.UseSetting("User", "sa");
            builder.UseSetting("Password", SqlServerPassword);
            builder.UseSetting("MessageBrokerHost", string.Empty);
        }

        private async Task CreateSchemaAsync()
        {
            await using var connection = new SqlConnection(_sqlServer.GetConnectionString());
            await connection.OpenAsync();

            foreach (var batch in ReadSchemaBatches())
            {
                await using var command = new SqlCommand(batch, connection);
                await command.ExecuteNonQueryAsync();
            }
        }

        private static IEnumerable<string> ReadSchemaBatches()
        {
            var script = File.ReadAllText(FindSchemaScript());

            return script
                .Split(Environment.NewLine + BatchSeparator, StringSplitOptions.RemoveEmptyEntries)
                .Select(batch => batch.Trim())
                .Where(batch => batch.Length > 0);
        }

        private static string FindSchemaScript()
        {
            var directory = new DirectoryInfo(AppContext.BaseDirectory);

            while (directory is not null)
            {
                var candidate = Path.Combine(directory.FullName, SchemaScript);

                if (File.Exists(candidate))
                {
                    return candidate;
                }

                directory = directory.Parent;
            }

            throw new FileNotFoundException($"No se encontró {SchemaScript} en la jerarquía de directorios.");
        }
    }
}
