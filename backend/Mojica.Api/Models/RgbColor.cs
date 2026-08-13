using System.Diagnostics.CodeAnalysis;

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
        color = new RgbColor(red, green, blue);
        error = null;
        return true;
    }
}
