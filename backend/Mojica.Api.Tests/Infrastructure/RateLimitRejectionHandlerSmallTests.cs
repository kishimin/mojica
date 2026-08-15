namespace Mojica.Api.Tests.Infrastructure;

public sealed class RateLimitRejectionHandlerSmallTests
{
    [Fact(Skip = "TODO: implement local API rate limiting")]
    public void WriteAsync_WhenLeaseHasRetryAfterMetadata_SetsStatusAndRetryAfterHeader()
    {
        // ID: RATE-LIMIT-S-008
        // Source: docs/v1/api/controllers.md §6, §10; docs/v1/api/api.md §13
        // Given: A rejected lease that carries Retry-After metadata
        // When: The rejection handler writes the response
        // Then: The response status is 429 and the Retry-After header carries the whole-second value
        // Priority: High
    }

    [Fact(Skip = "TODO: implement local API rate limiting")]
    public void WriteAsync_WhenLeaseHasNoRetryAfterMetadata_SetsStatusWithoutRetryAfterHeader()
    {
        // ID: RATE-LIMIT-S-009
        // Source: docs/v1/api/controllers.md §10
        // Given: A rejected lease that carries no Retry-After metadata
        // When: The rejection handler writes the response
        // Then: The response status is 429 and no Retry-After header is added
        // Error: Do not fabricate a retry hint when the retry timing is unknown
        // Priority: Medium
    }
}
