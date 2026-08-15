using System.Net.Http.Json;
using Mojica.Api.Models;
using Mojica.Api.Ports;

namespace Mojica.Api.Infrastructure;

public sealed class GlyphForgeImageGenerationAdapter(
    IHttpClientFactory httpClientFactory) : ImageGenerationPort
{
    public async Task<ImageGenerationPortResult> GenerateAsync(
        ImageGenerationRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        try
        {
            var mapped = GlyphForgeRequestMapper.Map(request);
            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, mapped.Path)
            {
                Content = JsonContent.Create(mapped.Payload),
            };

            var client = httpClientFactory.CreateClient("GlyphForge");
            using var response = await client.SendAsync(
                httpRequest,
                HttpCompletionOption.ResponseHeadersRead,
                cancellationToken);
            var statusCode = (int)response.StatusCode;
            var content = statusCode is >= 200 and <= 299
                ? await response.Content.ReadAsByteArrayAsync(cancellationToken)
                : null;
            var mediaType = response.Content.Headers.ContentType?.MediaType;
            var retryAfter = ParseRetryAfter(response.Headers.RetryAfter);

            return GlyphForgeResponseMapper.Map(
                new GlyphForgeResponse(response.StatusCode, mediaType, content, retryAfter));
        }
        catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            return GlyphForgeResponseMapper.Map(
                new GlyphForgeResponse(null, null, null, Failure: GlyphForgeResponseFailure.Timeout));
        }
        catch (HttpRequestException)
        {
            return GlyphForgeResponseMapper.Map(
                new GlyphForgeResponse(null, null, null, Failure: GlyphForgeResponseFailure.Communication));
        }
        catch (InvalidOperationException)
        {
            return GlyphForgeResponseMapper.Map(
                new GlyphForgeResponse(null, null, null, Failure: GlyphForgeResponseFailure.Failed));
        }
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
            if (seconds >= 0 && seconds <= int.MaxValue)
            {
                return (int)Math.Ceiling(seconds);
            }
        }

        return null;
    }
}
