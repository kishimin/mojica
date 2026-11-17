using System.Net;
using Mojica.Api.Models;
using Mojica.Api.Ports;

namespace Mojica.Api.Infrastructure;

public sealed class GlyphForgeImageUploadAdapter(IHttpClientFactory httpClientFactory) : ImageUploadPort
{
    private const int MaximumImageBytes = 10 * 1024 * 1024;

    public async Task<ImageGenerationPortResult> GenerateAsync(
        Stream image, ImageGenerationRequest options, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient("GlyphForge");
        using var timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        if (client.Timeout != Timeout.InfiniteTimeSpan)
        {
            timeout.CancelAfter(client.Timeout);
        }

        using var form = new MultipartFormDataContent();
        form.Add(new StreamContent(image), "frame_image", "image");
        form.Add(new StringContent(options.ForegroundCharacter.Value), "inner_text");
        form.Add(new StringContent(options.BackgroundCharacter.Value), "outer_text");
        form.Add(new StringContent(string.Join(",", options.ForegroundColor.ToRgb().ToArray())), "inner_color");
        form.Add(new StringContent(string.Join(",", options.BackgroundColor.ToRgb().ToArray())), "outer_color");

        try
        {
            var baseAddress = client.BaseAddress;
            var uri = baseAddress is null
                ? new Uri("images/frame-file", UriKind.Relative)
                : new Uri(new Uri(baseAddress.AbsoluteUri.TrimEnd('/') + "/"), "images/frame-file");
            using var request = new HttpRequestMessage(HttpMethod.Post, uri) { Content = form };
            using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, timeout.Token);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                var codes = response.Headers.TryGetValues("X-Glyph-Forge-Error", out var values)
                    ? values.Take(2).ToArray()
                    : [];
                var code = codes.Length == 1 ? codes[0] : null;
                var retryAfter = ParseRetryAfter(response.Headers.RetryAfter);
                try
                {
                    await response.Content.CopyToAsync(Stream.Null, timeout.Token);
                }
                catch (Exception error) when (error is HttpRequestException or IOException or InvalidOperationException
                    || error is OperationCanceledException && !cancellationToken.IsCancellationRequested)
                {
                }
                return Failure(response.StatusCode switch
                {
                    HttpStatusCode.UnprocessableEntity when code == "INVALID_IMAGE" => ImageGenerationPortErrorCode.InvalidImage,
                    HttpStatusCode.UnprocessableEntity when code == "OUTPUT_SIZE_EXCEEDED" => ImageGenerationPortErrorCode.ImageOutputSizeExceeded,
                    HttpStatusCode.UnprocessableEntity => ImageGenerationPortErrorCode.ImageRequestInvalid,
                    HttpStatusCode.RequestEntityTooLarge => ImageGenerationPortErrorCode.RequestTooLarge,
                    HttpStatusCode.TooManyRequests => ImageGenerationPortErrorCode.RateLimited,
                    HttpStatusCode.ServiceUnavailable => ImageGenerationPortErrorCode.Unavailable,
                    _ => ImageGenerationPortErrorCode.Failed,
                }, retryAfter);
            }

            if (!string.Equals(response.Content.Headers.ContentType?.MediaType, "image/png", StringComparison.OrdinalIgnoreCase)
                || response.Content.Headers.ContentLength is > MaximumImageBytes)
            {
                return Failure(ImageGenerationPortErrorCode.InvalidResponse);
            }

            await using var content = await response.Content.ReadAsStreamAsync(timeout.Token);
            using var destination = new MemoryStream();
            var buffer = new byte[81920];
            int count;
            while ((count = await content.ReadAsync(buffer, timeout.Token)) > 0)
            {
                if (destination.Length > MaximumImageBytes - count)
                {
                    return Failure(ImageGenerationPortErrorCode.InvalidResponse);
                }
                await destination.WriteAsync(buffer.AsMemory(0, count), timeout.Token);
            }

            return destination.Length == 0
                ? Failure(ImageGenerationPortErrorCode.InvalidResponse)
                : ImageGenerationPortResult.Success(new GeneratedImageData(destination.ToArray(), "image/png"));
        }
        catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            return Failure(ImageGenerationPortErrorCode.Timeout);
        }
        catch (Exception error) when (error is HttpRequestException or IOException or InvalidOperationException)
        {
            return Failure(ImageGenerationPortErrorCode.Unavailable);
        }
    }

    private static int? ParseRetryAfter(System.Net.Http.Headers.RetryConditionHeaderValue? value)
    {
        var delay = value?.Delta ?? (value?.Date is { } date ? date - DateTimeOffset.UtcNow : null);
        if (delay is null || delay.Value.TotalSeconds > int.MaxValue)
        {
            return null;
        }
        return (int)Math.Ceiling(Math.Max(0, delay.Value.TotalSeconds));
    }

    private static ImageGenerationPortResult Failure(ImageGenerationPortErrorCode code, int? retryAfter = null) =>
        ImageGenerationPortResult.Failure(new ImageGenerationPortError(code, retryAfter));
}
