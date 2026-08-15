using System.Text.Json;
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
    public async Task WriteAsync_WhenLeaseHasRetryAfterMetadata_SetsStatusRetryAfterHeaderAndLocalizedBody()
    {
        var options = new RateLimitOptions { PermitLimit = 1, Window = TimeSpan.FromSeconds(30), QueueLimit = 0 };
        using var limiter = ImageGenerationRateLimiterPolicy.CreateLimiter(options);
        using var acquired = limiter.AttemptAcquire(1);
        using var rejected = limiter.AttemptAcquire(1);

        var httpContext = new DefaultHttpContext { Response = { Body = new MemoryStream() } };
        httpContext.Request.Headers.AcceptLanguage = "en";
        var context = new OnRejectedContext { HttpContext = httpContext, Lease = rejected };

        await RateLimitRejectionHandler.WriteAsync(context, CancellationToken.None);

        Assert.Equal(StatusCodes.Status429TooManyRequests, httpContext.Response.StatusCode);
        Assert.True(int.TryParse(httpContext.Response.Headers.RetryAfter, out var seconds));
        Assert.True(seconds > 0);

        httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
        using var document = await JsonDocument.ParseAsync(httpContext.Response.Body);
        Assert.Equal("RATE_LIMIT_EXCEEDED", document.RootElement.GetProperty("code").GetString());
        Assert.Equal(
            "The request limit has been exceeded. Please try again later.",
            document.RootElement.GetProperty("message").GetString());
    }

    [Fact]
    public async Task WriteAsync_WhenAcceptLanguageIsJapanese_WritesJapaneseBody()
    {
        var options = new RateLimitOptions { PermitLimit = 1, Window = TimeSpan.FromSeconds(30), QueueLimit = 0 };
        using var limiter = ImageGenerationRateLimiterPolicy.CreateLimiter(options);
        using var acquired = limiter.AttemptAcquire(1);
        using var rejected = limiter.AttemptAcquire(1);

        var httpContext = new DefaultHttpContext { Response = { Body = new MemoryStream() } };
        httpContext.Request.Headers.AcceptLanguage = "ja";
        var context = new OnRejectedContext { HttpContext = httpContext, Lease = rejected };

        await RateLimitRejectionHandler.WriteAsync(context, CancellationToken.None);

        httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
        using var document = await JsonDocument.ParseAsync(httpContext.Response.Body);
        Assert.Equal(
            "リクエスト回数の上限に達しました。時間をおいて再度お試しください。",
            document.RootElement.GetProperty("message").GetString());
    }

    [Fact]
    public async Task WriteAsync_WhenLeaseHasNoRetryAfterMetadata_SetsStatusAndBodyWithoutRetryAfterHeader()
    {
        var httpContext = new DefaultHttpContext { Response = { Body = new MemoryStream() } };
        var context = new OnRejectedContext { HttpContext = httpContext, Lease = new LeaseWithoutMetadata() };

        await RateLimitRejectionHandler.WriteAsync(context, CancellationToken.None);

        Assert.Equal(StatusCodes.Status429TooManyRequests, httpContext.Response.StatusCode);
        Assert.True(StringValues.IsNullOrEmpty(httpContext.Response.Headers.RetryAfter));

        httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
        using var document = await JsonDocument.ParseAsync(httpContext.Response.Body);
        Assert.Equal("RATE_LIMIT_EXCEEDED", document.RootElement.GetProperty("code").GetString());
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
