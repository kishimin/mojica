using Mojica.Api.Models;

namespace Mojica.Api.Tests.Models;

public sealed class ImageTypeTests
{
    [Fact]
    public void ImageType_Create_WhenInputIsMissing_ReturnsRequiredError()
    {
        var succeeded = ImageType.TryCreate(null, out var imageType, out var error);

        Assert.False(succeeded);
        Assert.Null(imageType);
        Assert.NotNull(error);
        Assert.Equal("REQUIRED", error.Code);
        Assert.Equal("type", error.Target);
        Assert.Equal(ModelValidationReason.Required, error.Reason);
    }

    [Theory]
    [InlineData("standard")]
    [InlineData("x-background")]
    [InlineData("x-icon")]
    public void ImageType_Create_WhenValueIsSupported_ReturnsDefinedImageType(string value)
    {
        var succeeded = ImageType.TryCreate(value, out var imageType, out var error);

        Assert.True(succeeded);
        Assert.NotNull(imageType);
        Assert.Equal(value, imageType.Value);
        Assert.Null(error);
    }

    [Fact]
    public void ImageType_Create_WhenValueIsUndefined_ReturnsUnsupportedImageTypeError()
    {
        var succeeded = ImageType.TryCreate("unsupported", out var imageType, out var error);

        Assert.False(succeeded);
        Assert.Null(imageType);
        Assert.NotNull(error);
        Assert.Equal("UNSUPPORTED_IMAGE_TYPE", error.Code);
        Assert.Equal("type", error.Target);
        Assert.Equal(ModelValidationReason.UnsupportedImageType, error.Reason);
    }
}
