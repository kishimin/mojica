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

    [Fact(Skip = "TODO: verify these transport concerns at the Adapter boundary")]
    public void Map_WhenCreatingGlyphForgeRequest_DoesNotSpecifyFontSizeOverridesOrAuthentication()
    {
        // ID: 8A-REQ-006
        // Source: docs/v1/api/adapters.md §15 Request
        // Given: A validated ImageGenerationRequest mapped under the current Glyph Forge contract.
        // When: The mapper creates the outbound request representation.
        // Then: It omits frame_font_size and output_font_size so Glyph Forge uses its default of 20.
        // Then: It does not add an authentication header to the request mapping result.
        // Blocked by: HTTP headers and serialized transport options are owned by the Adapter boundary.
        // Priority: P1
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
