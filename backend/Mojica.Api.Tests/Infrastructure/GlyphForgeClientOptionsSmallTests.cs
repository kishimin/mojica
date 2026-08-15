using Xunit;
using Microsoft.Extensions.Options;

namespace Mojica.Api.Tests.Infrastructure;

public sealed class GlyphForgeClientOptionsSmallTests
{
    [Fact]
    public void Validate_WhenBaseUrlAndTimeoutAreValid_AcceptsConfiguration()
    {
        var options = new GlyphForgeClientOptions
        {
            BaseUrl = new Uri("https://glyph-forge.example/"),
            Timeout = TimeSpan.FromSeconds(20)
        };

        var result = new GlyphForgeClientOptionsValidator().Validate(null, options);

        Assert.Equal(ValidateOptionsResult.Success, result);
    }

    [Fact(Skip = "TODO: Implement from documented test plan.")]
    public void Validate_WhenBaseUrlIsMissing_RejectsConfiguration()
    {
        // ID: GLYPH-CONFIG-02
        // Source: docs/v1/api/adapters.md section 13
        // Given: a configuration without a Glyph Forge base URL
        // When: the client options are validated
        // Then: configuration validation fails before an HTTP client is used
        // Level: Unit
        // Error: missing required external service configuration
        // Priority: High
    }

    [Fact(Skip = "TODO: Implement from documented test plan.")]
    public void Validate_WhenBaseUrlIsNotAbsoluteHttpUrl_RejectsConfiguration()
    {
        // ID: GLYPH-CONFIG-03
        // Source: docs/v1/api/adapters.md section 13
        // Given: a missing scheme or otherwise non-absolute Glyph Forge URL
        // When: the client options are validated
        // Then: configuration validation fails without silently accepting a relative URL
        // Level: Unit
        // Error: base URL cannot identify the external service
        // Priority: High
    }

    [Fact(Skip = "TODO: Implement from documented test plan.")]
    public void Validate_WhenTimeoutIsNotPositive_RejectsConfiguration()
    {
        // ID: GLYPH-CONFIG-04
        // Source: docs/v1/api/adapters.md section 15
        // Given: a zero or negative connection/response timeout
        // When: the client options are validated
        // Then: configuration validation fails before a request can wait indefinitely or immediately time out
        // Level: Unit
        // Error: timeout must be positive
        // Priority: High
    }

    [Fact(Skip = "TODO: Implement from documented test plan.")]
    public void Validate_WhenTimeoutIsThirtyFiveSeconds_AcceptsProviderLimitConfiguration()
    {
        // ID: GLYPH-CONFIG-05
        // Source: docs/v1/api/adapters.md section 15
        // Given: a timeout of 35 seconds for a provider with a 30-second generation limit
        // When: the client options are validated
        // Then: the provider timeout configuration is accepted
        // Level: Unit
        // Priority: Medium
    }
}
