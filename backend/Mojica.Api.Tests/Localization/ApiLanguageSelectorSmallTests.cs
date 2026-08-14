using System.Reflection;
using Mojica.Api.Localization;

namespace Mojica.Api.Tests.Localization;

public sealed class ApiLanguageSelectorSmallTests
{
    [Fact]
    public void ApiLanguageSelector_Select_WhenJapaneseIsRequested_ReturnsJapanese()
    {
        var language = ApiLanguageSelector.Select("ja");

        Assert.Equal(ApiLanguage.Japanese, language);
    }

    [Fact]
    public void ApiLanguageSelector_Select_WhenEnglishIsRequested_ReturnsEnglish()
    {
        var language = ApiLanguageSelector.Select("en");

        Assert.Equal(ApiLanguage.English, language);
    }

    [Fact]
    public void ApiLanguageSelector_Select_WhenLanguageIsOmitted_ReturnsJapanese()
    {
        var language = ApiLanguageSelector.Select(null);
        var parameter = typeof(ApiLanguageSelector)
            .GetMethod(nameof(ApiLanguageSelector.Select))!
            .GetParameters()
            .Single();
        var nullability = new NullabilityInfoContext().Create(parameter);

        Assert.Equal(ApiLanguage.Japanese, language);
        Assert.Equal(NullabilityState.Nullable, nullability.ReadState);
    }

    [Fact(Skip = "TODO: Implement when the API language selector exists.")]
    public void ApiLanguageSelector_Select_WhenLanguageIsUnsupported_ReturnsJapanese()
    {
        // ID: LOCALIZATION-LANGUAGE-04
        // Source: docs/v1/api/api.md §9 Language Selection; docs/v1/api/controllers.md §8 Language Selection.
        // Given: an unsupported language code (Theory candidate: "fr", "de", and an empty value)
        // When: the display language is selected
        // Then: Japanese is returned as the fallback language
        // Error: unsupported input must not escape into the public message lookup
        // Blocked by: feature/add-api-error-localization must define the language value and selector
        // Priority: High
    }
}
