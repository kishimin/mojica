using Mojica.Api.Models;
using Mojica.Api.Ports;

namespace Mojica.Api.Services;

public sealed class ImageUploadService(ImageUploadPort port)
{
    public async Task<ImageGenerationServiceResult> GenerateAsync(
        Stream image, ImageGenerationRequest options, CancellationToken cancellationToken)
    {
        var result = await port.GenerateAsync(image, options, cancellationToken);
        if (!result.IsSuccess)
        {
            return ImageGenerationServiceResult.Failure(result.Error!);
        }

        return ImageGenerationServiceResult.Success(new GeneratedImage(
            result.Data!.Content, result.Data.MediaType, $"mojica-standard-{Guid.NewGuid()}.png"));
    }
}
