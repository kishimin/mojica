namespace Mojica.Api.Tests.Infrastructure;

public sealed class ImageGenerationRateLimiterPolicySmallTests
{
    [Fact(Skip = "TODO: implement local API rate limiting")]
    public void Acquire_WhenRequestsAreWithinPermitLimit_AcquiresEachLease()
    {
        // ID: RATE-LIMIT-S-005
        // Source: docs/v1/api/api.md §13; docs/v1/api/controllers.md §6, §10
        // Given: A limiter configured with a PermitLimit of N and no other requests
        // When: N acquisitions are attempted in sequence
        // Then: Every acquisition succeeds
        // Priority: High
    }

    [Fact(Skip = "TODO: implement local API rate limiting")]
    public void Acquire_WhenRequestsExceedPermitLimit_RejectsAdditionalRequest()
    {
        // ID: RATE-LIMIT-S-006
        // Source: docs/v1/api/api.md §13; docs/v1/api/controllers.md §6, §10
        // Given: A limiter configured with a PermitLimit of N whose permits are already exhausted
        // When: One more acquisition is attempted within the same window
        // Then: The acquisition is rejected without calling the Glyph Forge API
        // Priority: High
    }

    [Fact(Skip = "TODO: implement local API rate limiting")]
    public void Acquire_WhenRequestIsRejected_ExposesRetryAfterMetadata()
    {
        // ID: RATE-LIMIT-S-007
        // Source: docs/v1/api/api.md §13; docs/v1/api/controllers.md §10
        // Given: A limiter whose permits are already exhausted for the current window
        // When: An acquisition is rejected
        // Then: The rejected lease exposes Retry-After metadata consistent with the configured window
        // Priority: High
    }
}
