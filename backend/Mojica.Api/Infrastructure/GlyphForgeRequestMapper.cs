using Mojica.Api.Models;

namespace Mojica.Api.Infrastructure;

public static class GlyphForgeRequestMapper
{
    public static GlyphForgeRequestMapping Map(ImageGenerationRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var path = request.Type switch
        {
            ImageType type when type == ImageType.Standard => "/images",
            ImageType type when type == ImageType.XBackground => "/images/background",
            ImageType type when type == ImageType.XIcon => "/images/x-icon",
            _ => throw new InvalidOperationException("Validated image type is not supported."),
        };

        var foregroundColor = request.ForegroundColor.ToRgb();
        var backgroundColor = request.BackgroundColor.ToRgb();
        var payload = new GlyphForgeRequest(
            request.Text.Value,
            request.ForegroundCharacter.Value,
            request.BackgroundCharacter.Value,
            [foregroundColor.Red, foregroundColor.Green, foregroundColor.Blue],
            [backgroundColor.Red, backgroundColor.Green, backgroundColor.Blue]);

        return new GlyphForgeRequestMapping(path, payload);
    }
}
