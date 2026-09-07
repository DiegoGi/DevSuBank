using System.Text.Json.Serialization;

namespace DevSu.Bank.Accounts.Application.DTOs
{
    public record CreateTransactionRequest(
        [property: JsonPropertyName("numeroCuenta")] string AccountNumber,
        [property: JsonPropertyName("valor")] decimal Amount);
}
