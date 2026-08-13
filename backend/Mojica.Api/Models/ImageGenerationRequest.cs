using System.Diagnostics.CodeAnalysis;

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
        ImageType type,
        RenderText text,
        PatternCharacter foregroundCharacter,
        HexColor foregroundColor,
        PatternCharacter backgroundCharacter,
        HexColor backgroundColor,
        [NotNullWhen(true)] out ImageGenerationRequest? request,
        [NotNullWhen(false)] out ModelValidationError? error)
    {
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
}
