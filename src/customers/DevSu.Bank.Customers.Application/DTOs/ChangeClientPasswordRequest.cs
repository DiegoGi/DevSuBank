using System.Text.Json.Serialization;

namespace DevSu.Bank.Customers.Application.DTOs
{
    public record ChangeClientPasswordRequest(
        [property: JsonPropertyName("contrasena")] string Password);
}
