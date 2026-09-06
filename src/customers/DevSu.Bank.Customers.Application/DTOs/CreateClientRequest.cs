using DevSu.Bank.Customers.Domain.ValueObjects;
using System.Text.Json.Serialization;

namespace DevSu.Bank.Customers.Application.DTOs
{
    public record CreateClientRequest(
        [property: JsonPropertyName("nombre")] string Name,
        [property: JsonPropertyName("genero")] Gender Gender,
        [property: JsonPropertyName("edad")] int Age,
        [property: JsonPropertyName("identificacion")] string Identification,
        [property: JsonPropertyName("direccion")] string? Address,
        [property: JsonPropertyName("telefono")] string? Phone,
        [property: JsonPropertyName("clienteid")] string ClientId,
        [property: JsonPropertyName("contrasena")] string Password);
}
