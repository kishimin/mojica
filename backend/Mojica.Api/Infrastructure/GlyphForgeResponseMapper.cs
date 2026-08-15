using System.Net;
using Mojica.Api.Ports;

namespace Mojica.Api.Infrastructure;

public static class GlyphForgeResponseMapper
{
    public static ImageGenerationPortResult Map(GlyphForgeResponse response)
    {
        ArgumentNullException.ThrowIfNull(response);

        if (response.StatusCode is >= HttpStatusCode.OK and < HttpStatusCode.MultipleChoices &&
            response.MediaType is { } mediaType &&
            string.Equals(mediaType, "image/png", StringComparison.OrdinalIgnoreCase) &&
            response.Content is { Length: > 0 } content)
        {
            return ImageGenerationPortResult.Success(
                new GeneratedImageData(content, mediaType));
        }

        return ImageGenerationPortResult.Failure(
            new ImageGenerationPortError(ImageGenerationPortErrorCode.InvalidResponse));
    }
}
