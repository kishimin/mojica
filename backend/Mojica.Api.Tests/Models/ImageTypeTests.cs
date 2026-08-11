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
        // ID: IMGTYPE-01
        // Source: docs/v1/api/models.md §4 ImageType.
        // Given: each supported value "standard", "x-background", and "x-icon" (Theory candidate)
        // When: ImageType creation is requested
        // Then: creation succeeds and preserves the corresponding predefined value
        // Error: none
        // Priority: High
        var succeeded = ImageType.TryCreate(value, out var imageType, out var error);

        Assert.True(succeeded);
        Assert.NotNull(imageType);
        Assert.Equal(value, imageType.Value);
        Assert.Null(error);
    }

    [Fact]
    public void ImageType_Create_WhenValueIsUndefined_ReturnsUnsupportedImageTypeError()
    {
        // ID: IMGTYPE-02
        // Source: docs/v1/api/models.md §4 ImageType.
        // Given: an arbitrary value that is not one of the three supported values
        // When: ImageType creation is requested
        // Then: creation fails with code UNSUPPORTED_IMAGE_TYPE, target type, and a closed ModelValidationReason
        // Error: UNSUPPORTED_IMAGE_TYPE targeting type
        // Priority: High
        var succeeded = ImageType.TryCreate("unsupported", out var imageType, out var error);

        Assert.False(succeeded);
        Assert.Null(imageType);
        Assert.NotNull(error);
        Assert.Equal("UNSUPPORTED_IMAGE_TYPE", error.Code);
        Assert.Equal("type", error.Target);
        Assert.Equal(ModelValidationReason.UnsupportedImageType, error.Reason);
    }
}
