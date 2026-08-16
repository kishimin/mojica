using Mojica.Api.Localization;
using Mojica.Api.Models;

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

    public static TheoryData<ModelValidationReason, string, string> JapaneseValidationMessages => new()
    {
        { ModelValidationReason.Required, "type", "画像の種類は必須です。" },
        { ModelValidationReason.Required, "text", "描画する文字列は必須です。" },
        { ModelValidationReason.Required, "foregroundCharacter", "描画に使う文字は必須です。" },
        { ModelValidationReason.Required, "foregroundColor", "描画に使う文字の色は必須です。" },
        { ModelValidationReason.Required, "backgroundCharacter", "敷き詰める文字は必須です。" },
        { ModelValidationReason.Required, "backgroundColor", "敷き詰める文字の色は必須です。" },
        { ModelValidationReason.LengthOutOfRange, "text", "描画する文字列は64文字以内で入力してください。" },
        { ModelValidationReason.LengthOutOfRange, "foregroundCharacter", "描画に使う文字は128文字以内で入力してください。" },
        { ModelValidationReason.LengthOutOfRange, "backgroundCharacter", "敷き詰める文字は128文字以内で入力してください。" },
        { ModelValidationReason.NotBlank, "text", "描画する文字列には空白以外の文字を入力してください。" },
        { ModelValidationReason.ControlCharacter, "text", "描画する文字列に制御文字は使用できません。" },
        { ModelValidationReason.ControlCharacter, "foregroundCharacter", "描画に使う文字に制御文字は使用できません。" },
        { ModelValidationReason.ControlCharacter, "backgroundCharacter", "敷き詰める文字に制御文字は使用できません。" },
        { ModelValidationReason.InvalidHexColor, "foregroundColor", "HEXカラー形式（#RRGGBB）で指定してください。" },
        { ModelValidationReason.InvalidHexColor, "backgroundColor", "HEXカラー形式（#RRGGBB）で指定してください。" },
        { ModelValidationReason.UnsupportedImageType, "type", "standard、x-background、x-iconのいずれかを指定してください。" },
        { ModelValidationReason.VisibleCharacterRequired, "foregroundCharacter", "描画に使う文字または敷き詰める文字のどちらかに、表示可能な文字を入力してください。" },
        { ModelValidationReason.VisibleCharacterRequired, "backgroundCharacter", "描画に使う文字または敷き詰める文字のどちらかに、表示可能な文字を入力してください。" },
        { ModelValidationReason.VisibleCharacterRequired, "foregroundCharacter,backgroundCharacter", "描画に使う文字または敷き詰める文字のどちらかに、表示可能な文字を入力してください。" },
    };

    public static TheoryData<ModelValidationReason, string, string> EnglishValidationMessages => new()
    {
        { ModelValidationReason.Required, "type", "The type field is required." },
        { ModelValidationReason.Required, "text", "The text field is required." },
        { ModelValidationReason.Required, "foregroundCharacter", "The foreground character field is required." },
        { ModelValidationReason.Required, "foregroundColor", "The foreground color field is required." },
        { ModelValidationReason.Required, "backgroundCharacter", "The background character field is required." },
        { ModelValidationReason.Required, "backgroundColor", "The background color field is required." },
        { ModelValidationReason.LengthOutOfRange, "text", "The text must be 64 characters or fewer." },
        { ModelValidationReason.LengthOutOfRange, "foregroundCharacter", "The foreground character must be 128 characters or fewer." },
        { ModelValidationReason.LengthOutOfRange, "backgroundCharacter", "The background character must be 128 characters or fewer." },
        { ModelValidationReason.NotBlank, "text", "The text must contain a non-whitespace character." },
        { ModelValidationReason.ControlCharacter, "text", "The text must not contain control characters." },
        { ModelValidationReason.ControlCharacter, "foregroundCharacter", "The foreground character must not contain control characters." },
        { ModelValidationReason.ControlCharacter, "backgroundCharacter", "The background character must not contain control characters." },
        { ModelValidationReason.InvalidHexColor, "foregroundColor", "The value must be specified in HEX color format (#RRGGBB)." },
        { ModelValidationReason.InvalidHexColor, "backgroundColor", "The value must be specified in HEX color format (#RRGGBB)." },
        { ModelValidationReason.UnsupportedImageType, "type", "The value must be one of: standard, x-background, or x-icon." },
        { ModelValidationReason.VisibleCharacterRequired, "foregroundCharacter", "Either the foreground or background characters must contain at least one visible character." },
        { ModelValidationReason.VisibleCharacterRequired, "backgroundCharacter", "Either the foreground or background characters must contain at least one visible character." },
        { ModelValidationReason.VisibleCharacterRequired, "foregroundCharacter,backgroundCharacter", "Either the foreground or background characters must contain at least one visible character." },
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

    [Theory]
    [MemberData(nameof(JapaneseValidationMessages))]
    public void ApiErrorMessageProvider_GetValidationMessage_WhenLanguageIsJapanese_ReturnsMessageForReasonAndTarget(
        ModelValidationReason reason,
        string target,
        string expectedMessage)
    {
        var message = ApiErrorMessageProvider.GetValidationMessage(
            ApiLanguage.Japanese,
            reason,
            target);

        Assert.Equal(expectedMessage, message);
    }

    [Theory]
    [MemberData(nameof(EnglishValidationMessages))]
    public void ApiErrorMessageProvider_GetValidationMessage_WhenLanguageIsEnglish_ReturnsMessageForReasonAndTarget(
        ModelValidationReason reason,
        string target,
        string expectedMessage)
    {
        var message = ApiErrorMessageProvider.GetValidationMessage(
            ApiLanguage.English,
            reason,
            target);

        Assert.Equal(expectedMessage, message);
    }

    [Theory]
    [InlineData(ApiLanguage.Japanese)]
    [InlineData(ApiLanguage.English)]
    public void ApiErrorMessageProvider_GetPublicMessage_WhenCodeIsUnsupported_ThrowsArgumentOutOfRangeException(
        ApiLanguage language)
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            ApiErrorMessageProvider.GetPublicMessage(language, "UNSUPPORTED"));

        Assert.StartsWith("Unsupported public API error code.", exception.Message);
    }

    [Theory]
    [InlineData(ApiLanguage.Japanese)]
    [InlineData(ApiLanguage.English)]
    public void ApiErrorMessageProvider_GetValidationMessage_WhenReasonAndTargetCombinationIsUnsupported_ThrowsArgumentException(
        ApiLanguage language)
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            ApiErrorMessageProvider.GetValidationMessage(
                language,
                ModelValidationReason.ValueOutOfRange,
                "red"));

        Assert.StartsWith("Unsupported validation reason and target combination.", exception.Message);
    }

    [Fact]
    public void ApiErrorMessageProvider_GetPublicMessage_WhenLanguageIsUnsupported_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            ApiErrorMessageProvider.GetPublicMessage((ApiLanguage)(-1), "BAD_REQUEST"));
    }

    [Fact]
    public void ApiErrorMessageProvider_GetValidationMessage_WhenLanguageIsUnsupported_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            ApiErrorMessageProvider.GetValidationMessage(
                (ApiLanguage)(-1),
                ModelValidationReason.Required,
                "text"));
    }
}
