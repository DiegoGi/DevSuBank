using DevSu.Bank.Customers.Application.ReadOnlyModels;
using DevSu.Bank.Customers.Domain.ValueObjects;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace DevSu.Bank.Customers.Application.DTOs
{
    public record ClientResponse(
        [property: JsonPropertyName("id")] int Id,
        [property: JsonPropertyName("nombre")] string Name,
        [property: JsonPropertyName("genero")] Gender Gender,
        [property: JsonPropertyName("edad")] int Age,
        [property: JsonPropertyName("identificacion")] string Identification,
        [property: JsonPropertyName("direccion")] string? Address,
        [property: JsonPropertyName("telefono")] string? Phone,
        [property: JsonPropertyName("clienteid")] string ClientId,
        [property: JsonPropertyName("estado")] bool Status)
    {
        public static readonly Expression<Func<Client, ClientResponse>> Projection = client => new ClientResponse(
            client.Id, client.Name, (Gender)client.Gender, client.Age, client.Identification,
            client.Address, client.Phone, client.ClientId, client.Status);
    }
}
