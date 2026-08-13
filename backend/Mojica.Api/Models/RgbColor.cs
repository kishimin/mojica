using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Mojica.Api.Models;

public sealed record RgbColor
{
    private RgbColor(int red, int green, int blue)
    {
        Red = red;
        Green = green;
        Blue = blue;
    }

    public int Red { get; }

    public int Green { get; }

    public int Blue { get; }

    public static bool TryCreate(
        int red,
        int green,
        int blue,
        [NotNullWhen(true)] out RgbColor? color,
        [NotNullWhen(false)] out ModelValidationError? error)
    {
        if (TryGetBelowMinimumError("red", red, out error)
            || TryGetBelowMinimumError("green", green, out error)
            || TryGetBelowMinimumError("blue", blue, out error))
        {
            color = null;
            return false;
        }

        color = new RgbColor(red, green, blue);
        error = null;
        return true;
    }

    private static bool TryGetBelowMinimumError(
        string target,
        int value,
        [NotNullWhen(true)] out ModelValidationError? error)
    {
        if (value >= 0)
        {
            error = null;
            return false;
        }

        error = new ModelValidationError(
            target,
            ModelValidationReason.ValueOutOfRange,
            new Dictionary<string, string>
            {
                ["minimum"] = "0",
                ["maximum"] = "255",
                ["actual"] = value.ToString(CultureInfo.InvariantCulture),
            });
        return true;
    }
}
