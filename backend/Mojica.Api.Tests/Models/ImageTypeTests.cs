using Mojica.Api.Models;

namespace Mojica.Api.Tests.Models;

public sealed class ImageTypeTests
{
    [Theory]
    [InlineData("standard")]
    [InlineData("x-background")]
    [InlineData("x-icon")]
    public void ImageType_Create_WhenValueIsSupported_ReturnsDefinedImageType(string value)
    {
        if (!ImageType.TryCreate(value, out var imageType, out var error))
        {
            Assert.Fail("A supported image type should be created.");
        }

        Assert.Equal(value, imageType.Value);
        Assert.Null(error);
    }

    [Fact]
    public void ImageType_Create_WhenValueIsUndefined_ReturnsUnsupportedImageTypeError()
    {
        if (ImageType.TryCreate("unsupported", out var imageType, out var error))
        {
            Assert.Fail("An unsupported image type should not be created.");
        }

        Assert.Null(imageType);
        Assert.Equal("UNSUPPORTED_IMAGE_TYPE", error.Code);
        Assert.Equal("type", error.Target);
        Assert.Equal(ModelValidationReason.UnsupportedImageType, error.Reason);
    }
}
