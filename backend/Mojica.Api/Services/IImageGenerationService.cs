using Mojica.Api.Models;

namespace Mojica.Api.Services;

public interface IImageGenerationService
{
    Task<ImageGenerationServiceResult> GenerateAsync(
        ImageGenerationRequest request,
        CancellationToken cancellationToken);
}
