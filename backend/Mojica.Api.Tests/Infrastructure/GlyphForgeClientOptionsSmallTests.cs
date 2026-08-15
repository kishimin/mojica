using Xunit;
using Microsoft.Extensions.Options;
using Mojica.Api.Infrastructure;

namespace Mojica.Api.Tests.Infrastructure;

public sealed class GlyphForgeClientOptionsSmallTests
{
    [Fact]
    public void Validate_WhenBaseUrlAndTimeoutAreValid_AcceptsConfiguration()
    {
        var options = new GlyphForgeClientOptions
        {
            BaseUrl = new Uri("https://glyph-forge.example/"),
            Timeout = TimeSpan.FromSeconds(20)
        };

        var result = new GlyphForgeClientOptionsValidator().Validate(null, options);

        Assert.Equal(ValidateOptionsResult.Success, result);
    }

    [Fact]
    public void Validate_WhenBaseUrlIsMissing_RejectsConfiguration()
    {
        var options = new GlyphForgeClientOptions
        {
            Timeout = TimeSpan.FromSeconds(20)
        };

        var result = new GlyphForgeClientOptionsValidator().Validate(null, options);

        Assert.False(result.Succeeded);
        Assert.Contains("base URL is required", result.FailureMessage);
    }

    [Fact]
    public void Validate_WhenBaseUrlIsNotAbsoluteHttpUrl_RejectsConfiguration()
    {
        var options = new GlyphForgeClientOptions
        {
            BaseUrl = new Uri("/images", UriKind.Relative),
            Timeout = TimeSpan.FromSeconds(20)
        };

        var result = new GlyphForgeClientOptionsValidator().Validate(null, options);

        Assert.False(result.Succeeded);
        Assert.Contains("absolute HTTP URL", result.FailureMessage);
    }

    [Fact]
    public void Validate_WhenTimeoutIsNotPositive_RejectsConfiguration()
    {
        var options = new GlyphForgeClientOptions
        {
            BaseUrl = new Uri("https://glyph-forge.example/"),
            Timeout = TimeSpan.Zero
        };

        var result = new GlyphForgeClientOptionsValidator().Validate(null, options);

        Assert.False(result.Succeeded);
        Assert.Contains("timeout must be positive", result.FailureMessage);
    }

    [Fact]
    public void Validate_WhenTimeoutExceedsHttpClientMaximum_RejectsConfiguration()
    {
        var options = new GlyphForgeClientOptions
        {
            BaseUrl = new Uri("https://glyph-forge.example/"),
            Timeout = TimeSpan.FromMilliseconds(int.MaxValue + 1L)
        };

        var result = new GlyphForgeClientOptionsValidator().Validate(null, options);

        Assert.False(result.Succeeded);
        Assert.Contains("maximum", result.FailureMessage);
    }

    [Fact]
    public void Validate_WhenTimeoutIsThirtyFiveSeconds_AcceptsProviderLimitConfiguration()
    {
        var options = new GlyphForgeClientOptions
        {
            BaseUrl = new Uri("https://glyph-forge.example/"),
            Timeout = TimeSpan.FromSeconds(35)
        };

        var result = new GlyphForgeClientOptionsValidator().Validate(null, options);

        Assert.Equal(ValidateOptionsResult.Success, result);
    }
}
