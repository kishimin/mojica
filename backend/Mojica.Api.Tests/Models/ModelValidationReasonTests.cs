using Mojica.Api.Models;

namespace Mojica.Api.Tests.Models;

public sealed class ModelValidationReasonTests
{
    [Fact]
    public void ModelValidationReason_DocumentedReasons_ExposeExpectedValues()
    {
        Assert.Equal("CONTROL_CHARACTER", ModelValidationReason.ControlCharacter.Value);
        Assert.Equal("INVALID_HEX_COLOR", ModelValidationReason.InvalidHexColor.Value);
        Assert.Equal("LENGTH_OUT_OF_RANGE", ModelValidationReason.LengthOutOfRange.Value);
        Assert.Equal("NOT_BLANK", ModelValidationReason.NotBlank.Value);
        Assert.Equal("REQUIRED", ModelValidationReason.Required.Value);
        Assert.Equal("UNSUPPORTED_IMAGE_TYPE", ModelValidationReason.UnsupportedImageType.Value);
        Assert.Equal("VISIBLE_CHARACTER_REQUIRED", ModelValidationReason.VisibleCharacterRequired.Value);
    }
}
