using Mojica.Api.Models;

namespace Mojica.Api.Tests.Models;

public sealed class GeneratedImageTests
{
    [Fact]
    public void GeneratedImage_Create_WhenGenerationSucceeds_PreservesResultData()
    {
        byte[] content = [0x89, 0x50, 0x4E, 0x47];
        const string mediaType = "image/png";
        const string fileName = "generated.png";

        var image = new GeneratedImage(content, mediaType, fileName);

        Assert.Same(content, image.Content);
        Assert.Equal(mediaType, image.MediaType);
        Assert.Equal(fileName, image.FileName);
    }
}
