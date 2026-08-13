using System.Diagnostics.CodeAnalysis;

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
