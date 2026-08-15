using Xunit;
using Microsoft.Extensions.Options;
using Mojica.Api.Infrastructure;

namespace Mojica.Api.Tests.Infrastructure;

public sealed class RateLimitOptionsSmallTests
{
    [Fact]
    public void Validate_WhenPermitLimitWindowAndQueueLimitAreValid_AcceptsConfiguration()
    {
        // ID: RATE-LIMIT-S-001
        // Source: docs/v1/api/api.md §13
        var options = new RateLimitOptions
        {
            PermitLimit = 10,
            Window = TimeSpan.FromMinutes(1),
            QueueLimit = 0
        };

        var result = new RateLimitOptionsValidator().Validate(null, options);

        Assert.Equal(ValidateOptionsResult.Success, result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_WhenPermitLimitIsNotPositive_RejectsConfiguration(int permitLimit)
    {
        // ID: RATE-LIMIT-S-002
        // Source: docs/v1/api/api.md §13
        var options = new RateLimitOptions
        {
            PermitLimit = permitLimit,
            Window = TimeSpan.FromMinutes(1),
            QueueLimit = 0
        };

        var result = new RateLimitOptionsValidator().Validate(null, options);

        Assert.False(result.Succeeded);
        Assert.Contains("permit limit", result.FailureMessage);
    }

    [Theory]
    [MemberData(nameof(NonPositiveWindows))]
    public void Validate_WhenWindowIsNotPositive_RejectsConfiguration(TimeSpan window)
    {
        // ID: RATE-LIMIT-S-003
        // Source: docs/v1/api/api.md §13
        var options = new RateLimitOptions
        {
            PermitLimit = 10,
            Window = window,
            QueueLimit = 0
        };

        var result = new RateLimitOptionsValidator().Validate(null, options);

        Assert.False(result.Succeeded);
        Assert.Contains("window", result.FailureMessage);
    }

    [Fact]
    public void Validate_WhenQueueLimitIsNegative_RejectsConfiguration()
    {
        // ID: RATE-LIMIT-S-004
        // Source: docs/v1/api/api.md §13
        var options = new RateLimitOptions
        {
            PermitLimit = 10,
            Window = TimeSpan.FromMinutes(1),
            QueueLimit = -1
        };

        var result = new RateLimitOptionsValidator().Validate(null, options);

        Assert.False(result.Succeeded);
        Assert.Contains("queue limit", result.FailureMessage);
    }

    public static TheoryData<TimeSpan> NonPositiveWindows => new()
    {
        TimeSpan.Zero,
        TimeSpan.FromSeconds(-1)
    };
}
