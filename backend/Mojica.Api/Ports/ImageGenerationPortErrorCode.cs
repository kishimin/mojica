namespace Mojica.Api.Ports;

public sealed record ImageGenerationPortErrorCode
{
    public static ImageGenerationPortErrorCode RateLimited { get; } = new("RATE_LIMITED");
    public static ImageGenerationPortErrorCode Timeout { get; } = new("TIMEOUT");
    public static ImageGenerationPortErrorCode Unavailable { get; } = new("UNAVAILABLE");
    public static ImageGenerationPortErrorCode InvalidResponse { get; } = new("INVALID_RESPONSE");
    public static ImageGenerationPortErrorCode OutputSizeExceeded { get; } = new("OUTPUT_SIZE_EXCEEDED");
    public static ImageGenerationPortErrorCode InvalidImage { get; } = new("INVALID_IMAGE");
    public static ImageGenerationPortErrorCode ImageOutputSizeExceeded { get; } = new("IMAGE_OUTPUT_SIZE_EXCEEDED");
    public static ImageGenerationPortErrorCode ImageRequestInvalid { get; } = new("IMAGE_REQUEST_INVALID");
    public static ImageGenerationPortErrorCode RequestTooLarge { get; } = new("REQUEST_TOO_LARGE");
    public static ImageGenerationPortErrorCode Failed { get; } = new("FAILED");

    private ImageGenerationPortErrorCode(string value)
    {
        Value = value;
    }

    public string Value { get; }
}
