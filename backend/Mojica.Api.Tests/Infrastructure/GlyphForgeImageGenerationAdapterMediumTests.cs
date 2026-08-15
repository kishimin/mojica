using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Mojica.Api.Infrastructure;
using Mojica.Api.Models;
using Mojica.Api.Ports;
using Xunit.Sdk;

namespace Mojica.Api.Tests.Infrastructure;

public sealed class GlyphForgeImageGenerationAdapterMediumTests
{
    [Fact]
    public async Task Send_WhenCreatingGlyphForgeRequest_DoesNotSpecifyFontSizeOverridesOrAuthentication()
    {
        string? requestBody = null;
        var hasAuthorizationHeader = false;
        var adapter = CreateAsyncAdapter(async (request, _) =>
        {
            var content = request.Content
                ?? throw new XunitException("The Adapter must send a request body.");
            requestBody = await content.ReadAsStringAsync();
            hasAuthorizationHeader = request.Headers.Authorization is not null;
            return PngResponse();
        });

        await adapter.GenerateAsync(ValidRequest(), CancellationToken.None);

        var serializedRequest = requestBody
            ?? throw new XunitException("The HTTP handler did not observe a request body.");
        using var document = JsonDocument.Parse(serializedRequest);
        Assert.False(document.RootElement.TryGetProperty("frame_font_size", out _));
        Assert.False(document.RootElement.TryGetProperty("output_font_size", out _));
        Assert.False(hasAuthorizationHeader);
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

    [Fact]
    public async Task Send_WhenCallerCancellationIsRequested_PropagatesCancellationToHttpCommunication()
    {
        var handlerStarted = new TaskCompletionSource(
            TaskCreationOptions.RunContinuationsAsynchronously);
        var adapter = CreateAsyncAdapter(async (_, cancellationToken) =>
        {
            handlerStarted.SetResult();
            await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);
            return PngResponse();
        });
        using var cancellation = new CancellationTokenSource();

        var operation = adapter.GenerateAsync(ValidRequest(), cancellation.Token);
        await handlerStarted.Task;
        cancellation.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => operation);
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

    private static GlyphForgeImageGenerationAdapter CreateAsyncAdapter(
        Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> handler)
    {
        var client = new HttpClient(new AsyncStubHttpMessageHandler(handler))
        {
            BaseAddress = new Uri("https://glyph-forge.example/"),
        };
        return new GlyphForgeImageGenerationAdapter(new StubHttpClientFactory(client));
    }

    private static HttpResponseMessage PngResponse()
    {
        return new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new ByteArrayContent([0x89, 0x50, 0x4E, 0x47])
            {
                Headers = { ContentType = new MediaTypeHeaderValue("image/png") },
            },
        };
    }

    private sealed class AsyncStubHttpMessageHandler(
        Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> handler) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            return handler(request, cancellationToken);
        }
    }
}
