using Mojica.Api.Models;

namespace Mojica.Api.Ports;

public interface ImageGenerationPort
{
    Task<ImageGenerationPortResult> GenerateAsync(
        ImageGenerationRequest request,
        CancellationToken cancellationToken);
}
