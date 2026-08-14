using Mojica.Api.Models;
using Mojica.Api.Ports;

namespace Mojica.Api.Services;

public sealed record ImageGenerationServiceResult
{
    private ImageGenerationServiceResult(
        GeneratedImage? image,
        ImageGenerationPortError? error)
    {
        Image = image;
        Error = error;
    }

    public bool IsSuccess => Error is null;

    public GeneratedImage? Image { get; }

    public ImageGenerationPortError? Error { get; }

    internal static ImageGenerationServiceResult Success(GeneratedImage image)
    {
        ArgumentNullException.ThrowIfNull(image);
        return new ImageGenerationServiceResult(image, null);
    }

    internal static ImageGenerationServiceResult Failure(ImageGenerationPortError error)
    {
        ArgumentNullException.ThrowIfNull(error);
        return new ImageGenerationServiceResult(null, error);
    }
}
