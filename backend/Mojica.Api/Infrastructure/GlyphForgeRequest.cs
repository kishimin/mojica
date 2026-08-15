using System.Text.Json.Serialization;
using Mojica.Api.Models;

namespace Mojica.Api.Infrastructure;

public sealed record GlyphForgeRequest
{
    public GlyphForgeRequest(
        string frameText,
        string innerText,
        string outerText,
        int[] innerColor,
        int[] outerColor)
    {
        ArgumentNullException.ThrowIfNull(frameText);
        ArgumentNullException.ThrowIfNull(innerText);
        ArgumentNullException.ThrowIfNull(outerText);
        ArgumentNullException.ThrowIfNull(innerColor);
        ArgumentNullException.ThrowIfNull(outerColor);

        FrameText = frameText;
        InnerText = innerText;
        OuterText = outerText;
        this.innerColor = [.. innerColor];
        this.outerColor = [.. outerColor];
    }

    [JsonPropertyName("frame_text")]
    public string FrameText { get; }

    [JsonPropertyName("inner_text")]
    public string InnerText { get; }

    [JsonPropertyName("outer_text")]
    public string OuterText { get; }

    private readonly int[] innerColor;

    [JsonPropertyName("inner_color")]
    public int[] InnerColor => [.. innerColor];

    private readonly int[] outerColor;

    [JsonPropertyName("outer_color")]
    public int[] OuterColor => [.. outerColor];

    public bool Equals(GlyphForgeRequest? other)
    {
        return other is not null
            && FrameText == other.FrameText
            && InnerText == other.InnerText
            && OuterText == other.OuterText
            && ValueEquality.ContentEquals(innerColor, other.innerColor)
            && ValueEquality.ContentEquals(outerColor, other.outerColor);
    }

    public override int GetHashCode()
    {
        return ValueEquality.GetStableHashCode(FrameText, InnerText, OuterText);
    }
}
