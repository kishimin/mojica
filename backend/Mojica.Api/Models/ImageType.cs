using System.Diagnostics.CodeAnalysis;

namespace Mojica.Api.Models;

public sealed record ImageType
{
    public const string StandardValue = "standard";

    public const string XBackgroundValue = "x-background";

    public const string XIconValue = "x-icon";

    public static ImageType Standard { get; } = new(StandardValue);

    public static ImageType XBackground { get; } = new(XBackgroundValue);

    public static ImageType XIcon { get; } = new(XIconValue);

    private ImageType(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static bool TryCreate(
        string? value,
        [NotNullWhen(true)] out ImageType? imageType,
        [NotNullWhen(false)] out ModelValidationError? error)
    {
        if (value is null)
        {
            imageType = null;
            error = new ModelValidationError("type", ModelValidationReason.Required);
            return false;
        }

        imageType = value switch
        {
            StandardValue => Standard,
            XBackgroundValue => XBackground,
            XIconValue => XIcon,
            _ => null,
        };
        error = imageType is null
            ? new ModelValidationError("type", ModelValidationReason.UnsupportedImageType)
            : null;

        return imageType is not null;
    }
}
