using System.Net;
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

        var mapped = GlyphForgeRequestMapper.Map(request);
        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, mapped.Path)
        {
            Content = JsonContent.Create(mapped.Payload),
        };

        try
        {
            var client = httpClientFactory.CreateClient("GlyphForge");
            using var response = await client.SendAsync(
                httpRequest,
                HttpCompletionOption.ResponseHeadersRead,
                cancellationToken);
            var content = await response.Content.ReadAsByteArrayAsync(cancellationToken);
            var mediaType = response.Content.Headers.ContentType?.MediaType;
            var retryAfter = ParseRetryAfter(response.Headers.RetryAfter?.Delta);

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
    }

    private static int? ParseRetryAfter(TimeSpan? value)
    {
        return value is { } delay && delay >= TimeSpan.Zero && delay.TotalSeconds <= int.MaxValue
            ? (int)delay.TotalSeconds
            : null;
    }
}
