using DevSu.Bank.Accounts.Application.ReadOnlyModels;
using DevSu.Bank.Accounts.Domain.ValueObjects;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace DevSu.Bank.Accounts.Application.DTOs
{
    public record AccountResponse(
        [property: JsonPropertyName("id")] int Id,
        [property: JsonPropertyName("numeroCuenta")] string AccountNumber,
        [property: JsonPropertyName("tipoCuenta")] AccountType AccountType,
        [property: JsonPropertyName("saldoInicial")] decimal InitialBalance,
        [property: JsonPropertyName("saldoDisponible")] decimal CurrentBalance,
        [property: JsonPropertyName("estado")] bool Status,
        [property: JsonPropertyName("clienteId")] int ClientId,
        [property: JsonPropertyName("cliente")] string ClientName)
    {
        public static readonly Expression<Func<Account, AccountResponse>> Projection = account => new AccountResponse(
            account.Id, account.AccountNumber, (AccountType)account.AccountType, account.InitialBalance,
            account.CurrentBalance, account.Status, account.ClientId, account.Client.Name);
    }
}
