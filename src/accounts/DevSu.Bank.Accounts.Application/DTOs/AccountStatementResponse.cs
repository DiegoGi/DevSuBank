using DevSu.Bank.Accounts.Domain.Resources;
using DevSu.Bank.Accounts.Domain.ValueObjects;
using System.Globalization;
using System.Text.Json.Serialization;

namespace DevSu.Bank.Accounts.Application.DTOs
{
    public record AccountStatementResponse(
        [property: JsonPropertyName("Fecha")] string TransactionDate,
        [property: JsonPropertyName("Cliente")] string ClientName,
        [property: JsonPropertyName("Numero Cuenta")] string AccountNumber,
        [property: JsonPropertyName("Tipo")] string AccountType,
        [property: JsonPropertyName("Saldo Inicial")] decimal InitialBalance,
        [property: JsonPropertyName("Estado")] bool Status,
        [property: JsonPropertyName("Movimiento")] decimal Amount,
        [property: JsonPropertyName("Saldo Disponible")] decimal Balance)
    {
        private const string DateFormat = "d/M/yyyy";

        public static AccountStatementResponse From(AccountStatementData data)
        {
            return new AccountStatementResponse(
                data.TransactionDate.ToString(DateFormat, CultureInfo.InvariantCulture),
                data.ClientName,
                data.AccountNumber,
                Describe((AccountType)data.AccountType),
                data.InitialBalance,
                data.Status,
                data.Amount,
                data.Balance);
        }

        private static string Describe(AccountType accountType) => accountType switch
        {
            Domain.ValueObjects.AccountType.Savings => Generals.AccountTypeSavings,
            Domain.ValueObjects.AccountType.Checking => Generals.AccountTypeChecking,
            _ => accountType.ToString()
        };
    }
}
