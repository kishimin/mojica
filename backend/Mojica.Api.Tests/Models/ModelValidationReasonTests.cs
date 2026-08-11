using Mojica.Api.Models;

namespace Mojica.Api.Tests.Models;

public sealed class ModelValidationReasonTests
{
    [Fact]
    public void ModelValidationReason_WhenReasonsAreInspected_ExposesDocumentedReasons()
    {
        // ID: ERROR-02
        // Source: docs/v1/api/models.md §11 ModelValidationReason.
        // Given: the documented set of ModelValidationReason values
        // When: the public reasons are inspected
        // Then: every documented machine-readable reason exposes its expected value
        // Priority: Medium
        var expectedValues = new[]
        {
            "CONTROL_CHARACTER",
            "INVALID_HEX_COLOR",
            "LENGTH_OUT_OF_RANGE",
            "REQUIRED",
            "UNSUPPORTED_IMAGE_TYPE",
            "VISIBLE_CHARACTER_REQUIRED",
        };

        var actualValues = new[]
        {
            ModelValidationReason.ControlCharacter.Value,
            ModelValidationReason.InvalidHexColor.Value,
            ModelValidationReason.LengthOutOfRange.Value,
            ModelValidationReason.Required.Value,
            ModelValidationReason.UnsupportedImageType.Value,
            ModelValidationReason.VisibleCharacterRequired.Value,
        };

        Assert.Equal(expectedValues, actualValues);
    }
}
