using Microsoft.Extensions.Options;

namespace Mojica.Api.Infrastructure;

public sealed class RateLimitOptions
{
    public const string SectionName = "RateLimit";

    public int PermitLimit { get; set; }

    public TimeSpan Window { get; set; }

    public int QueueLimit { get; set; }
}

public sealed class RateLimitOptionsValidator : IValidateOptions<RateLimitOptions>
{
    public ValidateOptionsResult Validate(string? name, RateLimitOptions options)
    {
        if (options.PermitLimit <= 0)
        {
            return ValidateOptionsResult.Fail("Rate limit permit limit must be positive.");
        }

        if (options.Window <= TimeSpan.Zero)
        {
            return ValidateOptionsResult.Fail("Rate limit window must be positive.");
        }

        if (options.QueueLimit < 0)
        {
            return ValidateOptionsResult.Fail("Rate limit queue limit must not be negative.");
        }

        return ValidateOptionsResult.Success;
    }
}
