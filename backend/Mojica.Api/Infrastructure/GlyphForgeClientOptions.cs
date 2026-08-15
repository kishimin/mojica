using Microsoft.Extensions.Options;

namespace Mojica.Api.Infrastructure;

public sealed class GlyphForgeClientOptions
{
    public const string SectionName = "GlyphForge";

    public Uri? BaseUrl { get; set; }

    public TimeSpan Timeout { get; set; }
}

public sealed class GlyphForgeClientOptionsValidator : IValidateOptions<GlyphForgeClientOptions>
{
    private static readonly TimeSpan MaximumHttpClientTimeout =
        TimeSpan.FromMilliseconds(int.MaxValue);

    public ValidateOptionsResult Validate(string? name, GlyphForgeClientOptions options)
    {
        if (options.BaseUrl is null)
        {
            return ValidateOptionsResult.Fail("Glyph Forge base URL is required.");
        }

        if (!options.BaseUrl.IsAbsoluteUri
            || (options.BaseUrl.Scheme != Uri.UriSchemeHttp && options.BaseUrl.Scheme != Uri.UriSchemeHttps))
        {
            return ValidateOptionsResult.Fail("Glyph Forge base URL must be an absolute HTTP URL.");
        }

        if (options.Timeout <= TimeSpan.Zero)
        {
            return ValidateOptionsResult.Fail("Glyph Forge timeout must be positive.");
        }

        if (options.Timeout > MaximumHttpClientTimeout)
        {
            return ValidateOptionsResult.Fail("Glyph Forge timeout exceeds the HttpClient maximum.");
        }

        return ValidateOptionsResult.Success;
    }
}
