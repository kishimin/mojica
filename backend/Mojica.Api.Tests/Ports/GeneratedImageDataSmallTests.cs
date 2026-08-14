using Mojica.Api.Ports;

namespace Mojica.Api.Tests.Ports;

public sealed class GeneratedImageDataSmallTests
{
    [Fact]
    public void GeneratedImageData_Create_WhenResultIsValid_PreservesContentAndMediaType()
    {
        byte[] content = [0x89, 0x50, 0x4E, 0x47];
        const string mediaType = "image/png";

        var imageData = new GeneratedImageData(content, mediaType);

        Assert.Same(content, imageData.Content);
        Assert.Equal(mediaType, imageData.MediaType);
    }
}
