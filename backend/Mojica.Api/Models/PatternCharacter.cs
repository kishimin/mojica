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
        patternCharacter = null;
        error = new ModelValidationError(
            target,
            ModelValidationReason.Required);
        return false;
    }
}
