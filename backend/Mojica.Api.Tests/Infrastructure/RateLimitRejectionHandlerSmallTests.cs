using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Primitives;
using Xunit;
using Mojica.Api.Infrastructure;

namespace Mojica.Api.Tests.Infrastructure;

public sealed class RateLimitRejectionHandlerSmallTests
{
    [Fact]
    public async Task WriteAsync_WhenLeaseHasRetryAfterMetadata_SetsStatusAndRetryAfterHeader()
    {
        // ID: RATE-LIMIT-S-008
        // Source: docs/v1/api/controllers.md §6, §10; docs/v1/api/api.md §13
        var options = new RateLimitOptions { PermitLimit = 1, Window = TimeSpan.FromSeconds(30), QueueLimit = 0 };
        using var limiter = ImageGenerationRateLimiterPolicy.CreateLimiter(options);
        using var acquired = limiter.AttemptAcquire(1);
        using var rejected = limiter.AttemptAcquire(1);

        var httpContext = new DefaultHttpContext();
        var context = new OnRejectedContext { HttpContext = httpContext, Lease = rejected };

        await RateLimitRejectionHandler.WriteAsync(context, CancellationToken.None);

        Assert.Equal(StatusCodes.Status429TooManyRequests, httpContext.Response.StatusCode);
        Assert.True(int.TryParse(httpContext.Response.Headers.RetryAfter, out var seconds));
        Assert.True(seconds > 0);
    }

    [Fact]
    public async Task WriteAsync_WhenLeaseHasNoRetryAfterMetadata_SetsStatusWithoutRetryAfterHeader()
    {
        // ID: RATE-LIMIT-S-009
        // Source: docs/v1/api/controllers.md §10
        var httpContext = new DefaultHttpContext();
        var context = new OnRejectedContext { HttpContext = httpContext, Lease = new LeaseWithoutMetadata() };

        await RateLimitRejectionHandler.WriteAsync(context, CancellationToken.None);

        Assert.Equal(StatusCodes.Status429TooManyRequests, httpContext.Response.StatusCode);
        Assert.True(StringValues.IsNullOrEmpty(httpContext.Response.Headers.RetryAfter));
    }

    private sealed class LeaseWithoutMetadata : RateLimitLease
    {
        public override bool IsAcquired => false;

        public override IEnumerable<string> MetadataNames => Array.Empty<string>();

        public override bool TryGetMetadata(string metadataName, out object? metadata)
        {
            metadata = null;
            return false;
        }
    }
}
