using System.Net;

namespace Mojica.Api.Infrastructure;

public sealed record GlyphForgeResponse(
    HttpStatusCode? StatusCode,
    string? MediaType,
    byte[]? Content,
    int? RetryAfter = null,
    GlyphForgeResponseFailure? Failure = null);

public enum GlyphForgeResponseFailure
{
    Timeout,
    Communication,
}
