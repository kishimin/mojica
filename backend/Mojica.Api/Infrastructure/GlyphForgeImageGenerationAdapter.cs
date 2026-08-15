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

        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, mapped.Path.TrimStart('/'));
        using var timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        try
        {
            var client = httpClientFactory.CreateClient("GlyphForge");
            if (client.Timeout != Timeout.InfiniteTimeSpan)
            {
                timeout.CancelAfter(client.Timeout);
            }

            httpRequest.Content = JsonContent.Create(mapped.Payload);
            using var response = await client.SendAsync(
                httpRequest,
                HttpCompletionOption.ResponseHeadersRead,
                timeout.Token);
            var statusCode = (int)response.StatusCode;
            var content = statusCode == 200
                ? await response.Content.ReadAsByteArrayAsync(timeout.Token)
                : await DrainErrorBodyAsync(response, timeout.Token);
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

    private static async Task<byte[]?> DrainErrorBodyAsync(
        HttpResponseMessage response,
        CancellationToken cancellationToken)
    {
        await response.Content.CopyToAsync(Stream.Null, cancellationToken);
        return null;
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
