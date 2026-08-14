using Mojica.Api.Localization;

namespace Mojica.Api.Tests.Localization;

public sealed class ApiErrorMessageProviderSmallTests
{
    public static TheoryData<string, string> JapanesePublicMessages => new()
    {
        { "BAD_REQUEST", "リクエストの形式が正しくありません。" },
        { "VALIDATION_ERROR", "入力内容に誤りがあります。" },
        { "IMAGE_SIZE_LIMIT_EXCEEDED", "生成される画像がサイズ上限を超えます。入力する文字を減らしてください。" },
        { "RATE_LIMIT_EXCEEDED", "リクエスト回数の上限に達しました。時間をおいて再度お試しください。" },
        { "INTERNAL_SERVER_ERROR", "画像生成中に予期しないエラーが発生しました。" },
        { "IMAGE_GENERATION_FAILED", "画像の生成に失敗しました。時間をおいて再度お試しください。" },
        { "IMAGE_GENERATION_TIMEOUT", "画像の生成に時間がかかっています。時間をおいて再度お試しください。" },
    };

    public static TheoryData<string, string> EnglishPublicMessages => new()
    {
        { "BAD_REQUEST", "The request format is invalid." },
        { "VALIDATION_ERROR", "The input contains validation errors." },
        { "IMAGE_SIZE_LIMIT_EXCEEDED", "The generated image would exceed the size limit. Reduce the input text." },
        { "RATE_LIMIT_EXCEEDED", "The request limit has been exceeded. Please try again later." },
        { "INTERNAL_SERVER_ERROR", "An unexpected error occurred while generating the image." },
        { "IMAGE_GENERATION_FAILED", "Image generation failed. Please try again later." },
        { "IMAGE_GENERATION_TIMEOUT", "Image generation is taking too long. Please try again later." },
    };

    [Theory]
    [MemberData(nameof(JapanesePublicMessages))]
    public void ApiErrorMessageProvider_GetPublicMessage_WhenLanguageIsJapanese_ReturnsDocumentedMessage(
        string code,
        string expectedMessage)
    {
        var message = ApiErrorMessageProvider.GetPublicMessage(
            ApiLanguage.Japanese,
            code);

        Assert.Equal(expectedMessage, message);
    }

    [Theory]
    [MemberData(nameof(EnglishPublicMessages))]
    public void ApiErrorMessageProvider_GetPublicMessage_WhenLanguageIsEnglish_ReturnsDocumentedMessage(
        string code,
        string expectedMessage)
    {
        var message = ApiErrorMessageProvider.GetPublicMessage(
            ApiLanguage.English,
            code);

        Assert.Equal(expectedMessage, message);
    }

    [Fact(Skip = "TODO: Implement when the API error message provider exists.")]
    public void ApiErrorMessageProvider_GetValidationMessage_WhenLanguageIsJapanese_ReturnsMessageForReasonAndTarget()
    {
        // ID: LOCALIZATION-VALIDATION-01
        // Source: docs/v1/api/api.md §9 and §11 422 Unprocessable Entity; ADR-0022.
        // Given: Japanese, a documented ModelValidationReason, and the request target that owns the field context (Theory candidate for every supported reason-target pair)
        // When: the validation detail message is resolved
        // Then: the exact documented Japanese message for that validation condition and request attribute is returned
        // Error: the reason remains language-independent while the target supplies context such as text, foregroundCharacter, or backgroundColor
        // Blocked by: feature/add-api-error-localization must define the validation message lookup boundary
        // Priority: High
    }

    [Fact(Skip = "TODO: Implement when the API error message provider exists.")]
    public void ApiErrorMessageProvider_GetValidationMessage_WhenLanguageIsEnglish_ReturnsMessageForReasonAndTarget()
    {
        // ID: LOCALIZATION-VALIDATION-02
        // Source: docs/v1/api/api.md §9 and §11 422 Unprocessable Entity; ADR-0022.
        // Given: English, a documented ModelValidationReason, and the request target that owns the field context (Theory candidate for every supported reason-target pair)
        // When: the validation detail message is resolved
        // Then: the exact documented English message for that validation condition and request attribute is returned
        // Error: the reason remains language-independent while the target supplies context such as text, foregroundCharacter, or backgroundColor
        // Blocked by: feature/add-api-error-localization must define the validation message lookup boundary
        // Priority: High
    }
}
