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
        string target,
        out PatternCharacter? patternCharacter,
        out ModelValidationError? error)
    {
        if (value is null)
        {
            patternCharacter = null;
            error = new ModelValidationError(
                target,
                ModelValidationReason.Required);
            return false;
        }

        var characterCount = StringInfo.ParseCombiningCharacters(value).Length;

        if (characterCount is 0 or > 128)
        {
            patternCharacter = null;
            error = new ModelValidationError(
                target,
                ModelValidationReason.LengthOutOfRange);
            return false;
        }

        if (value.Any(char.IsControl))
        {
            patternCharacter = null;
            error = new ModelValidationError(
                target,
                ModelValidationReason.ControlCharacter);
            return false;
        }

        patternCharacter = new PatternCharacter(value);
        error = null;
        return true;
    }
}
