using Mojica.Api.Models;

namespace Mojica.Api.Localization;

public static class ApiErrorMessageProvider
{
    public static string GetPublicMessage(ApiLanguage language, string code)
    {
        return language switch
        {
            ApiLanguage.Japanese => code switch
            {
                "BAD_REQUEST" => "リクエストの形式が正しくありません。",
                "VALIDATION_ERROR" => "入力内容に誤りがあります。",
                "IMAGE_SIZE_LIMIT_EXCEEDED" => "生成される画像がサイズ上限を超えます。入力する文字を減らしてください。",
                "RATE_LIMIT_EXCEEDED" => "リクエスト回数の上限に達しました。時間をおいて再度お試しください。",
                "INTERNAL_SERVER_ERROR" => "画像生成中に予期しないエラーが発生しました。",
                "IMAGE_GENERATION_FAILED" => "画像の生成に失敗しました。時間をおいて再度お試しください。",
                "IMAGE_GENERATION_TIMEOUT" => "画像の生成に時間がかかっています。時間をおいて再度お試しください。",
                _ => throw new ArgumentOutOfRangeException(
                    nameof(code),
                    code,
                    "Unsupported public API error code."),
            },
            ApiLanguage.English => code switch
            {
                "BAD_REQUEST" => "The request format is invalid.",
                "VALIDATION_ERROR" => "The input contains validation errors.",
                "IMAGE_SIZE_LIMIT_EXCEEDED" => "The generated image would exceed the size limit. Reduce the input text.",
                "RATE_LIMIT_EXCEEDED" => "The request limit has been exceeded. Please try again later.",
                "INTERNAL_SERVER_ERROR" => "An unexpected error occurred while generating the image.",
                "IMAGE_GENERATION_FAILED" => "Image generation failed. Please try again later.",
                "IMAGE_GENERATION_TIMEOUT" => "Image generation is taking too long. Please try again later.",
                _ => throw new ArgumentOutOfRangeException(
                    nameof(code),
                    code,
                    "Unsupported public API error code."),
            },
            _ => throw new ArgumentOutOfRangeException(nameof(language)),
        };
    }

    public static string GetValidationMessage(
        ApiLanguage language,
        ModelValidationReason reason,
        string target)
    {
        return language switch
        {
            ApiLanguage.Japanese => (reason.Value, target) switch
            {
                ("REQUIRED", "type") => "画像の種類は必須です。",
                ("REQUIRED", "text") => "描画する文字列は必須です。",
                ("REQUIRED", "foregroundCharacter") => "描画に使う文字は必須です。",
                ("REQUIRED", "foregroundColor") => "描画に使う文字の色は必須です。",
                ("REQUIRED", "backgroundCharacter") => "敷き詰める文字は必須です。",
                ("REQUIRED", "backgroundColor") => "敷き詰める文字の色は必須です。",
                ("LENGTH_OUT_OF_RANGE", "text") => "描画する文字列は64文字以内で入力してください。",
                ("LENGTH_OUT_OF_RANGE", "foregroundCharacter") => "描画に使う文字は128文字以内で入力してください。",
                ("LENGTH_OUT_OF_RANGE", "backgroundCharacter") => "敷き詰める文字は128文字以内で入力してください。",
                ("NOT_BLANK", "text") => "描画する文字列には空白以外の文字を入力してください。",
                ("CONTROL_CHARACTER", "text") => "描画する文字列に制御文字は使用できません。",
                ("CONTROL_CHARACTER", "foregroundCharacter") => "描画に使う文字に制御文字は使用できません。",
                ("CONTROL_CHARACTER", "backgroundCharacter") => "敷き詰める文字に制御文字は使用できません。",
                ("INVALID_HEX_COLOR", "foregroundColor") => "HEXカラー形式（#RRGGBB）で指定してください。",
                ("INVALID_HEX_COLOR", "backgroundColor") => "HEXカラー形式（#RRGGBB）で指定してください。",
                ("UNSUPPORTED_IMAGE_TYPE", "type") => "standard、x-background、x-iconのいずれかを指定してください。",
                ("VISIBLE_CHARACTER_REQUIRED", "foregroundCharacter") => "描画に使う文字または敷き詰める文字のどちらかに、表示可能な文字を入力してください。",
                ("VISIBLE_CHARACTER_REQUIRED", "backgroundCharacter") => "描画に使う文字または敷き詰める文字のどちらかに、表示可能な文字を入力してください。",
                _ => throw new ArgumentException(
                    "Unsupported validation reason and target combination."),
            },
            ApiLanguage.English => (reason.Value, target) switch
            {
                ("REQUIRED", "type") => "The type field is required.",
                ("REQUIRED", "text") => "The text field is required.",
                ("REQUIRED", "foregroundCharacter") => "The foreground character field is required.",
                ("REQUIRED", "foregroundColor") => "The foreground color field is required.",
                ("REQUIRED", "backgroundCharacter") => "The background character field is required.",
                ("REQUIRED", "backgroundColor") => "The background color field is required.",
                ("LENGTH_OUT_OF_RANGE", "text") => "The text must be 64 characters or fewer.",
                ("LENGTH_OUT_OF_RANGE", "foregroundCharacter") => "The foreground character must be 128 characters or fewer.",
                ("LENGTH_OUT_OF_RANGE", "backgroundCharacter") => "The background character must be 128 characters or fewer.",
                ("NOT_BLANK", "text") => "The text must contain a non-whitespace character.",
                ("CONTROL_CHARACTER", "text") => "The text must not contain control characters.",
                ("CONTROL_CHARACTER", "foregroundCharacter") => "The foreground character must not contain control characters.",
                ("CONTROL_CHARACTER", "backgroundCharacter") => "The background character must not contain control characters.",
                ("INVALID_HEX_COLOR", "foregroundColor") => "The value must be specified in HEX color format (#RRGGBB).",
                ("INVALID_HEX_COLOR", "backgroundColor") => "The value must be specified in HEX color format (#RRGGBB).",
                ("UNSUPPORTED_IMAGE_TYPE", "type") => "The value must be one of: standard, x-background, or x-icon.",
                ("VISIBLE_CHARACTER_REQUIRED", "foregroundCharacter") => "Either the foreground or background characters must contain at least one visible character.",
                ("VISIBLE_CHARACTER_REQUIRED", "backgroundCharacter") => "Either the foreground or background characters must contain at least one visible character.",
                _ => throw new ArgumentException(
                    "Unsupported validation reason and target combination."),
            },
            _ => throw new ArgumentOutOfRangeException(nameof(language)),
        };
    }
}
