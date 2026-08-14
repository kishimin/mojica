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
}
