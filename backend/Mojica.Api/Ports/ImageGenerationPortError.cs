namespace Mojica.Api.Ports;

public sealed record ImageGenerationPortError
{
    public ImageGenerationPortError(
        ImageGenerationPortErrorCode errorCode,
        int? retryAfter = null)
    {
        ErrorCode = errorCode;
        RetryAfter = retryAfter;
    }

    public string Code => ErrorCode.Value;

    public ImageGenerationPortErrorCode ErrorCode { get; }

    public int? RetryAfter { get; }

    public string? Details => null;
}
