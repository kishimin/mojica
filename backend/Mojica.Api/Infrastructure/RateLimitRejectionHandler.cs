using System.Globalization;
using System.Text.Json;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using Mojica.Api.Localization;
using Mojica.Api.Mapping;
using Mojica.Api.Ports;

namespace Mojica.Api.Infrastructure;

public static class RateLimitRejectionHandler
{
    public static async ValueTask WriteAsync(OnRejectedContext context, CancellationToken cancellationToken)
    {
        var language = ApiLanguageSelector.Select(context.HttpContext.Request.Headers.AcceptLanguage.ToString());
        var portError = new ImageGenerationPortError(
            ImageGenerationPortErrorCode.RateLimited,
            retryAfter: TryGetRetryAfterSeconds(context.Lease));
        var result = ApiErrorMapper.MapPortFailure(portError, language);

        context.HttpContext.Response.StatusCode = result.StatusCode;
        context.HttpContext.Response.ContentType = "application/json";

        if (result.RetryAfter is { } retryAfterSeconds)
        {
            context.HttpContext.Response.Headers.RetryAfter =
                retryAfterSeconds.ToString(CultureInfo.InvariantCulture);
        }

        await JsonSerializer.SerializeAsync(
            context.HttpContext.Response.Body,
            result.Response,
            cancellationToken: cancellationToken);
    }

    private static int? TryGetRetryAfterSeconds(RateLimitLease lease)
    {
        if (!lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
        {
            return null;
        }

        return (int)Math.Ceiling(retryAfter.TotalSeconds);
    }
}
