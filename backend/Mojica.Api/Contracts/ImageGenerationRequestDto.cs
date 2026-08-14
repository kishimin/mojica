using System.Text.Json.Serialization;

namespace Mojica.Api.Contracts;

public sealed record ImageGenerationRequestDto(
    [property: JsonPropertyName("type")] string? Type,
    [property: JsonPropertyName("text")] string? Text,
    [property: JsonPropertyName("foregroundCharacter")] string? ForegroundCharacter,
    [property: JsonPropertyName("foregroundColor")] string? ForegroundColor,
    [property: JsonPropertyName("backgroundCharacter")] string? BackgroundCharacter,
    [property: JsonPropertyName("backgroundColor")] string? BackgroundColor);
