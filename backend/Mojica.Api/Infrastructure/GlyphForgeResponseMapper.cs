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

        if (response.Failure is GlyphForgeResponseFailure.Failed)
        {
            return Failure(ImageGenerationPortErrorCode.Failed);
        }

        if (response.StatusCode is not { } statusCode)
        {
            return Failure(ImageGenerationPortErrorCode.InvalidResponse);
        }

        return ((int)statusCode) switch
        {
            (int)HttpStatusCode.TooManyRequests =>
                Failure(ImageGenerationPortErrorCode.RateLimited, response.RetryAfter),
            (int)HttpStatusCode.UnprocessableEntity =>
                Failure(ImageGenerationPortErrorCode.OutputSizeExceeded),
            (int)HttpStatusCode.ServiceUnavailable =>
                Failure(ImageGenerationPortErrorCode.Unavailable, response.RetryAfter),
            >= 500 and <= 599 => Failure(ImageGenerationPortErrorCode.Failed),
            (int)HttpStatusCode.OK => MapSuccessfulResponse(response),
            _ => Failure(ImageGenerationPortErrorCode.InvalidResponse),
        };
    }

    private static ImageGenerationPortResult MapSuccessfulResponse(GlyphForgeResponse response)
    {
        if (response.MediaType is { } mediaType &&
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
