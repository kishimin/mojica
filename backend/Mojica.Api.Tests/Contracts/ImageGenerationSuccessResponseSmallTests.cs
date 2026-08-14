using Mojica.Api.Contracts;

namespace Mojica.Api.Tests.Contracts;

public sealed class ImageGenerationSuccessResponseSmallTests
{
    public static TheoryData<byte[]?, string?, string?> MissingValues => new()
    {
        { null, "image/png", "mojica-standard-123.png" },
        { [], null, "mojica-standard-123.png" },
        { [], "image/png", null },
    };

    [Fact]
    public void Create_WhenGeneratedImageIsValid_RetainsContentMediaTypeAndFileName()
    {
        byte[] content = [0x89, 0x50, 0x4E, 0x47];

        var response = new ImageGenerationSuccessResponse(
            content,
            "image/png",
            "mojica-standard-123.png");

        Assert.Same(content, response.Content);
        Assert.Equal("image/png", response.MediaType);
        Assert.Equal("mojica-standard-123.png", response.FileName);
    }

    [Theory]
    [MemberData(nameof(MissingValues))]
    public void Create_WhenRequiredValueIsNull_ThrowsArgumentNullException(
        byte[]? content,
        string? mediaType,
        string? fileName)
    {
        Assert.Throws<ArgumentNullException>(() =>
            new ImageGenerationSuccessResponse(content!, mediaType!, fileName!));
    }
}
