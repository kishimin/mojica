namespace Mojica.Api.Contracts;

public static class PublicApiErrorCode
{
    public const string BadRequest = "BAD_REQUEST";
    public const string ValidationError = "VALIDATION_ERROR";
    public const string ImageSizeLimitExceeded = "IMAGE_SIZE_LIMIT_EXCEEDED";
    public const string RateLimitExceeded = "RATE_LIMIT_EXCEEDED";
    public const string InternalServerError = "INTERNAL_SERVER_ERROR";
    public const string ImageGenerationFailed = "IMAGE_GENERATION_FAILED";
    public const string ImageGenerationTimeout = "IMAGE_GENERATION_TIMEOUT";
}
