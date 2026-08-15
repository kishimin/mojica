using System.Net;
using Mojica.Api.Models;

namespace Mojica.Api.Infrastructure;

public sealed record GlyphForgeResponse
{
    public GlyphForgeResponse(
        HttpStatusCode? statusCode,
        string? mediaType,
        byte[]? content,
        int? retryAfter = null,
        GlyphForgeResponseFailure? failure = null)
    {
        if (statusCode is not null && failure is not null)
        {
            throw new ArgumentException(
                "A response cannot contain both an HTTP status and a transport failure.");
        }

        StatusCode = statusCode;
        MediaType = mediaType;
        Content = content;
        RetryAfter = retryAfter;
        Failure = failure;
    }

    public HttpStatusCode? StatusCode { get; }

    public string? MediaType { get; }

    public byte[]? Content { get; }

    public int? RetryAfter { get; }

    public GlyphForgeResponseFailure? Failure { get; }

    public bool Equals(GlyphForgeResponse? other)
    {
        return other is not null
            && StatusCode == other.StatusCode
            && MediaType == other.MediaType
            && RetryAfter == other.RetryAfter
            && Failure == other.Failure
            && ValueEquality.ContentEquals(Content, other.Content);
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
