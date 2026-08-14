using Mojica.Api.Models;
using Mojica.Api.Ports;

namespace Mojica.Api.Services;

public sealed class ImageGenerationService(ImageGenerationPort port)
{
    public async Task<ImageGenerationServiceResult> GenerateAsync(
        ImageGenerationRequest request,
        CancellationToken cancellationToken)
    {
        var portResult = await port.GenerateAsync(request, cancellationToken);
        var imageData = portResult.Data!;

        return ImageGenerationServiceResult.Success(
            new GeneratedImage(imageData.Content, imageData.MediaType, string.Empty));
    }
}
