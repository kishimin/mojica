using Mojica.Api.Models;

namespace Mojica.Api.Tests.Models;

public sealed class GeneratedImageTests
{
    [Fact]
    public void GeneratedImage_Create_WhenGenerationSucceeds_PreservesResultData()
    {
        // ID: GENERATED-01
        // Source: docs/v1/api/models.md §10 GeneratedImage.
        // Given: binary image content, an image media type, and a download filename
        // When: GeneratedImage is created
        // Then: it exposes the same content, mediaType, and fileName
        // Priority: Medium
        byte[] content = [0x89, 0x50, 0x4E, 0x47];
        const string mediaType = "image/png";
        const string fileName = "generated.png";

        var image = new GeneratedImage(content, mediaType, fileName);

        Assert.Same(content, image.Content);
        Assert.Equal(mediaType, image.MediaType);
        Assert.Equal(fileName, image.FileName);
    }
}
