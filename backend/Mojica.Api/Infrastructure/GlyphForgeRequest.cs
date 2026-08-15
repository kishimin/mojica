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
        FrameText = frameText;
        InnerText = innerText;
        OuterText = outerText;
        InnerColor = innerColor;
        OuterColor = outerColor;
    }

    [JsonPropertyName("frame_text")]
    public string FrameText { get; }

    [JsonPropertyName("inner_text")]
    public string InnerText { get; }

    [JsonPropertyName("outer_text")]
    public string OuterText { get; }

    [JsonPropertyName("inner_color")]
    public int[] InnerColor { get; }

    [JsonPropertyName("outer_color")]
    public int[] OuterColor { get; }

    public bool Equals(GlyphForgeRequest? other)
    {
        return other is not null
            && FrameText == other.FrameText
            && InnerText == other.InnerText
            && OuterText == other.OuterText
            && BinaryValueEquality.ContentEquals(InnerColor, other.InnerColor)
            && BinaryValueEquality.ContentEquals(OuterColor, other.OuterColor);
    }

    public override int GetHashCode()
    {
        return BinaryValueEquality.GetStableHashCode(FrameText, InnerText, OuterText);
    }
}
