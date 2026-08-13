using System.Diagnostics.CodeAnalysis;

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

        reason = TextValueValidation.GetFailureReason(
            value,
            maximumGraphemes: 128,
            rejectWhitespaceOnly: false);

        if (reason is not null)
        {
            patternCharacter = null;
            return false;
        }

        patternCharacter = new PatternCharacter(value);
        reason = null;
        return true;
    }
}
