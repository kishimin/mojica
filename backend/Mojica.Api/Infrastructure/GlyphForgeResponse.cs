using System.Net;
using Mojica.Api.Models;

namespace Mojica.Api.Infrastructure;

public sealed record GlyphForgeResponse(
    HttpStatusCode? StatusCode,
    string? MediaType,
    byte[]? Content,
    int? RetryAfter = null,
    GlyphForgeResponseFailure? Failure = null)
{
    public bool Equals(GlyphForgeResponse? other)
    {
        return other is not null
            && StatusCode == other.StatusCode
            && MediaType == other.MediaType
            && RetryAfter == other.RetryAfter
            && Failure == other.Failure
            && (Content is null
                ? other.Content is null
                : other.Content is not null && ValueEquality.ContentEquals(Content, other.Content));
    }

    public override int GetHashCode()
    {
        return ValueEquality.GetStableHashCode(
            StatusCode?.ToString() ?? string.Empty,
            MediaType ?? string.Empty,
            RetryAfter?.ToString() ?? string.Empty,
            Failure?.ToString() ?? string.Empty);
    }
}

public enum GlyphForgeResponseFailure
{
    Timeout,
    Communication,
    Failed,
}
