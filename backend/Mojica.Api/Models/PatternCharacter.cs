using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Mojica.Api.Models;

public sealed record PatternCharacter
{
    private PatternCharacter(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static bool TryCreate(
        string? value,
        [NotNullWhen(true)] out PatternCharacter? patternCharacter,
        [NotNullWhen(false)] out ModelValidationReason? reason)
    {
        if (value is null)
        {
            patternCharacter = null;
            reason = ModelValidationReason.Required;
            return false;
        }

        var characterCount = StringInfo.ParseCombiningCharacters(value).Length;

        if (characterCount is 0 or > 128)
        {
            patternCharacter = null;
            reason = ModelValidationReason.LengthOutOfRange;
            return false;
        }

        if (value.Any(char.IsControl))
        {
            patternCharacter = null;
            reason = ModelValidationReason.ControlCharacter;
            return false;
        }

        patternCharacter = new PatternCharacter(value);
        reason = null;
        return true;
    }
}
