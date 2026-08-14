using Mojica.Api.Models;

namespace Mojica.Api.Localization;

public static class ApiErrorMessageProvider
{
    public static string GetPublicMessage(ApiLanguage language, string code)
    {
        return code switch
        {
            "BAD_REQUEST" => Localize(language,
                "リクエストの形式が正しくありません。",
                "The request format is invalid."),
            "VALIDATION_ERROR" => Localize(language,
                "入力内容に誤りがあります。",
                "The input contains validation errors."),
            "IMAGE_SIZE_LIMIT_EXCEEDED" => Localize(language,
                "生成される画像がサイズ上限を超えます。入力する文字を減らしてください。",
                "The generated image would exceed the size limit. Reduce the input text."),
            "RATE_LIMIT_EXCEEDED" => Localize(language,
                "リクエスト回数の上限に達しました。時間をおいて再度お試しください。",
                "The request limit has been exceeded. Please try again later."),
            "INTERNAL_SERVER_ERROR" => Localize(language,
                "画像生成中に予期しないエラーが発生しました。",
                "An unexpected error occurred while generating the image."),
            "IMAGE_GENERATION_FAILED" => Localize(language,
                "画像の生成に失敗しました。時間をおいて再度お試しください。",
                "Image generation failed. Please try again later."),
            "IMAGE_GENERATION_TIMEOUT" => Localize(language,
                "画像の生成に時間がかかっています。時間をおいて再度お試しください。",
                "Image generation is taking too long. Please try again later."),
            _ => throw new ArgumentOutOfRangeException(
                nameof(code),
                code,
                "Unsupported public API error code."),
        };
    }

    public static string GetValidationMessage(
        ApiLanguage language,
        ModelValidationReason reason,
        string target)
    {
        return (reason.Value, target) switch
        {
            ("REQUIRED", "type") => Localize(language, "画像の種類は必須です。", "The type field is required."),
            ("REQUIRED", "text") => Localize(language, "描画する文字列は必須です。", "The text field is required."),
            ("REQUIRED", "foregroundCharacter") => Localize(language, "描画に使う文字は必須です。", "The foreground character field is required."),
            ("REQUIRED", "foregroundColor") => Localize(language, "描画に使う文字の色は必須です。", "The foreground color field is required."),
            ("REQUIRED", "backgroundCharacter") => Localize(language, "敷き詰める文字は必須です。", "The background character field is required."),
            ("REQUIRED", "backgroundColor") => Localize(language, "敷き詰める文字の色は必須です。", "The background color field is required."),
            ("LENGTH_OUT_OF_RANGE", "text") => Localize(language, "描画する文字列は64文字以内で入力してください。", "The text must be 64 characters or fewer."),
            ("LENGTH_OUT_OF_RANGE", "foregroundCharacter") => Localize(language, "描画に使う文字は128文字以内で入力してください。", "The foreground character must be 128 characters or fewer."),
            ("LENGTH_OUT_OF_RANGE", "backgroundCharacter") => Localize(language, "敷き詰める文字は128文字以内で入力してください。", "The background character must be 128 characters or fewer."),
            ("NOT_BLANK", "text") => Localize(language, "描画する文字列には空白以外の文字を入力してください。", "The text must contain a non-whitespace character."),
            ("CONTROL_CHARACTER", "text") => Localize(language, "描画する文字列に制御文字は使用できません。", "The text must not contain control characters."),
            ("CONTROL_CHARACTER", "foregroundCharacter") => Localize(language, "描画に使う文字に制御文字は使用できません。", "The foreground character must not contain control characters."),
            ("CONTROL_CHARACTER", "backgroundCharacter") => Localize(language, "敷き詰める文字に制御文字は使用できません。", "The background character must not contain control characters."),
            ("INVALID_HEX_COLOR", "foregroundColor") => Localize(language, "HEXカラー形式（#RRGGBB）で指定してください。", "The value must be specified in HEX color format (#RRGGBB)."),
            ("INVALID_HEX_COLOR", "backgroundColor") => Localize(language, "HEXカラー形式（#RRGGBB）で指定してください。", "The value must be specified in HEX color format (#RRGGBB)."),
            ("UNSUPPORTED_IMAGE_TYPE", "type") => Localize(language, "standard、x-background、x-iconのいずれかを指定してください。", "The value must be one of: standard, x-background, or x-icon."),
            ("VISIBLE_CHARACTER_REQUIRED", "foregroundCharacter") => Localize(language, "描画に使う文字または敷き詰める文字のどちらかに、表示可能な文字を入力してください。", "Either the foreground or background characters must contain at least one visible character."),
            ("VISIBLE_CHARACTER_REQUIRED", "backgroundCharacter") => Localize(language, "描画に使う文字または敷き詰める文字のどちらかに、表示可能な文字を入力してください。", "Either the foreground or background characters must contain at least one visible character."),
            _ => throw new ArgumentException(
                "Unsupported validation reason and target combination."),
        };
    }

    private static string Localize(
        ApiLanguage language,
        string japanese,
        string english)
    {
        return language switch
        {
            ApiLanguage.Japanese => japanese,
            ApiLanguage.English => english,
            _ => throw new ArgumentOutOfRangeException(nameof(language)),
        };
    }
}
