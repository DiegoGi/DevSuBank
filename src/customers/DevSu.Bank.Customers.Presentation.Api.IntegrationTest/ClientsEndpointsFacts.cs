using DevSu.Bank.Customers.Application.DTOs;
using DevSu.Bank.Customers.Domain.ValueObjects;
using System.Net;
using System.Net.Http.Json;

namespace DevSu.Bank.Customers.Presentation.Api.IntegrationTest
{
    public class ClientsEndpointsFacts(CustomersApiFactory factory) : IClassFixture<CustomersApiFactory>
    {
        private const string Endpoint = "/api/v1/clientes";

        private static CreateClientRequest CreateRequest(string clientId, string identification)
        {
            return new CreateClientRequest("John Doe", Gender.Male, 41, identification,
                "742 Evergreen Terrace", "5551234567", clientId, "plain-password");
        }

        [Fact]
        public async Task CreatingAClientShouldPersistItAndReturnItOnRead()
        {
            //Arrange
            var client = factory.CreateClient();
            var request = CreateRequest("CLI-INT-01", "0102030411");

            //Act
            var creation = await client.PostAsJsonAsync(Endpoint, request);
            var created = await creation.Content.ReadFromJsonAsync<CreatedClient>();
            var read = await client.GetAsync($"{Endpoint}/{created!.Id}");
            var found = await read.Content.ReadFromJsonAsync<ClientResponse>();

            //Assert
            Assert.Equal(HttpStatusCode.Created, creation.StatusCode);
            Assert.Equal(HttpStatusCode.OK, read.StatusCode);
            Assert.Equal("John Doe", found!.Name);
            Assert.Equal(Gender.Male, found.Gender);
            Assert.Equal("CLI-INT-01", found.ClientId);
            Assert.True(found.Status);
        }

        [Fact]
        public async Task CreatedClientShouldNeverExposeThePassword()
        {
            //Arrange
            var client = factory.CreateClient();
            var request = CreateRequest("CLI-INT-02", "0102030412");

            //Act
            var creation = await client.PostAsJsonAsync(Endpoint, request);
            var created = await creation.Content.ReadFromJsonAsync<CreatedClient>();
            var read = await client.GetAsync($"{Endpoint}/{created!.Id}");
            var payload = await read.Content.ReadAsStringAsync();

            //Assert
            Assert.DoesNotContain("plain-password", payload);
            Assert.DoesNotContain("contrasena", payload);
        }

        [Fact]
        public async Task CreatingAClientWithADuplicatedClientIdShouldFail()
        {
            //Arrange
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Add("Accept-Language", "es");
            var request = CreateRequest("CLI-INT-03", "0102030413");
            await client.PostAsJsonAsync(Endpoint, request);

            //Act
            var response = await client.PostAsJsonAsync(Endpoint, CreateRequest("CLI-INT-03", "0102030414"));
            var error = await response.Content.ReadFromJsonAsync<ErrorPayload>();

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("Ya existe un cliente con ese clienteid", error!.Message);
        }

        [Fact]
        public async Task CreatingAnInvalidClientShouldReturnEveryValidationError()
        {
            //Arrange
            var client = factory.CreateClient();
            var request = new CreateClientRequest(string.Empty, (Gender)9, 300, string.Empty,
                null, null, string.Empty, "1");

            //Act
            var response = await client.PostAsJsonAsync(Endpoint, request);
            var error = await response.Content.ReadFromJsonAsync<ErrorPayload>();

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.True(error!.Details.Count >= 4);
        }

        [Fact]
        public async Task DeletingAClientShouldHideItFromTheReadModel()
        {
            //Arrange
            var client = factory.CreateClient();
            var creation = await client.PostAsJsonAsync(Endpoint, CreateRequest("CLI-INT-04", "0102030415"));
            var created = await creation.Content.ReadFromJsonAsync<CreatedClient>();

            //Act
            var deletion = await client.DeleteAsync($"{Endpoint}/{created!.Id}");
            var read = await client.GetAsync($"{Endpoint}/{created.Id}");

            //Assert
            Assert.Equal(HttpStatusCode.NoContent, deletion.StatusCode);
            Assert.Equal(HttpStatusCode.NotFound, read.StatusCode);
        }

        [Fact]
        public async Task ReadingAMissingClientShouldReturnNotFound()
        {
            //Arrange
            var client = factory.CreateClient();

            //Act
            var response = await client.GetAsync($"{Endpoint}/999999");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        private record CreatedClient(int Id);

        private record ErrorPayload(string Message, int HttpStatusCode, IReadOnlyList<string> Details);
    }
}
