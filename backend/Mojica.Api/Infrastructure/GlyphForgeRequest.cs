using System.Text.Json.Serialization;

namespace Mojica.Api.Infrastructure;

public sealed record GlyphForgeRequest(
    [property: JsonPropertyName("frame_text")]
    string FrameText,
    [property: JsonPropertyName("inner_text")]
    string InnerText,
    [property: JsonPropertyName("outer_text")]
    string OuterText,
    [property: JsonPropertyName("inner_color")]
    int[] InnerColor,
    [property: JsonPropertyName("outer_color")]
    int[] OuterColor);
