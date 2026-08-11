using Mojica.Api.Models;

namespace Mojica.Api.Tests.Models;

public sealed class ModelValidationReasonTests
{
    [Fact(Skip = "TODO: Implement from documented test plan.")]
    public void ModelValidationReason_WhenArbitraryValueIsRequested_CannotRepresentUndefinedReason()
    {
        // ID: ERROR-02
        // Source: docs/v1/api/models.md §11 ModelValidationReason.
        // Given: a value outside the closed set of ModelValidationReason values
        // When: the domain attempts to represent the reason
        // Then: an undefined reason cannot be created
        // Priority: Medium
    }

    [Fact]
    public void ModelValidationReason_DocumentedReasons_ExposeExpectedValues()
    {
        Assert.Equal("CONTROL_CHARACTER", ModelValidationReason.ControlCharacter.Value);
        Assert.Equal("INVALID_HEX_COLOR", ModelValidationReason.InvalidHexColor.Value);
        Assert.Equal("LENGTH_OUT_OF_RANGE", ModelValidationReason.LengthOutOfRange.Value);
        Assert.Equal("REQUIRED", ModelValidationReason.Required.Value);
        Assert.Equal("UNSUPPORTED_IMAGE_TYPE", ModelValidationReason.UnsupportedImageType.Value);
        Assert.Equal("VISIBLE_CHARACTER_REQUIRED", ModelValidationReason.VisibleCharacterRequired.Value);
    }
}
