using System.Diagnostics.CodeAnalysis;

namespace Mojica.Api.Models;

public sealed record RenderText
{
    private RenderText(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static bool TryCreate(
        string? value,
        [NotNullWhen(true)] out RenderText? renderText,
        [NotNullWhen(false)] out ModelValidationError? error)
    {
        if (value is null)
        {
            renderText = null;
            error = new ModelValidationError(
                "text",
                ModelValidationReason.Required);
            return false;
        }

        var reason = TextValueValidation.GetFailureReason(
            value,
            maximumGraphemes: 64,
            rejectWhitespaceOnly: true);

        if (reason is not null)
        {
            renderText = null;
            error = new ModelValidationError(
                "text",
                reason);
            return false;
        }

        renderText = new RenderText(value);
        error = null;
        return true;
    }
}
