using System.Diagnostics.CodeAnalysis;

namespace Mojica.Api.Models;

public sealed record HexColor
{
    private HexColor(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static bool TryCreate(
        string? value,
        [NotNullWhen(true)] out HexColor? color,
        [NotNullWhen(false)] out ModelValidationReason? reason)
    {
        color = new HexColor(value!.ToUpperInvariant());
        reason = null;
        return true;
    }

    public override string ToString() => Value;
}
