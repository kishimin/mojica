namespace Mojica.Api.Models;

public sealed record ModelValidationReason
{
    public static ModelValidationReason ControlCharacter { get; } = new("CONTROL_CHARACTER");

    public static ModelValidationReason InvalidHexColor { get; } = new("INVALID_HEX_COLOR");

    public static ModelValidationReason LengthOutOfRange { get; } = new("LENGTH_OUT_OF_RANGE");

    public static ModelValidationReason NotBlank { get; } = new("NOT_BLANK");

    public static ModelValidationReason Required { get; } = new("REQUIRED");

    public static ModelValidationReason UnsupportedImageType { get; } = new("UNSUPPORTED_IMAGE_TYPE");

    public static ModelValidationReason VisibleCharacterRequired { get; } = new("VISIBLE_CHARACTER_REQUIRED");

    private ModelValidationReason(string value)
    {
        Value = value;
    }

    public string Value { get; }
}
