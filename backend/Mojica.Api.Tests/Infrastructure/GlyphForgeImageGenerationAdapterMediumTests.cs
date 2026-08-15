using System.Net;
using System.Net.Http.Headers;
using Mojica.Api.Infrastructure;
using Mojica.Api.Models;

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

    [Fact]
    public async Task Send_WhenSuccessfulPngResponse_PreservesContentTypeAndBinaryData()
    {
        var content = new byte[] { 0x89, 0x50, 0x4E, 0x47 };
        using var client = new HttpClient(new StubHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new ByteArrayContent(content)
            {
                Headers = { ContentType = new MediaTypeHeaderValue("image/png") },
            },
        }))
        {
            BaseAddress = new Uri("https://glyph-forge.example/"),
        };
        client.DefaultRequestHeaders.Accept.ParseAdd("image/png");
        var adapter = new GlyphForgeImageGenerationAdapter(new StubHttpClientFactory(client));

        var result = await adapter.GenerateAsync(ValidRequest(), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(content, result.Data?.Content);
        Assert.Equal("image/png", result.Data?.MediaType);
    }

    private static ImageGenerationRequest ValidRequest()
    {
        Assert.True(RenderText.TryCreate("KA", out var text, out _));
        Assert.True(PatternCharacter.TryCreate("🌻", out var foregroundCharacter, out _));
        Assert.True(HexColor.TryCreate("#FFD400", out var foregroundColor, out _));
        Assert.True(PatternCharacter.TryCreate("☀", out var backgroundCharacter, out _));
        Assert.True(HexColor.TryCreate("#FF69B4", out var backgroundColor, out _));
        Assert.True(ImageGenerationRequest.TryCreate(
            ImageType.Standard,
            text,
            foregroundCharacter,
            foregroundColor,
            backgroundCharacter,
            backgroundColor,
            out var request,
            out _));

        return request!;
    }

    private sealed class StubHttpClientFactory(HttpClient client) : IHttpClientFactory
    {
        public HttpClient CreateClient(string name) => client;
    }

    private sealed class StubHttpMessageHandler(
        Func<HttpRequestMessage, HttpResponseMessage> handler) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(handler(request));
        }
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
