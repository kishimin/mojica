namespace Mojica.Api.Ports;

public sealed record ImageGenerationPortError
{
    public ImageGenerationPortError(
        ImageGenerationPortErrorCode errorCode,
        int? retryAfter = null,
        string? details = null)
    {
        ArgumentNullException.ThrowIfNull(errorCode);

        ErrorCode = errorCode;
        RetryAfter = retryAfter;
        Details = details;
    }

    public string Code => ErrorCode.Value;

    public ImageGenerationPortErrorCode ErrorCode { get; }

    public int? RetryAfter { get; }

    public string? Details { get; }
}
