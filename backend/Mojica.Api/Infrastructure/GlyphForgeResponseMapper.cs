using System.Net;
using Mojica.Api.Ports;

namespace Mojica.Api.Infrastructure;

public static class GlyphForgeResponseMapper
{
    private static readonly HashSet<string> SupportedMediaTypes =
        new(StringComparer.OrdinalIgnoreCase)
        {
            "image/png",
        };

    public static ImageGenerationPortResult Map(GlyphForgeResponse response)
    {
        ArgumentNullException.ThrowIfNull(response);

        if (response.Failure is GlyphForgeResponseFailure.Timeout)
        {
            return Failure(ImageGenerationPortErrorCode.Timeout);
        }

        if (response.Failure is GlyphForgeResponseFailure.Communication)
        {
            return Failure(ImageGenerationPortErrorCode.Unavailable);
        }

        if (response.StatusCode is HttpStatusCode.TooManyRequests)
        {
            return Failure(ImageGenerationPortErrorCode.RateLimited, response.RetryAfter);
        }

        if (response.StatusCode is HttpStatusCode.UnprocessableEntity)
        {
            return Failure(ImageGenerationPortErrorCode.OutputSizeExceeded);
        }

        if (response.StatusCode is HttpStatusCode.ServiceUnavailable)
        {
            return Failure(ImageGenerationPortErrorCode.Unavailable, response.RetryAfter);
        }

        if (response.StatusCode is >= HttpStatusCode.InternalServerError and < HttpStatusCode.NetworkAuthenticationRequired)
        {
            return Failure(ImageGenerationPortErrorCode.Failed);
        }

        if (response.StatusCode is >= HttpStatusCode.OK and < HttpStatusCode.MultipleChoices &&
            response.MediaType is { } mediaType &&
            SupportedMediaTypes.Contains(mediaType) &&
            response.Content is { Length: > 0 } content)
        {
            return ImageGenerationPortResult.Success(
                new GeneratedImageData(content, mediaType));
        }

        return Failure(ImageGenerationPortErrorCode.InvalidResponse);
    }

    private static ImageGenerationPortResult Failure(
        ImageGenerationPortErrorCode errorCode,
        int? retryAfter = null)
    {
        return ImageGenerationPortResult.Failure(
            new ImageGenerationPortError(errorCode, retryAfter));
    }
}
