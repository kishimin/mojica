using Mojica.Api.Models;

namespace Mojica.Api.Infrastructure;

public static class GlyphForgeRequestMapper
{
    public static (string Path, GlyphForgeRequest Payload) Map(ImageGenerationRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var path = request.Type.Value switch
        {
            ImageType.StandardValue => "/images",
            ImageType.XBackgroundValue => "/images/background",
            ImageType.XIconValue => "/images/x-icon",
            _ => throw new InvalidOperationException("Validated image type is not supported."),
        };

        var foregroundColor = request.ForegroundColor.ToRgb();
        var backgroundColor = request.BackgroundColor.ToRgb();
        var payload = new GlyphForgeRequest(
            request.Text.Value,
            request.ForegroundCharacter.Value,
            request.BackgroundCharacter.Value,
            foregroundColor.ToArray(),
            backgroundColor.ToArray());

        return (path, payload);
    }
}
