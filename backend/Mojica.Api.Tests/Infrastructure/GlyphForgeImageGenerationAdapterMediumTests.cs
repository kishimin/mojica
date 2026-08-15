using System.Net;
using System.Net.Http.Headers;
using Mojica.Api.Infrastructure;
using Mojica.Api.Models;
using Mojica.Api.Ports;

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

    [Fact]
    public async Task Send_WhenRateLimitedResponseHasRetryAfter_ParsesIntegerSeconds()
    {
        var response = new HttpResponseMessage(HttpStatusCode.TooManyRequests)
        {
            Content = new StringContent("provider secret"),
        };
        response.Headers.RetryAfter = new RetryConditionHeaderValue(TimeSpan.FromSeconds(7));
        var adapter = CreateAdapter(_ => response);

        var result = await adapter.GenerateAsync(ValidRequest(), CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal(ImageGenerationPortErrorCode.RateLimited, result.Error?.ErrorCode);
        Assert.Equal(7, result.Error?.RetryAfter);
        Assert.Null(result.Error?.Details);
    }

    [Fact]
    public async Task Send_WhenUnavailableResponseHasRetryAfter_ParsesIntegerSeconds()
    {
        var response = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
        {
            Content = new StringContent("provider secret"),
        };
        response.Headers.RetryAfter = new RetryConditionHeaderValue(TimeSpan.FromSeconds(1));
        var adapter = CreateAdapter(_ => response);

        var result = await adapter.GenerateAsync(ValidRequest(), CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal(ImageGenerationPortErrorCode.Unavailable, result.Error?.ErrorCode);
        Assert.Equal(1, result.Error?.RetryAfter);
        Assert.Null(result.Error?.Details);
    }

    [Fact]
    public async Task Send_WhenRetryAfterIsMalformed_DoesNotExposeOrInventRetryPeriod()
    {
        var response = new HttpResponseMessage(HttpStatusCode.TooManyRequests)
        {
            Content = new StringContent("provider secret"),
        };
        response.Headers.TryAddWithoutValidation("Retry-After", "later");
        var adapter = CreateAdapter(_ => response);

        var result = await adapter.GenerateAsync(ValidRequest(), CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal(ImageGenerationPortErrorCode.RateLimited, result.Error?.ErrorCode);
        Assert.Null(result.Error?.RetryAfter);
        Assert.Null(result.Error?.Details);
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

    [Theory]
    [InlineData(422, "OUTPUT_SIZE_EXCEEDED")]
    [InlineData(429, "RATE_LIMITED")]
    [InlineData(503, "UNAVAILABLE")]
    [InlineData(502, "FAILED")]
    public async Task Send_WhenExternalResponsesFail_MapsEachStatusToPortError(
        int statusCode,
        string expectedCode)
    {
        var response = new HttpResponseMessage((HttpStatusCode)statusCode)
        {
            Content = new StringContent("provider secret"),
        };
        var adapter = CreateAdapter(_ => response);

        var result = await adapter.GenerateAsync(ValidRequest(), CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal(expectedCode, result.Error?.Code);
        Assert.Null(result.Error?.Details);
    }

    private static GlyphForgeImageGenerationAdapter CreateAdapter(
        Func<HttpRequestMessage, HttpResponseMessage> handler)
    {
        var client = new HttpClient(new StubHttpMessageHandler(handler))
        {
            BaseAddress = new Uri("https://glyph-forge.example/"),
        };
        return new GlyphForgeImageGenerationAdapter(new StubHttpClientFactory(client));
    }
}
