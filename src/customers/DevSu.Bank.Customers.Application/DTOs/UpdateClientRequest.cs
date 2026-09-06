using DevSu.Bank.Customers.Domain.ValueObjects;
using System.Text.Json.Serialization;

namespace DevSu.Bank.Customers.Application.DTOs
{
    public record UpdateClientRequest(
        [property: JsonPropertyName("nombre")] string Name,
        [property: JsonPropertyName("genero")] Gender Gender,
        [property: JsonPropertyName("edad")] int Age,
        [property: JsonPropertyName("direccion")] string? Address,
        [property: JsonPropertyName("telefono")] string? Phone);
}
