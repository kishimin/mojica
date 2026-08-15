using System.Net;
using Xunit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Mojica.Api.Infrastructure;

namespace Mojica.Api.Tests.Infrastructure;

public sealed class RateLimitRegistrationMediumTests
{
    [Fact]
    public void Start_InProduction_WhenConfigurationIsMissing_FailsFast()
    {
        // ID: RATE-LIMIT-M-001
        // Source: docs/v1/api/api.md §13
        using var factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder => builder.UseEnvironment("Production"));

        var exception = Assert.ThrowsAny<Exception>(() => _ = factory.Services);

        Assert.Contains("Rate limit permit limit must be positive.", exception.ToString());
    }

    [Fact]
    public async Task Start_WhenRateLimitConfigurationIsMissing_AllowsHealthEndpointToStart()
    {
        // ID: RATE-LIMIT-M-002
        // Source: docs/v1/api/api.md §13
        using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();

        using var response = await client.GetAsync("/health");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public void Resolve_WhenConfigurationIsValid_RegistersRateLimiterWithoutThrowing()
    {
        // ID: RATE-LIMIT-M-003
        // Source: docs/v1/api/api.md §13
        using var factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder => builder.ConfigureAppConfiguration((_, configuration) =>
                configuration.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["RateLimit:PermitLimit"] = "10",
                    ["RateLimit:Window"] = "00:01:00",
                    ["RateLimit:QueueLimit"] = "0"
                })));

        var options = factory.Services.GetRequiredService<IOptions<RateLimitOptions>>().Value;

        Assert.Equal(10, options.PermitLimit);
        Assert.Equal(TimeSpan.FromMinutes(1), options.Window);
        Assert.Equal(0, options.QueueLimit);
    }
}
