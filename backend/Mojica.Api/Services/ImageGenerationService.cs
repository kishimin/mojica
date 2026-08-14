using Mojica.Api.Models;
using Mojica.Api.Ports;

namespace Mojica.Api.Services;

public sealed class ImageGenerationService
{
    private readonly ImageGenerationPort port;
    private readonly Guid uuid;

    public ImageGenerationService(ImageGenerationPort port)
        : this(port, new SystemUuidProvider())
    {
    }

    public ImageGenerationService(
        ImageGenerationPort port,
        UuidProvider uuidProvider)
    {
        this.port = port;
        uuid = uuidProvider.Create();
    }

    public async Task<ImageGenerationServiceResult> GenerateAsync(
        ImageGenerationRequest request,
        CancellationToken cancellationToken)
    {
        var portResult = await port.GenerateAsync(request, cancellationToken);
        var imageData = portResult.Data!;
        var fileName = $"mojica-{request.Type.Value}-{uuid}.png";

        return ImageGenerationServiceResult.Success(
            new GeneratedImage(imageData.Content, imageData.MediaType, fileName));
    }

    private sealed class SystemUuidProvider : UuidProvider
    {
        public Guid Create()
        {
            return Guid.NewGuid();
        }
    }
}
