using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mojica.Api.Infrastructure;
using Mojica.Api.Models;
using Mojica.Api.Ports;
using Xunit.Sdk;

namespace Mojica.Api.Tests.Infrastructure;

public sealed class GlyphForgeImageGenerationAdapterMediumTests
{
    [Fact]
    public async Task Send_WhenRequestIsNull_ThrowsArgumentNullException()
    {
        var adapter = CreateAdapter(_ => PngResponse());

        await Assert.ThrowsAsync<ArgumentNullException>(
            () => adapter.GenerateAsync(null!, CancellationToken.None));
    }

    [Fact]
    public async Task Send_WhenRequestTimesOut_LogsWarningForOperators()
    {
        var client = new HttpClient(new StubHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new BlockingHttpContent(),
        }))
        {
            BaseAddress = new Uri("https://glyph-forge.example/"),
            Timeout = TimeSpan.FromMilliseconds(50),
        };
        var recordingLogger = new RecordingLogger<GlyphForgeImageGenerationAdapter>();
        var adapter = new GlyphForgeImageGenerationAdapter(new StubHttpClientFactory(client), recordingLogger);

        var result = await adapter.GenerateAsync(ValidRequest(), CancellationToken.None);

        Assert.Equal(ImageGenerationPortErrorCode.Timeout, result.Error?.ErrorCode);
        var warning = Assert.Single(recordingLogger.Entries, entry => entry.Level == LogLevel.Warning);
        Assert.Equal("Glyph Forge request timed out.", warning.Message);
    }

    [Fact]
    public async Task Send_WhenClientConfigurationIsInvalid_LogsCommunicationFailureWithExceptionType()
    {
        var recordingLogger = new RecordingLogger<GlyphForgeImageGenerationAdapter>();
        var adapter = new GlyphForgeImageGenerationAdapter(
            new ThrowingHttpClientFactory(new OptionsValidationException(
                "GlyphForge",
                typeof(GlyphForgeClientOptions),
                [])),
            recordingLogger);

        var result = await adapter.GenerateAsync(ValidRequest(), CancellationToken.None);

        Assert.Equal(ImageGenerationPortErrorCode.Unavailable, result.Error?.ErrorCode);
        var warning = Assert.Single(recordingLogger.Entries, entry => entry.Level == LogLevel.Warning);
        Assert.Equal(
            "Glyph Forge communication failed with OptionsValidationException.",
            warning.Message);
    }

    [Fact]
    public async Task Send_WhenSuccessfulResponseEqualsMaximumImageSize_ReturnsSuccessWithFullContent()
    {
        var content = new byte[10 * 1024 * 1024];
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new ByteArrayContent(content),
        };
        response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
        var adapter = CreateAdapter(_ => response);

        var result = await adapter.GenerateAsync(ValidRequest(), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(content.Length, result.Data?.Content.Length);
    }

    [Fact]
    public async Task Send_WhenDeclaredContentLengthExceedsMaximum_RejectsWithoutReadingActualBody()
    {
        var body = new TrackingHttpContent();
        body.Headers.ContentLength = 10 * 1024 * 1024 + 1;
        body.Headers.ContentType = new MediaTypeHeaderValue("image/png");
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = body,
        };
        var adapter = CreateAdapter(_ => response);

        var result = await adapter.GenerateAsync(ValidRequest(), CancellationToken.None);

        Assert.Equal(ImageGenerationPortErrorCode.InvalidResponse, result.Error?.ErrorCode);
        Assert.False(body.WasRead);
    }

    [Fact]
    public async Task Send_WhenActualBodyExceedsMaximumWithoutDeclaredContentLength_ReturnsInvalidResponse()
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new OversizedHttpContent(10 * 1024 * 1024 + 1),
        };
        response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
        response.Content.Headers.ContentLength = null;
        var adapter = CreateAdapter(_ => response);

        var result = await adapter.GenerateAsync(ValidRequest(), CancellationToken.None);

        Assert.Equal(ImageGenerationPortErrorCode.InvalidResponse, result.Error?.ErrorCode);
    }

    [Fact]
    public async Task Send_WhenByteArrayBodyExceedsMaximumWithoutDeclaredContentLength_ReturnsInvalidResponse()
    {
        // Uses the built-in ByteArrayContent (as real GlyphForge PNG responses do) with its
        // Content-Length header suppressed, so the adapter can only detect the oversized body
        // while writing through BoundedMemoryStream's byte[]-based Write/WriteAsync overrides
        // rather than the declared-length fast path or the Memory<byte>-based override.
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new ByteArrayContent(new byte[10 * 1024 * 1024 + 1]),
        };
        response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
        response.Content.Headers.ContentLength = null;
        var adapter = CreateAdapter(_ => response);

        var result = await adapter.GenerateAsync(ValidRequest(), CancellationToken.None);

        Assert.Equal(ImageGenerationPortErrorCode.InvalidResponse, result.Error?.ErrorCode);
    }

    [Fact]
    public async Task Send_WhenHttpClientHasNoBaseAddress_ReturnsUnavailableWithoutThrowing()
    {
        var client = new HttpClient(new StubHttpMessageHandler(_ => PngResponse()));
        var adapter = new GlyphForgeImageGenerationAdapter(new StubHttpClientFactory(client));

        var result = await adapter.GenerateAsync(ValidRequest(), CancellationToken.None);

        Assert.Equal(ImageGenerationPortErrorCode.Unavailable, result.Error?.ErrorCode);
    }

    [Fact]
    public async Task Send_WhenRetryAfterDeltaEqualsMaximumSupportedSeconds_ParsesFullValue()
    {
        var response = new HttpResponseMessage(HttpStatusCode.TooManyRequests)
        {
            Content = new StringContent("provider secret"),
        };
        response.Headers.RetryAfter = new RetryConditionHeaderValue(TimeSpan.FromSeconds(int.MaxValue));
        var adapter = CreateAdapter(_ => response);

        var result = await adapter.GenerateAsync(ValidRequest(), CancellationToken.None);

        Assert.Equal(int.MaxValue, result.Error?.RetryAfter);
    }

    [Fact]
    public async Task Send_WhenCreatingGlyphForgeRequest_UsesApplicationJsonContentType()
    {
        string? mediaType = null;
        string? charset = null;
        var adapter = CreateAdapter(request =>
        {
            mediaType = request.Content?.Headers.ContentType?.MediaType;
            charset = request.Content?.Headers.ContentType?.CharSet;
            return PngResponse();
        });

        await adapter.GenerateAsync(ValidRequest(), CancellationToken.None);

        Assert.Equal("application/json", mediaType);
        Assert.Equal("utf-8", charset);
    }

    [Theory]
    [InlineData("frame_font_size")]
    [InlineData("output_font_size")]
    public async Task Send_WhenCreatingGlyphForgeRequest_DoesNotSpecifyFontSizeOverride(
        string forbiddenField)
    {
        string? requestBody = null;
        var adapter = CreateAsyncAdapter(async (request, _) =>
        {
            var content = request.Content
                ?? throw new XunitException("The Adapter must send a request body.");
            requestBody = await content.ReadAsStringAsync();
            return PngResponse();
        });

        await adapter.GenerateAsync(ValidRequest(), CancellationToken.None);

        var serializedRequest = requestBody
            ?? throw new XunitException("The HTTP handler did not observe a request body.");
        using var document = JsonDocument.Parse(serializedRequest);
        Assert.False(document.RootElement.TryGetProperty(forbiddenField, out _));
    }

    [Theory]
    [InlineData("Authorization")]
    [InlineData("X-API-Key")]
    [InlineData("Api-Key")]
    public async Task Send_WhenCreatingGlyphForgeRequest_DoesNotSpecifyAuthenticationHeader(
        string forbiddenHeader)
    {
        string[]? requestHeaders = null;
        var adapter = CreateAdapter(request =>
        {
            requestHeaders = request.Headers.Select(header => header.Key).ToArray();
            return PngResponse();
        });

        await adapter.GenerateAsync(ValidRequest(), CancellationToken.None);

        var observedHeaders = requestHeaders
            ?? throw new XunitException("The HTTP handler did not observe request headers.");
        Assert.DoesNotContain(
            observedHeaders,
            header => header.Equals(forbiddenHeader, StringComparison.OrdinalIgnoreCase));
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

    private sealed class FixedTimeProvider(DateTimeOffset now) : TimeProvider
    {
        public override DateTimeOffset GetUtcNow() => now;
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
    public async Task Send_WhenRetryAfterDateIsExactlyMaximumSupportedSeconds_ParsesFullValue()
    {
        var now = DateTimeOffset.UtcNow;
        var timeProvider = new FixedTimeProvider(now);
        var retryAt = now + TimeSpan.FromSeconds(int.MaxValue);
        var response = new HttpResponseMessage(HttpStatusCode.TooManyRequests)
        {
            Content = new StringContent("provider secret"),
        };
        response.Headers.RetryAfter = new RetryConditionHeaderValue(retryAt);
        var adapter = CreateAdapter(_ => response, timeProvider);

        var result = await adapter.GenerateAsync(ValidRequest(), CancellationToken.None);

        Assert.Equal(ImageGenerationPortErrorCode.RateLimited, result.Error?.ErrorCode);
        Assert.Equal(int.MaxValue, result.Error?.RetryAfter);
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
    public async Task Send_WhenCallerCancellationIsRequestedWhileDrainingErrorBody_PropagatesCancellation()
    {
        // The internal timeout token and the caller's token are linked, so cancelling
        // either one cancels the drain's Task. This proves the filter correctly tells
        // them apart: caller-initiated cancellation while discarding an error body must
        // propagate, not be swallowed as if it were our own internal timeout.
        var response = new HttpResponseMessage(HttpStatusCode.TooManyRequests)
        {
            Content = new BlockingHttpContent(),
        };
        var adapter = CreateAdapter(_ => response);
        using var cancellation = new CancellationTokenSource();

        var operation = adapter.GenerateAsync(ValidRequest(), cancellation.Token);
        cancellation.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => operation);
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
        Func<HttpRequestMessage, HttpResponseMessage> handler,
        TimeProvider? timeProvider = null)
    {
        var client = new HttpClient(new StubHttpMessageHandler(handler))
        {
            BaseAddress = new Uri("https://glyph-forge.example/"),
        };
        return new GlyphForgeImageGenerationAdapter(new StubHttpClientFactory(client), timeProvider: timeProvider);
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

    /// <summary>
    /// Streams more bytes than the adapter's maximum image size without declaring a
    /// Content-Length, so the adapter can only detect the overflow while writing.
    /// </summary>
    private sealed class OversizedHttpContent(int totalBytes) : HttpContent
    {
        protected override async Task SerializeToStreamAsync(Stream stream, TransportContext? context)
        {
            var chunk = new byte[64 * 1024];
            var remaining = totalBytes;
            while (remaining > 0)
            {
                var writeSize = Math.Min(chunk.Length, remaining);
                await stream.WriteAsync(chunk.AsMemory(0, writeSize));
                remaining -= writeSize;
            }
        }

        protected override bool TryComputeLength(out long length)
        {
            length = 0;
            return false;
        }
    }
}
