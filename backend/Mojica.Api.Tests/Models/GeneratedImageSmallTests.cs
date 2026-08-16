using Mojica.Api.Models;

namespace Mojica.Api.Tests.Models;

public sealed class GeneratedImageSmallTests
{
    [Fact]
    public void GeneratedImage_Create_WhenContentIsNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new GeneratedImage(null!, "image/png", "generated.png"));
    }

    [Fact]
    public void GeneratedImage_Create_WhenMediaTypeIsNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new GeneratedImage([], null!, "generated.png"));
    }

    [Fact]
    public void GeneratedImage_Create_WhenFileNameIsNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new GeneratedImage([], "image/png", null!));
    }

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

    [Fact]
    public void GeneratedImage_Equality_WhenContentValuesMatch_UsesByteContents()
    {
        var first = new GeneratedImage(
            [0x89, 0x50, 0x4E, 0x47],
            "image/png",
            "generated.png");
        var second = new GeneratedImage(
            [0x89, 0x50, 0x4E, 0x47],
            "image/png",
            "generated.png");

        Assert.Equal(first, second);
        Assert.Equal(first.GetHashCode(), second.GetHashCode());
    }

    [Fact]
    public void GeneratedImage_Equality_WhenContentValuesDiffer_IsNotEqual()
    {
        var first = new GeneratedImage(
            [0x89, 0x50, 0x4E, 0x47],
            "image/png",
            "generated.png");
        var second = new GeneratedImage(
            [0x89, 0x50, 0x4E, 0x46],
            "image/png",
            "generated.png");

        Assert.NotEqual(first, second);
    }

    [Fact]
    public void GeneratedImage_GetHashCode_WhenFileNameDiffers_ReturnsDifferentStableHashCode()
    {
        var first = new GeneratedImage(
            [0x89, 0x50, 0x4E, 0x47],
            "image/png",
            "generated-1.png");
        var second = new GeneratedImage(
            [0x89, 0x50, 0x4E, 0x47],
            "image/png",
            "generated-2.png");

        Assert.NotEqual(first.GetHashCode(), second.GetHashCode());
    }
}
