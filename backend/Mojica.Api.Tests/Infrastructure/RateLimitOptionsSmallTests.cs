namespace Mojica.Api.Tests.Infrastructure;

public sealed class RateLimitOptionsSmallTests
{
    [Fact(Skip = "TODO: implement local API rate limiting")]
    public void Validate_WhenPermitLimitWindowAndQueueLimitAreValid_AcceptsConfiguration()
    {
        // ID: RATE-LIMIT-S-001
        // Source: docs/v1/api/api.md §13
        // Given: A positive PermitLimit, a positive Window, and a non-negative QueueLimit
        // When: The rate limit configuration is validated
        // Then: Validation succeeds
        // Priority: High
    }

    [Fact(Skip = "TODO: implement local API rate limiting")]
    public void Validate_WhenPermitLimitIsNotPositive_RejectsConfiguration()
    {
        // ID: RATE-LIMIT-S-002
        // Source: docs/v1/api/api.md §13
        // Given: A PermitLimit of zero or a negative value
        // When: The rate limit configuration is validated
        // Then: Validation fails with a message identifying the permit limit
        // Priority: High
        // Theory candidate: zero and negative PermitLimit
    }

    [Fact(Skip = "TODO: implement local API rate limiting")]
    public void Validate_WhenWindowIsNotPositive_RejectsConfiguration()
    {
        // ID: RATE-LIMIT-S-003
        // Source: docs/v1/api/api.md §13
        // Given: A Window of TimeSpan.Zero or a negative TimeSpan
        // When: The rate limit configuration is validated
        // Then: Validation fails with a message identifying the window
        // Priority: High
        // Theory candidate: zero and negative Window
    }

    [Fact(Skip = "TODO: implement local API rate limiting")]
    public void Validate_WhenQueueLimitIsNegative_RejectsConfiguration()
    {
        // ID: RATE-LIMIT-S-004
        // Source: docs/v1/api/api.md §13
        // Given: A negative QueueLimit
        // When: The rate limit configuration is validated
        // Then: Validation fails with a message identifying the queue limit
        // Priority: Medium
    }
}
