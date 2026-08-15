namespace Mojica.Api.Infrastructure;

public sealed record GlyphForgeRequest(
    string FrameText,
    string InnerText,
    string OuterText,
    int[] InnerColor,
    int[] OuterColor);
