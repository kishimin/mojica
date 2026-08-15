using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace Mojica.Api.Tests.Endpoints;

public sealed class HealthEndpointMediumTests : IClassFixture<HealthEndpointFactory>
{
    private readonly HttpClient client;

    public HealthEndpointMediumTests(HealthEndpointFactory factory)
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

public sealed class HealthEndpointFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, configuration) =>
            configuration.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["GlyphForge:BaseUrl"] = "https://glyph-forge.example/",
                ["GlyphForge:Timeout"] = "00:00:35"
            }));
    }
}
