using System.Text.Json;
using Mojica.Api.Contracts;

namespace Mojica.Api.Tests.Contracts;

public sealed class ApiValidationErrorResponseSmallTests
{
    [Fact]
    public void Serialize_WhenValidationErrorsExist_WritesOverallErrorAndFieldErrors()
    {
        var response = new ApiValidationErrorResponse(
            "VALIDATION_ERROR",
            "The input contains validation errors.",
            [
                new ApiValidationFieldError("text", "The text field is required."),
                new ApiValidationFieldError("foregroundColor", "The value must be specified in HEX color format (#RRGGBB)."),
            ]);

        using var document = JsonDocument.Parse(JsonSerializer.Serialize(response));
        var root = document.RootElement;
        var errors = root.GetProperty("errors");

        Assert.Equal("VALIDATION_ERROR", root.GetProperty("code").GetString());
        Assert.Equal("The input contains validation errors.", root.GetProperty("message").GetString());
        Assert.Equal(2, errors.GetArrayLength());
        Assert.Equal("text", errors[0].GetProperty("field").GetString());
        Assert.Equal("The text field is required.", errors[0].GetProperty("message").GetString());
        Assert.Equal("foregroundColor", errors[1].GetProperty("field").GetString());
        Assert.Equal("The value must be specified in HEX color format (#RRGGBB).", errors[1].GetProperty("message").GetString());
    }

    [Fact]
    public void Serialize_WhenCombinationErrorAffectsTwoFields_WritesBothFieldEntries()
    {
        const string message = "Either the foreground or background characters must contain at least one visible character.";
        var response = new ApiValidationErrorResponse(
            "VALIDATION_ERROR",
            "The input contains validation errors.",
            [
                new ApiValidationFieldError("foregroundCharacter", message),
                new ApiValidationFieldError("backgroundCharacter", message),
            ]);

        using var document = JsonDocument.Parse(JsonSerializer.Serialize(response));
        var errors = document.RootElement.GetProperty("errors");

        Assert.Equal(2, errors.GetArrayLength());
        Assert.Equal("foregroundCharacter", errors[0].GetProperty("field").GetString());
        Assert.Equal("backgroundCharacter", errors[1].GetProperty("field").GetString());
        Assert.All(
            errors.EnumerateArray(),
            error => Assert.Equal(message, error.GetProperty("message").GetString()));
    }

    [Fact]
    public void Serialize_WhenJapaneseMessageIsUsed_KeepsCodeAndFieldValuesStable()
    {
        var response = new ApiValidationErrorResponse(
            "VALIDATION_ERROR",
            "入力内容に誤りがあります。",
            [new ApiValidationFieldError("text", "描画する文字列は必須です。")]);

        using var document = JsonDocument.Parse(JsonSerializer.Serialize(response));
        var root = document.RootElement;

        Assert.Equal("VALIDATION_ERROR", root.GetProperty("code").GetString());
        Assert.Equal("入力内容に誤りがあります。", root.GetProperty("message").GetString());
        Assert.Equal("text", root.GetProperty("errors")[0].GetProperty("field").GetString());
        Assert.Equal("描画する文字列は必須です。", root.GetProperty("errors")[0].GetProperty("message").GetString());
    }

    [Fact]
    public void Serialize_WhenEnglishMessageIsUsed_KeepsCodeAndFieldValuesStable()
    {
        var response = new ApiValidationErrorResponse(
            "VALIDATION_ERROR",
            "The input contains validation errors.",
            [new ApiValidationFieldError("text", "The text field is required.")]);

        using var document = JsonDocument.Parse(JsonSerializer.Serialize(response));
        var root = document.RootElement;

        Assert.Equal("VALIDATION_ERROR", root.GetProperty("code").GetString());
        Assert.Equal("The input contains validation errors.", root.GetProperty("message").GetString());
        Assert.Equal("text", root.GetProperty("errors")[0].GetProperty("field").GetString());
        Assert.Equal("The text field is required.", root.GetProperty("errors")[0].GetProperty("message").GetString());
    }
}
