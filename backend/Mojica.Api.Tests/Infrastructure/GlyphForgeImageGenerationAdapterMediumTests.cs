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

    [Fact(Skip = "TODO: implement Glyph Forge response mapping")]
    public void Send_WhenSuccessfulPngResponse_PreservesContentTypeAndBinaryData()
    {
        // ID: 8B-MED-001
        // Source: docs/v1/api/adapters.md §10, §14, §15
        // Given: A controllable HTTP handler returns 200, image/png, and valid PNG bytes.
        // When: The Adapter sends a generation request and maps the HTTP response.
        // Then: The port result is successful and GeneratedImageData preserves the response bytes and media type.
        // Blocked by: The response mapper and controllable HTTP handler are not yet implemented.
        // Priority: P0
    }

    [Fact(Skip = "TODO: implement Glyph Forge response mapping")]
    public void Send_WhenRateLimitedResponseHasRetryAfter_ParsesIntegerSeconds()
    {
        // ID: 8B-MED-002
        // Source: docs/v1/api/adapters.md §11 and §15
        // Given: A controllable HTTP handler returns 429 with Retry-After set to an integer number of seconds.
        // When: The Adapter sends a generation request and maps the HTTP response.
        // Then: The port result is RateLimited and carries the parsed retry period without exposing the response body.
        // Blocked by: The response mapper and controllable HTTP handler are not yet implemented.
        // Priority: P0
    }

    [Fact(Skip = "TODO: implement Glyph Forge response mapping")]
    public void Send_WhenUnavailableResponseHasRetryAfter_ParsesIntegerSeconds()
    {
        // ID: 8B-MED-003
        // Source: docs/v1/api/adapters.md §11 and §15
        // Given: A controllable HTTP handler returns 503 with Retry-After set to an integer number of seconds.
        // When: The Adapter sends a generation request and maps the HTTP response.
        // Then: The port result is Unavailable and carries the parsed retry period without exposing the response body.
        // Blocked by: The response mapper and controllable HTTP handler are not yet implemented.
        // Priority: P1
    }

    [Fact(Skip = "TODO: implement Glyph Forge response mapping")]
    public void Send_WhenRetryAfterIsMalformed_DoesNotExposeOrInventRetryPeriod()
    {
        // ID: 8B-MED-004
        // Source: docs/v1/api/adapters.md §11 and §15
        // Given: A rate-limit or unavailable response has a missing or malformed Retry-After header.
        // When: The Adapter sends a generation request and maps the HTTP response.
        // Then: The port result remains safe and does not invent a retry period from untrusted header text.
        // Blocked by: The response mapper and controllable HTTP handler are not yet implemented.
        // Priority: P1
    }

    [Fact(Skip = "TODO: implement Glyph Forge response mapping")]
    public void Send_WhenCallerCancellationIsRequested_PropagatesCancellationToHttpCommunication()
    {
        // ID: 8B-MED-005
        // Source: docs/v1/api/adapters.md §14 and §15
        // Given: The HTTP handler blocks until the caller cancellation token is cancelled.
        // When: The caller cancels the generation operation.
        // Then: Cancellation reaches the HTTP communication boundary without a real-time sleep or retry.
        // Blocked by: The Adapter and its controllable HTTP handler are not yet implemented.
        // Priority: P1
    }

    [Fact(Skip = "TODO: implement Glyph Forge response mapping")]
    public void Send_WhenExternalResponsesFail_MapsEachStatusToPortError()
    {
        // ID: 8B-MED-006
        // Source: docs/v1/api/adapters.md §11, §14, §15
        // Given: Controlled responses cover 422, 429, 503, and another 5xx status with provider error bodies.
        // When: The Adapter sends a generation request for each response.
        // Then: The outcomes map to OutputSizeExceeded, RateLimited, Unavailable, and Failed respectively without leaking bodies.
        // Blocked by: The response mapper and controllable HTTP handler are not yet implemented.
        // Priority: P0
    }
}
