namespace Mojica.Api.Tests.Models;

public sealed class ImageTypeTests
{
    [Fact(Skip = "TODO: Implement from documented test plan.")]
    public void ImageType_Create_WhenValueIsSupported_ReturnsDefinedImageType()
    {
        // ID: IMGTYPE-01
        // Source: docs/v1/api/models.md §4 ImageType.
        // Given: each supported value "standard", "x-background", and "x-icon" (Theory candidate)
        // When: ImageType creation is requested
        // Then: creation succeeds and preserves the corresponding predefined value
        // Error: none
        // Priority: High
    }

    [Fact(Skip = "TODO: Implement from documented test plan.")]
    public void ImageType_Create_WhenValueIsUndefined_ReturnsUnsupportedImageTypeError()
    {
        // ID: IMGTYPE-02
        // Source: docs/v1/api/models.md §4 ImageType.
        // Given: an arbitrary value that is not one of the three supported values
        // When: ImageType creation is requested
        // Then: creation fails with code UNSUPPORTED_IMAGE_TYPE, target type, and a closed ModelValidationReason
        // Priority: High
    }
}
