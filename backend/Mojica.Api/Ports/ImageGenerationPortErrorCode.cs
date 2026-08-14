namespace Mojica.Api.Ports;

public sealed record ImageGenerationPortErrorCode
{
    public static ImageGenerationPortErrorCode RateLimited { get; } = new("RATE_LIMITED");
    public static ImageGenerationPortErrorCode Timeout { get; } = new("TIMEOUT");
    public static ImageGenerationPortErrorCode Unavailable { get; } = new("UNAVAILABLE");
    public static ImageGenerationPortErrorCode InvalidResponse { get; } = new("INVALID_RESPONSE");
    public static ImageGenerationPortErrorCode Failed { get; } = new("FAILED");

    private ImageGenerationPortErrorCode(string value)
    {
        Value = value;
    }

    public string Value { get; }
}
