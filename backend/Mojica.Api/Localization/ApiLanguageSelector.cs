namespace Mojica.Api.Localization;

public static class ApiLanguageSelector
{
    public static ApiLanguage Select(string languageCode)
    {
        return languageCode == "en"
            ? ApiLanguage.English
            : ApiLanguage.Japanese;
    }
}
