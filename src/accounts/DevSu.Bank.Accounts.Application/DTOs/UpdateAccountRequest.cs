using DevSu.Bank.Accounts.Domain.ValueObjects;
using System.Text.Json.Serialization;

namespace DevSu.Bank.Accounts.Application.DTOs
{
    public record UpdateAccountRequest(
        [property: JsonPropertyName("tipoCuenta")] AccountType AccountType,
        [property: JsonPropertyName("estado")] bool Status);
}
