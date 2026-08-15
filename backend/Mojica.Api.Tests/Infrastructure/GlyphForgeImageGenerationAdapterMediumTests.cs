namespace Mojica.Api.Tests.Infrastructure;

public sealed class GlyphForgeImageGenerationAdapterMediumTests
{
    [Fact(Skip = "TODO: implement the Adapter HTTP contract")]
    public void Send_WhenCreatingGlyphForgeRequest_DoesNotSpecifyFontSizeOverridesOrAuthentication()
    {
        // ID: 9-REQ-001
        // Source: docs/v1/api/adapters.md §14 Medium Tests and §15 Request
        // Given: A configured Adapter and a validated ImageGenerationRequest.
        // When: The Adapter sends the request through its HTTP client.
        // Then: The serialized body omits frame_font_size and output_font_size so Glyph Forge uses its default of 20.
        // Then: The outbound request does not contain an authentication header.
        // Blocked by: The Adapter and its controllable HTTP handler are implemented in branch 9.
        // Priority: P1
    }
}
