using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mojica.Api.Models;
using Mojica.Api.Ports;

namespace Mojica.Api.Infrastructure;

public sealed class GlyphForgeImageGenerationAdapter(
    IHttpClientFactory httpClientFactory,
    ILogger<GlyphForgeImageGenerationAdapter>? logger = null) : ImageGenerationPort
{
    private const int MaximumImageBytes = 10 * 1024 * 1024;

    public async Task<ImageGenerationPortResult> GenerateAsync(
        ImageGenerationRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        (string Path, GlyphForgeRequest Payload) mapped;
        try
        {
            mapped = GlyphForgeRequestMapper.Map(request);
        }
        catch (InvalidOperationException)
        {
            return GlyphForgeResponseMapper.Map(
                new GlyphForgeResponse(null, null, null, failure: GlyphForgeResponseFailure.Failed));
        }

        using var timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        try
        {
            var client = httpClientFactory.CreateClient("GlyphForge");
            if (client.Timeout != Timeout.InfiniteTimeSpan)
            {
                timeout.CancelAfter(client.Timeout);
            }

            using var httpRequest = new HttpRequestMessage(
                HttpMethod.Post,
                CreateRequestUri(client.BaseAddress, mapped.Path));
            httpRequest.Content = JsonContent.Create(mapped.Payload);
            using var response = await client.SendAsync(
                httpRequest,
                HttpCompletionOption.ResponseHeadersRead,
                timeout.Token);
            var statusCode = (int)response.StatusCode;
            var content = statusCode == 200
                ? await ReadSuccessBodyAsync(response.Content, timeout.Token)
                : await DrainErrorBodyBestEffortAsync(response, timeout.Token, cancellationToken);
            var mediaType = response.Content.Headers.ContentType?.MediaType;
            var retryAfter = ParseRetryAfter(response.Headers.RetryAfter);

            return GlyphForgeResponseMapper.Map(
                new GlyphForgeResponse(response.StatusCode, mediaType, content, retryAfter));
        }
        catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            logger?.LogWarning("Glyph Forge request timed out.");
            return GlyphForgeResponseMapper.Map(
                new GlyphForgeResponse(null, null, null, failure: GlyphForgeResponseFailure.Timeout));
        }
        catch (Exception exception) when (
            exception is HttpRequestException
            or IOException
            or InvalidOperationException
            or OptionsValidationException)
        {
            logger?.LogWarning(
                "Glyph Forge communication failed with {ExceptionType}.",
                exception.GetType().Name);
            return GlyphForgeResponseMapper.Map(
                new GlyphForgeResponse(null, null, null, failure: GlyphForgeResponseFailure.Communication));
        }
    }

    private static async Task<byte[]?> DrainErrorBodyBestEffortAsync(
        HttpResponseMessage response,
        CancellationToken timeoutToken,
        CancellationToken callerToken)
    {
        try
        {
            await response.Content.CopyToAsync(Stream.Null, timeoutToken);
        }
        catch (OperationCanceledException) when (!callerToken.IsCancellationRequested)
        {
        }
        catch (HttpRequestException)
        {
        }
        catch (IOException)
        {
        }
        catch (InvalidOperationException)
        {
        }

        return null;
    }

    private static async Task<byte[]?> ReadSuccessBodyAsync(
        HttpContent content,
        CancellationToken cancellationToken)
    {
        if (content.Headers.ContentLength is > MaximumImageBytes)
        {
            return null;
        }

        using var destination = new BoundedMemoryStream(MaximumImageBytes);
        try
        {
            await content.CopyToAsync(destination, null, cancellationToken);
            return destination.ToArray();
        }
        catch (ResponseBodyTooLargeException)
        {
            return null;
        }
    }

    private sealed class BoundedMemoryStream(int maximumLength) : MemoryStream
    {
        public override void Write(byte[] buffer, int offset, int count)
        {
            EnsureCapacity(count);
            base.Write(buffer, offset, count);
        }

        public override Task WriteAsync(
            byte[] buffer,
            int offset,
            int count,
            CancellationToken cancellationToken)
        {
            EnsureCapacity(count);
            return base.WriteAsync(buffer, offset, count, cancellationToken);
        }

        public override ValueTask WriteAsync(
            ReadOnlyMemory<byte> buffer,
            CancellationToken cancellationToken = default)
        {
            EnsureCapacity(buffer.Length);
            return base.WriteAsync(buffer, cancellationToken);
        }

        private void EnsureCapacity(int count)
        {
            if (Length > maximumLength - count)
            {
                throw new ResponseBodyTooLargeException();
            }
        }
    }

    private sealed class ResponseBodyTooLargeException : Exception;

    private static Uri CreateRequestUri(Uri? baseAddress, string path)
    {
        var relativePath = path.TrimStart('/');
        if (baseAddress is null)
        {
            return new Uri(relativePath, UriKind.Relative);
        }

        var directoryBase = baseAddress.AbsoluteUri.EndsWith('/')
            ? baseAddress
            : new Uri($"{baseAddress.AbsoluteUri}/");
        return new Uri(directoryBase, relativePath);
    }

    private static int? ParseRetryAfter(System.Net.Http.Headers.RetryConditionHeaderValue? value)
    {
        if (value?.Delta is { } delay && delay >= TimeSpan.Zero && delay.TotalSeconds <= int.MaxValue)
        {
            return (int)delay.TotalSeconds;
        }

        if (value?.Date is { } date)
        {
            var seconds = (date - DateTimeOffset.UtcNow).TotalSeconds;
            if (seconds < 0)
            {
                return 0;
            }

            if (seconds <= int.MaxValue)
            {
                return (int)Math.Ceiling(seconds);
            }
        }

        return null;
    }
}
