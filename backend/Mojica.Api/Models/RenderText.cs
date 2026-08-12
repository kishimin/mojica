namespace Mojica.Api.Models;

public sealed record RenderText
{
    private RenderText()
    {
    }

    public static bool TryCreate(
        string? value,
        out RenderText? renderText,
        out ModelValidationError? error)
    {
        if (value is null)
        {
            renderText = null;
            error = new ModelValidationError(
                "text",
                ModelValidationReason.Required);
            return false;
        }

        if (value.Length == 0)
        {
            renderText = null;
            error = new ModelValidationError(
                "text",
                ModelValidationReason.LengthOutOfRange);
            return false;
        }

        renderText = new RenderText();
        error = null;
        return true;
    }
}
