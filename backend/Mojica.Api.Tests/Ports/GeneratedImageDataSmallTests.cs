using Mojica.Api.Ports;

namespace Mojica.Api.Tests.Ports;

public sealed class GeneratedImageDataSmallTests
{
    [Fact]
    public void GeneratedImageData_Create_WhenContentIsNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new GeneratedImageData(null!, "image/png"));
    }

    [Fact]
    public void GeneratedImageData_Create_WhenMediaTypeIsNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new GeneratedImageData([], null!));
    }

    [Fact]
    public void GeneratedImageData_Create_WhenResultIsValid_PreservesContentAndMediaType()
    {
        byte[] content = [0x89, 0x50, 0x4E, 0x47];
        const string mediaType = "image/png";

        var imageData = new GeneratedImageData(content, mediaType);

        Assert.Same(content, imageData.Content);
        Assert.Equal(mediaType, imageData.MediaType);
    }

    [Fact]
    public void GeneratedImageData_Equality_WhenContentValuesMatch_UsesByteContents()
    {
        var first = new GeneratedImageData(
            [0x89, 0x50, 0x4E, 0x47],
            "image/png");
        var second = new GeneratedImageData(
            [0x89, 0x50, 0x4E, 0x47],
            "image/png");

        Assert.Equal(first, second);
        Assert.Equal(first.GetHashCode(), second.GetHashCode());
    }
}
