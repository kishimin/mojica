using System.Net;
using Xunit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Mojica.Api.Infrastructure;
using Mojica.Api.Ports;

namespace Mojica.Api.Tests.Infrastructure;

public sealed class GlyphForgeClientRegistrationMediumTests
{
    [Fact]
    public void Start_InProduction_WhenConfigurationIsMissing_FailsFast()
    {
        using var factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder => builder.UseEnvironment("Production"));

        var exception = Assert.ThrowsAny<Exception>(() => _ = factory.Services);

        OptionsStartupValidationAssert.ContainsValidationFailure(exception, "Glyph Forge base URL is required.");
    }

    [Fact]
    public async Task Start_WhenGlyphForgeConfigurationIsMissing_AllowsHealthEndpointToStart()
    {
        using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();

        using var response = await client.GetAsync("/health");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

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
    public void Resolve_WhenRequiredConfigurationIsMissing_FailsBeforeSendingRequest()
    {
        using var factory = new WebApplicationFactory<Program>();

        var clientFactory = factory.Services.GetRequiredService<IHttpClientFactory>();

        var exception = Assert.Throws<OptionsValidationException>(
            () => clientFactory.CreateClient("GlyphForge"));

        Assert.Contains("Glyph Forge", exception.ToString());
    }

    [Fact]
    public void Resolve_WhenClientIsConfigured_DoesNotRequireAuthenticationHeader()
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

        Assert.Null(client.DefaultRequestHeaders.Authorization);
    }

    [Fact]
    public void Resolve_WhenConfigurationIsValid_ResolvesImageGenerationPort()
    {
        using var factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder => builder.ConfigureAppConfiguration((_, configuration) =>
                configuration.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["GlyphForge:BaseUrl"] = "https://glyph-forge.example/",
                    ["GlyphForge:Timeout"] = "00:00:35"
                })));

        using var scope = factory.Services.CreateScope();
        var port = scope.ServiceProvider.GetRequiredService<ImageGenerationPort>();

        Assert.IsType<GlyphForgeImageGenerationAdapter>(port);
    }
}
