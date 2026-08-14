using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Mojica.Api.Tests.Endpoints;

public sealed class HealthEndpointMediumTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient client;

    public HealthEndpointMediumTests(WebApplicationFactory<Program> factory)
    {
        client = factory.CreateClient();
    }

    [Fact]
    public async Task GetHealth_returns_ok_with_status_payload()
    {
        using var response = await client.GetAsync("/health");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("{\"status\":\"ok\"}", await response.Content.ReadAsStringAsync());
    }
}
