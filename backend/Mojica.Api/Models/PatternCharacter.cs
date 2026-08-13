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
        var characterCount = value is null
            ? 0
            : StringInfo.ParseCombiningCharacters(value).Length;

        if (value is not null && characterCount is 0 or > 128)
        {
            patternCharacter = null;
            error = new ModelValidationError(
                target,
                ModelValidationReason.LengthOutOfRange);
            return false;
        }

        if (value is not null)
        {
            patternCharacter = new PatternCharacter(value);
            error = null;
            return true;
        }

        patternCharacter = null;
        error = new ModelValidationError(
            target,
            ModelValidationReason.Required);
        return false;
    }
}
