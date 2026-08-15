using System.Threading.RateLimiting;
using Xunit;
using Mojica.Api.Infrastructure;

namespace Mojica.Api.Tests.Infrastructure;

public sealed class ImageGenerationRateLimiterPolicySmallTests
{
    [Fact]
    public void Acquire_WhenRequestsAreWithinPermitLimit_AcquiresEachLease()
    {
        // ID: RATE-LIMIT-S-005
        // Source: docs/v1/api/api.md §13; docs/v1/api/controllers.md §6, §10
        var options = new RateLimitOptions { PermitLimit = 3, Window = TimeSpan.FromMinutes(1), QueueLimit = 0 };
        using var limiter = ImageGenerationRateLimiterPolicy.CreateLimiter(options);

        for (var i = 0; i < options.PermitLimit; i++)
        {
            using var lease = limiter.AttemptAcquire(1);
            Assert.True(lease.IsAcquired);
        }
    }

    [Fact]
    public void Acquire_WhenRequestsExceedPermitLimit_RejectsAdditionalRequest()
    {
        // ID: RATE-LIMIT-S-006
        // Source: docs/v1/api/api.md §13; docs/v1/api/controllers.md §6, §10
        var options = new RateLimitOptions { PermitLimit = 2, Window = TimeSpan.FromMinutes(1), QueueLimit = 0 };
        using var limiter = ImageGenerationRateLimiterPolicy.CreateLimiter(options);

        using (var first = limiter.AttemptAcquire(1))
        {
            Assert.True(first.IsAcquired);
        }

        using (var second = limiter.AttemptAcquire(1))
        {
            Assert.True(second.IsAcquired);
        }

        using var third = limiter.AttemptAcquire(1);

        Assert.False(third.IsAcquired);
    }

    [Fact]
    public void Acquire_WhenRequestIsRejected_ExposesRetryAfterMetadata()
    {
        // ID: RATE-LIMIT-S-007
        // Source: docs/v1/api/api.md §13; docs/v1/api/controllers.md §10
        var options = new RateLimitOptions { PermitLimit = 1, Window = TimeSpan.FromSeconds(30), QueueLimit = 0 };
        using var limiter = ImageGenerationRateLimiterPolicy.CreateLimiter(options);

        using var first = limiter.AttemptAcquire(1);
        using var rejected = limiter.AttemptAcquire(1);

        Assert.False(rejected.IsAcquired);
        Assert.True(rejected.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter));
        Assert.True(retryAfter > TimeSpan.Zero);
        Assert.True(retryAfter <= options.Window);
    }
}
