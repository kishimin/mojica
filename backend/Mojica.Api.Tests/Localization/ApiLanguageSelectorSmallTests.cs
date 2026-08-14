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

        Assert.Equal(ApiLanguage.Japanese, language);
    }

    [Theory]
    [InlineData("fr")]
    [InlineData("de")]
    [InlineData("")]
    public void ApiLanguageSelector_Select_WhenLanguageIsUnsupported_ReturnsJapanese(
        string languageCode)
    {
        var language = ApiLanguageSelector.Select(languageCode);

        Assert.Equal(ApiLanguage.Japanese, language);
    }
}
