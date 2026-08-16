using System.Threading.RateLimiting;

namespace Mojica.Api.Infrastructure;

public static class ImageGenerationRateLimiterPolicy
{
    public const string PolicyName = "ImageGeneration";

    public static RateLimiter CreateLimiter(RateLimitOptions options)
    {
        return new FixedWindowRateLimiter(new FixedWindowRateLimiterOptions
        {
            PermitLimit = options.PermitLimit,
            Window = options.Window,
            QueueLimit = options.QueueLimit,
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            AutoReplenishment = true,
        });
    }
}
