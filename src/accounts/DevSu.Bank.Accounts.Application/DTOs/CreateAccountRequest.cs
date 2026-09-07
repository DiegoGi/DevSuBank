using DevSu.Bank.Accounts.Domain.ValueObjects;
using System.Text.Json.Serialization;

namespace DevSu.Bank.Accounts.Application.DTOs
{
    public record CreateAccountRequest(
        [property: JsonPropertyName("numeroCuenta")] string AccountNumber,
        [property: JsonPropertyName("tipoCuenta")] AccountType AccountType,
        [property: JsonPropertyName("saldoInicial")] decimal InitialBalance,
        [property: JsonPropertyName("clienteId")] int ClientId);
}
