using Mojica.Api.Models;

namespace Mojica.Api.Ports;

public interface ImageUploadPort
{
    Task<ImageGenerationPortResult> GenerateAsync(
        Stream image, ImageGenerationRequest options, CancellationToken cancellationToken);
}
