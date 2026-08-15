namespace Mojica.Api.Tests.Infrastructure;

public sealed class GlyphForgeRequestMapperSmallTests
{
    [Fact(Skip = "TODO: implement the request mapping contract")]
    public void Map_WhenImageTypeIsStandard_SelectsImagesEndpoint()
    {
        // ID: 8A-REQ-001
        // Source: docs/v1/api/adapters.md §15 Endpoints
        // Given: A validated ImageGenerationRequest whose ImageType is standard.
        // When: The request mapper selects the Glyph Forge endpoint.
        // Then: It returns POST /images and preserves the validated request payload.
        // Priority: P0
    }

    [Fact(Skip = "TODO: implement the request mapping contract")]
    public void Map_WhenImageTypeIsXBackground_SelectsBackgroundEndpoint()
    {
        // ID: 8A-REQ-002
        // Source: docs/v1/api/adapters.md §15 Endpoints
        // Given: A validated ImageGenerationRequest whose ImageType is x-background.
        // When: The request mapper selects the Glyph Forge endpoint.
        // Then: It returns POST /images/background and preserves the validated request payload.
        // Priority: P0
    }

    [Fact(Skip = "TODO: implement the request mapping contract")]
    public void Map_WhenImageTypeIsXIcon_SelectsIconEndpoint()
    {
        // ID: 8A-REQ-003
        // Source: docs/v1/api/adapters.md §15 Endpoints
        // Given: A validated ImageGenerationRequest whose ImageType is x-icon.
        // When: The request mapper selects the Glyph Forge endpoint.
        // Then: It returns POST /images/x-icon and preserves the validated request payload.
        // Priority: P0
    }

    [Fact(Skip = "TODO: implement the request mapping contract")]
    public void Map_WhenRequestValuesAreValidated_MapsDomainFieldsToGlyphForgeFields()
    {
        // ID: 8A-REQ-004
        // Source: docs/v1/api/adapters.md §15 Request
        // Given: A validated request with text, foreground/background characters, and HEX colors.
        // When: The request mapper creates the Glyph Forge request DTO.
        // Then: text maps to frame_text, foregroundCharacter to inner_text, and backgroundCharacter to outer_text without changing their values.
        // Then: foregroundColor and backgroundColor map to inner_color and outer_color as RGB components.
        // Theory candidate: use representative Unicode text/characters and normalized HEX colors.
        // Priority: P0
    }

    [Fact(Skip = "TODO: implement the request mapping contract")]
    public void Map_WhenHexColorIsPink_ConvertsItToExpectedRgbComponents()
    {
        // ID: 8A-REQ-005
        // Source: docs/v1/api/adapters.md §14 Small Tests and §15 Request
        // Given: A validated HEX color #FF69B4.
        // When: The request mapper converts the color for Glyph Forge.
        // Then: It produces RGB components 255, 105, and 180 in the correct array order.
        // Priority: P0
    }

    [Fact(Skip = "TODO: implement the request mapping contract")]
    public void Map_WhenCreatingGlyphForgeRequest_DoesNotSpecifyFontSizeOverridesOrAuthentication()
    {
        // ID: 8A-REQ-006
        // Source: docs/v1/api/adapters.md §15 Request
        // Given: A validated ImageGenerationRequest mapped under the current Glyph Forge contract.
        // When: The mapper creates the outbound request representation.
        // Then: It omits frame_font_size and output_font_size so Glyph Forge uses its default of 20.
        // Then: It does not add an authentication header to the request mapping result.
        // Error: Do not assert HTTP handler details here; those belong to the Adapter boundary.
        // Priority: P1
    }
}
