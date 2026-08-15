using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Mojica.Api.Infrastructure;
using Mojica.Api.Models;
using Mojica.Api.Ports;
using Xunit.Sdk;

namespace Mojica.Api.Tests.Infrastructure;

public sealed class GlyphForgeImageGenerationAdapterMediumTests
{
    [Fact]
    public async Task Send_WhenImageTypeIsStandard_UsesPostImagesEndpoint()
    {
        HttpMethod? requestMethod = null;
        Uri? requestUri = null;
        var adapter = CreateAdapter(request =>
        {
            requestMethod = request.Method;
            requestUri = request.RequestUri;
            return PngResponse();
        });

        await adapter.GenerateAsync(ValidRequest(), CancellationToken.None);

        Assert.Equal(HttpMethod.Post, requestMethod);
        Assert.Equal("https://glyph-forge.example/images", requestUri?.ToString());
    }

    [Fact]
    public async Task Send_WhenImageTypeIsXBackground_UsesPostBackgroundEndpoint()
    {
        HttpMethod? requestMethod = null;
        Uri? requestUri = null;
        var adapter = CreateAdapter(request =>
        {
            requestMethod = request.Method;
            requestUri = request.RequestUri;
            return PngResponse();
        });

        await adapter.GenerateAsync(ValidRequest(ImageType.XBackground), CancellationToken.None);

        Assert.Equal(HttpMethod.Post, requestMethod);
        Assert.Equal("https://glyph-forge.example/images/background", requestUri?.ToString());
    }

    [Fact(Skip = "TODO: implement the request method and endpoint contract assertion")]
    public void Send_WhenImageTypeIsXIcon_UsesPostXIconEndpoint()
    {
        // ID: GF-ADAPTER-M-003
        // Source: docs/v1/api/adapters.md §15, Endpoints
        // Given: A valid x-icon ImageGenerationRequest and a controllable HTTP handler
        // When: The Adapter sends the generation request
        // Then: The observed request uses POST and the /images/x-icon endpoint
        // Error: Fail if the endpoint does not correspond to ImageType.XIcon
        // Priority: P1
    }

    [Fact(Skip = "TODO: implement the request Content-Type contract assertion")]
    public void Send_WhenCreatingGlyphForgeRequest_UsesApplicationJsonContentType()
    {
        // ID: GF-ADAPTER-M-004
        // Source: docs/v1/api/adapters.md §15, Request
        // Given: A valid ImageGenerationRequest and a controllable HTTP handler
        // When: The Adapter serializes and sends the request
        // Then: The request Content-Type is application/json with the contract charset
        // Error: Fail if the request omits Content-Type or uses a different media type
        // Priority: P1
    }

    [Fact]
    public async Task Send_WhenCreatingGlyphForgeRequest_DoesNotSpecifyFontSizeOverridesOrAuthentication()
    {
        string? requestBody = null;
        string[]? requestHeaders = null;
        var adapter = CreateAsyncAdapter(async (request, _) =>
        {
            var content = request.Content
                ?? throw new XunitException("The Adapter must send a request body.");
            requestBody = await content.ReadAsStringAsync();
            requestHeaders = request.Headers.Select(header => header.Key).ToArray();
            return PngResponse();
        });

        await adapter.GenerateAsync(ValidRequest(), CancellationToken.None);

        var serializedRequest = requestBody
            ?? throw new XunitException("The HTTP handler did not observe a request body.");
        using var document = JsonDocument.Parse(serializedRequest);
        Assert.False(document.RootElement.TryGetProperty("frame_font_size", out _));
        Assert.False(document.RootElement.TryGetProperty("output_font_size", out _));
        var observedHeaders = requestHeaders
            ?? throw new XunitException("The HTTP handler did not observe request headers.");
        Assert.DoesNotContain(
            observedHeaders,
            header => header.Equals("Authorization", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(
            observedHeaders,
            header => header.Equals("X-API-Key", StringComparison.OrdinalIgnoreCase));
        Assert.DoesNotContain(
            observedHeaders,
            header => header.Equals("Api-Key", StringComparison.OrdinalIgnoreCase));
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
        return ValidRequest(ImageType.Standard);
    }

    private static ImageGenerationRequest ValidRequest(ImageType imageType)
    {
        Assert.True(RenderText.TryCreate("KA", out var text, out _));
        Assert.True(PatternCharacter.TryCreate("🌻", out var foregroundCharacter, out _));
        Assert.True(HexColor.TryCreate("#FFD400", out var foregroundColor, out _));
        Assert.True(PatternCharacter.TryCreate("☀", out var backgroundCharacter, out _));
        Assert.True(HexColor.TryCreate("#FF69B4", out var backgroundColor, out _));
        Assert.True(ImageGenerationRequest.TryCreate(
            imageType,
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

    private sealed class ThrowingHttpClientFactory(Exception exception) : IHttpClientFactory
    {
        public HttpClient CreateClient(string name) => throw exception;
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
    public async Task Send_WhenRetryAfterIsHttpDate_ParsesRemainingSeconds()
    {
        var retryAt = DateTimeOffset.UtcNow.AddSeconds(8);
        var response = new HttpResponseMessage(HttpStatusCode.TooManyRequests)
        {
            Content = new StringContent("provider secret"),
        };
        response.Headers.RetryAfter = new RetryConditionHeaderValue(retryAt);
        var adapter = CreateAdapter(_ => response);

        var result = await adapter.GenerateAsync(ValidRequest(), CancellationToken.None);

        Assert.Equal(ImageGenerationPortErrorCode.RateLimited, result.Error?.ErrorCode);
        var retryAfter = result.Error?.RetryAfter
            ?? throw new XunitException("Retry-After should be parsed.");
        Assert.InRange(retryAfter, 1, 8);
    }

    [Fact]
    public async Task Send_WhenRetryAfterDateHasElapsed_UsesZeroSeconds()
    {
        var response = new HttpResponseMessage(HttpStatusCode.TooManyRequests)
        {
            Content = new StringContent("provider secret"),
        };
        response.Headers.RetryAfter = new RetryConditionHeaderValue(DateTimeOffset.UtcNow.AddSeconds(-1));
        var adapter = CreateAdapter(_ => response);

        var result = await adapter.GenerateAsync(ValidRequest(), CancellationToken.None);

        Assert.Equal(0, result.Error?.RetryAfter);
    }

    [Fact]
    public async Task Send_WhenErrorResponseHasBody_DrainsWithoutExposingTheBody()
    {
        var content = new TrackingHttpContent();
        var response = new HttpResponseMessage(HttpStatusCode.TooManyRequests)
        {
            Content = content,
        };
        var adapter = CreateAdapter(_ => response);

        await adapter.GenerateAsync(ValidRequest(), CancellationToken.None);

        Assert.True(content.WasRead);
    }

    [Fact]
    public async Task Send_WhenReadingResponseBodyFailsWithIOException_ReturnsUnavailable()
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new ThrowingHttpContent(),
        };
        var adapter = CreateAdapter(_ => response);

        var result = await adapter.GenerateAsync(ValidRequest(), CancellationToken.None);

        Assert.Equal(ImageGenerationPortErrorCode.Unavailable, result.Error?.ErrorCode);
    }

    [Fact]
    public async Task Send_WhenErrorBodyDrainFails_PreservesStatusDerivedError()
    {
        var response = new HttpResponseMessage(HttpStatusCode.TooManyRequests)
        {
            Content = new ThrowingHttpContent(),
        };
        var adapter = CreateAdapter(_ => response);

        var result = await adapter.GenerateAsync(ValidRequest(), CancellationToken.None);

        Assert.Equal(ImageGenerationPortErrorCode.RateLimited, result.Error?.ErrorCode);
    }

    [Fact]
    public async Task Send_WhenSuccessfulResponseExceedsMaximumImageSize_ReturnsInvalidResponse()
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new ByteArrayContent(new byte[10 * 1024 * 1024 + 1]),
        };
        response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
        var adapter = CreateAdapter(_ => response);

        var result = await adapter.GenerateAsync(ValidRequest(), CancellationToken.None);

        Assert.Equal(ImageGenerationPortErrorCode.InvalidResponse, result.Error?.ErrorCode);
    }

    [Fact]
    public async Task Send_WhenBaseAddressContainsPath_AppendsEndpointPath()
    {
        Uri? requestUri = null;
        var client = new HttpClient(new StubHttpMessageHandler(request =>
        {
            requestUri = request.RequestUri;
            return PngResponse();
        }))
        {
            BaseAddress = new Uri("https://glyph-forge.example/api")
        };
        var adapter = new GlyphForgeImageGenerationAdapter(new StubHttpClientFactory(client));

        await adapter.GenerateAsync(ValidRequest(), CancellationToken.None);

        Assert.Equal("https://glyph-forge.example/api/images", requestUri?.ToString());
    }

    [Fact]
    public async Task Send_WhenClientConfigurationIsInvalid_ReturnsUnavailable()
    {
        var adapter = new GlyphForgeImageGenerationAdapter(
            new ThrowingHttpClientFactory(new OptionsValidationException(
                "GlyphForge",
                typeof(GlyphForgeClientOptions),
                [])));

        var result = await adapter.GenerateAsync(ValidRequest(), CancellationToken.None);

        Assert.Equal(ImageGenerationPortErrorCode.Unavailable, result.Error?.ErrorCode);
    }

    [Fact]
    public async Task Send_WhenResponseBodyExceedsClientTimeout_ReturnsTimeout()
    {
        var client = new HttpClient(new StubHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new BlockingHttpContent(),
        }))
        {
            BaseAddress = new Uri("https://glyph-forge.example/"),
            Timeout = TimeSpan.FromMilliseconds(50),
        };
        var adapter = new GlyphForgeImageGenerationAdapter(new StubHttpClientFactory(client));

        var result = await adapter.GenerateAsync(ValidRequest(), CancellationToken.None);

        Assert.Equal(ImageGenerationPortErrorCode.Timeout, result.Error?.ErrorCode);
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

    private sealed class TrackingHttpContent : HttpContent
    {
        public bool WasRead { get; private set; }

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext? context)
        {
            WasRead = true;
            return Task.CompletedTask;
        }

        protected override bool TryComputeLength(out long length)
        {
            length = 0;
            return true;
        }
    }

    private sealed class ThrowingHttpContent : HttpContent
    {
        protected override Task SerializeToStreamAsync(Stream stream, TransportContext? context)
        {
            throw new IOException("simulated response body read failure");
        }

        protected override bool TryComputeLength(out long length)
        {
            length = 0;
            return true;
        }
    }

    private sealed class BlockingHttpContent : HttpContent
    {
        protected override Task SerializeToStreamAsync(Stream stream, TransportContext? context)
        {
            return Task.Delay(Timeout.InfiniteTimeSpan);
        }

        protected override Task SerializeToStreamAsync(
            Stream stream,
            TransportContext? context,
            CancellationToken cancellationToken)
        {
            return Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);
        }

        protected override bool TryComputeLength(out long length)
        {
            length = 0;
            return true;
        }
    }
}
