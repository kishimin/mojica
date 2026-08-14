using Xunit;

namespace Mojica.Api.Tests.Infrastructure;

public sealed class GlyphForgeClientRegistrationMediumTests
{
    [Fact(Skip = "TODO: Implement from documented test plan.")]
    public void Resolve_WhenConfigurationIsValid_UsesConfiguredBaseAddressAndTimeout()
    {
        // ID: GLYPH-CONFIG-06
        // Source: docs/v1/api/adapters.md sections 13 and 15
        // Given: a test host with valid Glyph Forge client configuration
        // When: the registered HTTP client is resolved from dependency injection
        // Then: its base address and timeout match the validated configuration
        // Level: ASP.NET Core integration
        // Priority: High
    }

    [Fact(Skip = "TODO: Implement from documented test plan.")]
    public void Start_WhenRequiredConfigurationIsMissing_FailsBeforeResolvingClient()
    {
        // ID: GLYPH-CONFIG-07
        // Source: docs/v1/api/adapters.md section 13
        // Given: a test host with missing or invalid Glyph Forge client configuration
        // When: the ASP.NET Core host is built
        // Then: startup or options resolution fails before an outbound HTTP request can be sent
        // Level: ASP.NET Core integration
        // Blocked by: define the production options binding and validation entry point
        // Priority: High
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
