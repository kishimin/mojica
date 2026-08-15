using System.Text.Json;
using Mojica.Api.Infrastructure;
using Mojica.Api.Models;

namespace Mojica.Api.Tests.Infrastructure;

public sealed class GlyphForgeRequestMapperSmallTests
{
    [Fact]
    public void Map_WhenImageTypeIsStandard_SelectsImagesEndpoint()
    {
        var request = ValidRequest(ImageType.Standard);

        var result = GlyphForgeRequestMapper.Map(request);

        Assert.Equal("/images", result.Path);
    }

    [Fact]
    public void Map_WhenImageTypeIsXBackground_SelectsBackgroundEndpoint()
    {
        var result = GlyphForgeRequestMapper.Map(ValidRequest(ImageType.XBackground));

        Assert.Equal("/images/background", result.Path);
    }

    [Fact]
    public void Map_WhenImageTypeIsXIcon_SelectsIconEndpoint()
    {
        var result = GlyphForgeRequestMapper.Map(ValidRequest(ImageType.XIcon));

        Assert.Equal("/images/x-icon", result.Path);
    }

    [Fact]
    public void Map_WhenRequestValuesAreValidated_MapsDomainFieldsToGlyphForgeFields()
    {
        var result = GlyphForgeRequestMapper.Map(ValidRequest(ImageType.Standard));

        Assert.Equal("KA", result.Payload.FrameText);
        Assert.Equal("🌻", result.Payload.InnerText);
        Assert.Equal("☀", result.Payload.OuterText);
        Assert.Equal([255, 212, 0], result.Payload.InnerColor);
        Assert.Equal([255, 105, 180], result.Payload.OuterColor);
    }

    [Fact]
    public void Map_WhenHexColorIsPink_ConvertsItToExpectedRgbComponents()
    {
        var result = GlyphForgeRequestMapper.Map(ValidRequest(ImageType.Standard));

        Assert.Equal([255, 105, 180], result.Payload.OuterColor);
    }

    [Fact]
    public void Map_WhenPayloadIsSerialized_UsesGlyphForgeSnakeCaseFieldNames()
    {
        var result = GlyphForgeRequestMapper.Map(ValidRequest(ImageType.Standard));

        using var document = JsonDocument.Parse(JsonSerializer.Serialize(result.Payload));

        Assert.True(document.RootElement.TryGetProperty("frame_text", out _));
        Assert.True(document.RootElement.TryGetProperty("inner_text", out _));
        Assert.True(document.RootElement.TryGetProperty("outer_text", out _));
        Assert.True(document.RootElement.TryGetProperty("inner_color", out _));
        Assert.True(document.RootElement.TryGetProperty("outer_color", out _));
    }

    [Fact]
    public void Map_WhenEquivalentPayloadsUseDifferentColorArrays_ComparesThemByContent()
    {
        var first = GlyphForgeRequestMapper.Map(ValidRequest(ImageType.Standard));
        var second = GlyphForgeRequestMapper.Map(ValidRequest(ImageType.Standard));

        Assert.Equal(first.Payload, second.Payload);
        Assert.Equal(first.Payload.GetHashCode(), second.Payload.GetHashCode());
    }

    private static ImageGenerationRequest ValidRequest(ImageType type)
    {
        Assert.True(RenderText.TryCreate("KA", out var text, out _));
        Assert.True(PatternCharacter.TryCreate("🌻", out var foregroundCharacter, out _));
        Assert.True(HexColor.TryCreate("#FFD400", out var foregroundColor, out _));
        Assert.True(PatternCharacter.TryCreate("☀", out var backgroundCharacter, out _));
        Assert.True(HexColor.TryCreate("#FF69B4", out var backgroundColor, out _));
        Assert.True(ImageGenerationRequest.TryCreate(
            type,
            text,
            foregroundCharacter,
            foregroundColor,
            backgroundCharacter,
            backgroundColor,
            out var request,
            out _));

        return request!;
    }
}
