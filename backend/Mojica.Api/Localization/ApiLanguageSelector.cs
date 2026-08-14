namespace Mojica.Api.Localization;

public static class ApiLanguageSelector
{
    public static ApiLanguage Select(string? languageCode)
    {
        return languageCode switch
        {
            "ja" => ApiLanguage.Japanese,
            "en" => ApiLanguage.English,
            _ => ApiLanguage.Japanese,
        };
    }
}
