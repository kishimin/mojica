using Mojica.Api.Models;
using Mojica.Api.Ports;

namespace Mojica.Api.Services;

public sealed class ImageGenerationService(ImageGenerationPort port)
{
    public async Task<ImageGenerationServiceResult> GenerateAsync(
        ImageGenerationRequest request,
        CancellationToken cancellationToken)
    {
        await port.GenerateAsync(request, cancellationToken);

        return ImageGenerationServiceResult.Success(
            new GeneratedImage([], string.Empty, string.Empty));
    }
}
