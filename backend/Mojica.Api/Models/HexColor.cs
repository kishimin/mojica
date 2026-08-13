using System.Diagnostics.CodeAnalysis;

namespace Mojica.Api.Models;

public sealed record HexColor
{
    private HexColor(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public RgbColor ToRgb()
    {
        var red = Convert.ToInt32(Value.Substring(1, 2), 16);
        var green = Convert.ToInt32(Value.Substring(3, 2), 16);
        var blue = Convert.ToInt32(Value.Substring(5, 2), 16);

        if (!RgbColor.TryCreate(red, green, blue, out var color, out _))
        {
            throw new InvalidOperationException("Validated HEX components must be valid RGB values.");
        }

        return color;
    }

    public static bool TryCreate(
        string? value,
        [NotNullWhen(true)] out HexColor? color,
        [NotNullWhen(false)] out ModelValidationReason? reason)
    {
        if (value is null)
        {
            color = null;
            reason = ModelValidationReason.Required;
            return false;
        }

        if (value.Length != 7
            || value[0] != '#'
            || !value[1..].All(char.IsAsciiHexDigit))
        {
            color = null;
            reason = ModelValidationReason.InvalidHexColor;
            return false;
        }

        color = new HexColor(value.ToUpperInvariant());
        reason = null;
        return true;
    }

    public override string ToString() => Value;
}
