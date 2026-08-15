using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;

namespace Mojica.Api.Models;

public sealed record ImageGenerationRequest
{
    private ImageGenerationRequest(
        ImageType type,
        RenderText text,
        PatternCharacter foregroundCharacter,
        HexColor foregroundColor,
        PatternCharacter backgroundCharacter,
        HexColor backgroundColor)
    {
        Type = type;
        Text = text;
        ForegroundCharacter = foregroundCharacter;
        ForegroundColor = foregroundColor;
        BackgroundCharacter = backgroundCharacter;
        BackgroundColor = backgroundColor;
    }

    public ImageType Type { get; }

    public RenderText Text { get; }

    public PatternCharacter ForegroundCharacter { get; }

    public HexColor ForegroundColor { get; }

    public PatternCharacter BackgroundCharacter { get; }

    public HexColor BackgroundColor { get; }

    public static bool TryCreate(
        ImageType? type,
        RenderText? text,
        PatternCharacter? foregroundCharacter,
        HexColor? foregroundColor,
        PatternCharacter? backgroundCharacter,
        HexColor? backgroundColor,
        [NotNullWhen(true)] out ImageGenerationRequest? request,
        [NotNullWhen(false)] out ModelValidationError? error)
    {
        if (type is null)
        {
            return FailRequired("type", out request, out error);
        }

        if (text is null)
        {
            return FailRequired("text", out request, out error);
        }

        if (foregroundCharacter is null)
        {
            return FailRequired("foregroundCharacter", out request, out error);
        }

        if (foregroundColor is null)
        {
            return FailRequired("foregroundColor", out request, out error);
        }

        if (backgroundCharacter is null)
        {
            return FailRequired("backgroundCharacter", out request, out error);
        }

        if (backgroundColor is null)
        {
            return FailRequired("backgroundColor", out request, out error);
        }

        var patternError = GetPatternCombinationFailure(
            foregroundCharacter,
            backgroundCharacter);
        if (patternError is not null)
        {
            request = null;
            error = patternError;
            return false;
        }

        request = new ImageGenerationRequest(
            type,
            text,
            foregroundCharacter,
            foregroundColor,
            backgroundCharacter,
            backgroundColor);
        error = null;
        return true;
    }

    internal static ModelValidationError? GetPatternCombinationFailure(
        PatternCharacter foregroundCharacter,
        PatternCharacter backgroundCharacter)
    {
        var reason = ContainsVisibleCharacter(foregroundCharacter.Value)
            || ContainsVisibleCharacter(backgroundCharacter.Value)
            ? null
            : ModelValidationReason.VisibleCharacterRequired;

        return reason is null
            ? null
            : new ModelValidationError(
                ["foregroundCharacter", "backgroundCharacter"],
                reason);
    }

    private static bool ContainsVisibleCharacter(string value)
    {
        return value.EnumerateRunes().Any(rune =>
            !Rune.IsWhiteSpace(rune)
            && Rune.GetUnicodeCategory(rune) != UnicodeCategory.Format);
    }

    private static bool FailRequired(
        string target,
        out ImageGenerationRequest? request,
        out ModelValidationError error)
    {
        request = null;
        error = new ModelValidationError(target, ModelValidationReason.Required);
        return false;
    }
}
