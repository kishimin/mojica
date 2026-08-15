namespace Mojica.Api.Tests.Infrastructure;

public sealed class GlyphForgeResponseMapperSmallTests
{
    [Fact(Skip = "TODO: implement Glyph Forge response mapping")]
    public void Map_WhenSuccessfulPngResponse_ReturnsGeneratedImageData()
    {
        // ID: 8B-RES-001
        // Source: docs/v1/api/adapters.md §10, §14, §15
        // Given: A successful response with image/png content type and non-empty PNG bytes.
        // When: The response mapper converts the Glyph Forge response to a port result.
        // Then: The result is successful and preserves the binary content and media type in GeneratedImageData.
        // Blocked by: The response mapper contract and implementation are not yet present.
        // Priority: P0
    }

    [Fact(Skip = "TODO: implement Glyph Forge response mapping")]
    public void Map_WhenSuccessfulResponseHasNonPngContentType_ReturnsInvalidResponse()
    {
        // ID: 8B-RES-002
        // Source: docs/v1/api/adapters.md §10 and §14
        // Given: A successful response whose content type is not an accepted image type.
        // When: The response mapper converts the Glyph Forge response to a port result.
        // Then: The result is a failure with the InvalidResponse error code.
        // Blocked by: The response mapper contract and implementation are not yet present.
        // Priority: P0
    }

    [Fact(Skip = "TODO: implement Glyph Forge response mapping")]
    public void Map_WhenSuccessfulResponseHasEmptyBody_ReturnsInvalidResponse()
    {
        // ID: 8B-RES-003
        // Source: docs/v1/api/adapters.md §10 and §14
        // Given: A successful response with an empty image body.
        // When: The response mapper converts the Glyph Forge response to a port result.
        // Then: The result is a failure with the InvalidResponse error code.
        // Blocked by: The response mapper contract and implementation are not yet present.
        // Priority: P0
    }

    [Fact(Skip = "TODO: implement Glyph Forge response mapping")]
    public void Map_WhenResponseIsRateLimited_ReturnsRateLimitedError()
    {
        // ID: 8B-RES-004
        // Source: docs/v1/api/adapters.md §11, §14, §15
        // Given: A response with HTTP status 429.
        // When: The response mapper converts the Glyph Forge response to a port result.
        // Then: The result is a failure with the RateLimited error code and contains no provider response body.
        // Blocked by: The response mapper contract and implementation are not yet present.
        // Priority: P0
    }

    [Fact(Skip = "TODO: implement Glyph Forge response mapping")]
    public void Map_WhenOutputSizeIsRejected_ReturnsOutputSizeExceededWithoutExternalDetails()
    {
        // ID: 8B-RES-005
        // Source: docs/v1/api/adapters.md §11, §14, §15
        // Given: A response with HTTP status 422 and a provider error body containing external details.
        // When: The response mapper converts the Glyph Forge response to a port result.
        // Then: The result is a failure with the OutputSizeExceeded error code and does not expose the provider body.
        // Blocked by: The response mapper contract and implementation are not yet present.
        // Priority: P0
    }

    [Fact(Skip = "TODO: implement Glyph Forge response mapping")]
    public void Map_WhenResponseIndicatesUnavailable_ReturnsUnavailableError()
    {
        // ID: 8B-RES-006
        // Source: docs/v1/api/adapters.md §11, §14, §15
        // Given: A response with HTTP status 503.
        // When: The response mapper converts the Glyph Forge response to a port result.
        // Then: The result is a failure with the Unavailable error code and contains no provider response body.
        // Blocked by: The response mapper contract and implementation are not yet present.
        // Priority: P0
    }

    [Fact(Skip = "TODO: implement Glyph Forge response mapping")]
    public void Map_WhenResponseIsServerFailure_ReturnsFailedError()
    {
        // ID: 8B-RES-007
        // Source: docs/v1/api/adapters.md §11 and §15
        // Given: A response with an unhandled 5xx HTTP status.
        // When: The response mapper converts the Glyph Forge response to a port result.
        // Then: The result is a failure with the Failed error code and contains no provider response body.
        // Blocked by: The response mapper contract and implementation are not yet present.
        // Priority: P1
    }

    [Fact(Skip = "TODO: implement Glyph Forge response mapping")]
    public void Map_WhenTimeoutOccurs_ReturnsTimeoutError()
    {
        // ID: 8B-RES-008
        // Source: docs/v1/api/adapters.md §11, §14, §15
        // Given: The Glyph Forge call ends because its configured timeout elapses.
        // When: The response mapping boundary converts the timeout outcome to a port result.
        // Then: The result is a failure with the Timeout error code and no provider details.
        // Blocked by: The response mapper contract and implementation are not yet present.
        // Priority: P0
    }

    [Fact(Skip = "TODO: implement Glyph Forge response mapping")]
    public void Map_WhenCommunicationFails_ReturnsUnavailableError()
    {
        // ID: 8B-RES-009
        // Source: docs/v1/api/adapters.md §11, §14, §15
        // Given: The Glyph Forge call fails before receiving an HTTP response because of a communication error.
        // When: The response mapping boundary converts the communication outcome to a port result.
        // Then: The result is a failure with the Unavailable error code and no provider details.
        // Blocked by: The response mapper contract and implementation are not yet present.
        // Priority: P0
    }
}
