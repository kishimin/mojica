namespace Mojica.Api.Models;

public sealed record ImageType
{
    public static ImageType Standard { get; } = new("standard");

    public static ImageType XBackground { get; } = new("x-background");

    public static ImageType XIcon { get; } = new("x-icon");

    private ImageType(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static bool TryCreate(
        string value,
        out ImageType? imageType,
        out ModelValidationError? error)
    {
        imageType = value switch
        {
            "standard" => Standard,
            "x-background" => XBackground,
            "x-icon" => XIcon,
            _ => null,
        };
        error = imageType is null
            ? new ModelValidationError("type", ModelValidationReason.UnsupportedImageType)
            : null;

        return imageType is not null;
    }
}
