using Xunit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Mojica.Api.Infrastructure;

namespace Mojica.Api.Tests.Infrastructure;

public sealed class GlyphForgeClientRegistrationMediumTests
{
    [Fact]
    public void Resolve_WhenConfigurationIsValid_UsesConfiguredBaseAddressAndTimeout()
    {
        using var factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder => builder.ConfigureAppConfiguration((_, configuration) =>
                configuration.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["GlyphForge:BaseUrl"] = "https://glyph-forge.example/",
                    ["GlyphForge:Timeout"] = "00:00:35"
                })));

        var client = factory.Services
            .GetRequiredService<IHttpClientFactory>()
            .CreateClient("GlyphForge");

        Assert.Equal(new Uri("https://glyph-forge.example/"), client.BaseAddress);
        Assert.Equal(TimeSpan.FromSeconds(35), client.Timeout);
    }

    [Fact]
    public void Start_WhenRequiredConfigurationIsMissing_FailsBeforeResolvingClient()
    {
        using var factory = new WebApplicationFactory<Program>();

        var exception = Assert.Throws<OptionsValidationException>(() => _ = factory.Services);

        Assert.Contains("Glyph Forge", exception.ToString());
    }

    [Fact(Skip = "TODO: Implement from documented test plan.")]
    public void Resolve_WhenClientIsConfigured_DoesNotRequireAuthenticationHeader()
    {
        // ID: GLYPH-CONFIG-08
        // Source: docs/v1/api/adapters.md section 15
        // Given: a valid client registration under the current Glyph Forge contract
        // When: the HTTP client is prepared for an outbound request
        // Then: no authentication header is required or added by default
        // Level: ASP.NET Core integration
        // Priority: Medium
    }
}
