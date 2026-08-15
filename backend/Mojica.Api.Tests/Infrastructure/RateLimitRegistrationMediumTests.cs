namespace Mojica.Api.Tests.Infrastructure;

public sealed class RateLimitRegistrationMediumTests
{
    [Fact(Skip = "TODO: implement local API rate limiting")]
    public void Start_InProduction_WhenConfigurationIsMissing_FailsFast()
    {
        // ID: RATE-LIMIT-M-001
        // Source: docs/v1/api/api.md §13
        // Given: A Production host with no RateLimit configuration
        // When: The host resolves its services
        // Then: Startup fails with an options validation error
        // Priority: High
    }

    [Fact(Skip = "TODO: implement local API rate limiting")]
    public void Start_WhenRateLimitConfigurationIsMissing_AllowsHealthEndpointToStart()
    {
        // ID: RATE-LIMIT-M-002
        // Source: docs/v1/api/api.md §13
        // Given: A Development host with no RateLimit configuration
        // When: A client requests GET /health
        // Then: The host starts and returns 200 OK
        // Priority: Medium
    }

    [Fact(Skip = "TODO: implement local API rate limiting")]
    public void Resolve_WhenConfigurationIsValid_RegistersRateLimiterWithoutThrowing()
    {
        // ID: RATE-LIMIT-M-003
        // Source: docs/v1/api/api.md §13
        // Given: A host configured with valid RateLimit configuration
        // When: The host resolves its services
        // Then: Resolution succeeds and the configured RateLimitOptions are bound correctly
        // Priority: High
    }
}
