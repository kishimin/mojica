using Mojica.Api.Models;

namespace Mojica.Api.Tests.Models;

public sealed class RenderTextTests
{
    [Fact]
    public void RenderText_Create_WhenInputIsMissing_ReturnsRequiredError()
    {
        var succeeded = RenderText.TryCreate(null, out var renderText, out var error);

        Assert.False(succeeded);
        Assert.Null(renderText);
        Assert.NotNull(error);
        Assert.Equal("REQUIRED", error.Code);
        Assert.Equal("text", error.Target);
        Assert.Equal(ModelValidationReason.Required, error.Reason);
    }

    [Fact]
    public void RenderText_Create_WhenInputIsEmpty_ReturnsLengthOutOfRangeError()
    {
        var succeeded = RenderText.TryCreate(string.Empty, out var renderText, out var error);

        Assert.False(succeeded);
        Assert.Null(renderText);
        Assert.NotNull(error);
        Assert.Equal("LENGTH_OUT_OF_RANGE", error.Code);
        Assert.Equal("text", error.Target);
        Assert.Equal(ModelValidationReason.LengthOutOfRange, error.Reason);
    }

    [Fact]
    public void RenderText_Create_WhenInputContainsOneGrapheme_Succeeds()
    {
        const string value = "A";

        var succeeded = RenderText.TryCreate(value, out var renderText, out var error);

        Assert.True(succeeded);
        Assert.NotNull(renderText);
        Assert.Equal(value, renderText.Value);
        Assert.Null(error);
    }

    [Fact]
    public void RenderText_Create_WhenInputContainsSixtyFourGraphemes_Succeeds()
    {
        var value = new string('A', 64);

        var succeeded = RenderText.TryCreate(value, out var renderText, out var error);

        Assert.True(succeeded);
        Assert.NotNull(renderText);
        Assert.Equal(value, renderText.Value);
        Assert.Null(error);
    }

    [Fact]
    public void RenderText_Create_WhenInputContainsSixtyFiveGraphemes_ReturnsLengthOutOfRangeError()
    {
        var value = new string('A', 65);

        var succeeded = RenderText.TryCreate(value, out var renderText, out var error);

        Assert.False(succeeded);
        Assert.Null(renderText);
        Assert.NotNull(error);
        Assert.Equal("LENGTH_OUT_OF_RANGE", error.Code);
        Assert.Equal("text", error.Target);
        Assert.Equal(ModelValidationReason.LengthOutOfRange, error.Reason);
    }

    [Fact]
    public void RenderText_Create_WhenEmojiUsesSurrogatePair_CountsItAsOneGrapheme()
    {
        var value = string.Concat(Enumerable.Repeat("😀", 64));

        var succeeded = RenderText.TryCreate(value, out var renderText, out var error);

        Assert.True(succeeded);
        Assert.NotNull(renderText);
        Assert.Equal(value, renderText.Value);
        Assert.Null(error);
    }

    [Fact]
    public void RenderText_Create_WhenCharacterUsesCombiningMark_CountsItAsOneGrapheme()
    {
        var value = string.Concat(Enumerable.Repeat("e\u0301", 64));

        var succeeded = RenderText.TryCreate(value, out var renderText, out var error);

        Assert.True(succeeded);
        Assert.NotNull(renderText);
        Assert.Equal(value, renderText.Value);
        Assert.Null(error);
    }

    [Fact]
    public void RenderText_Create_WhenInputIsOnlyWhitespace_ReturnsNotBlankError()
    {
        const string value = "   ";

        var succeeded = RenderText.TryCreate(value, out var renderText, out var error);

        Assert.False(succeeded);
        Assert.Null(renderText);
        Assert.NotNull(error);
        Assert.Equal("NOT_BLANK", error.Code);
        Assert.Equal("text", error.Target);
        Assert.Equal(ModelValidationReason.NotBlank, error.Reason);
    }

    [Fact]
    public void RenderText_Create_WhenInputContainsControlCharacter_ReturnsControlCharacterError()
    {
        const string value = "A\u0000";

        var succeeded = RenderText.TryCreate(value, out var renderText, out var error);

        Assert.False(succeeded);
        Assert.Null(renderText);
        Assert.NotNull(error);
        Assert.Equal("CONTROL_CHARACTER", error.Code);
        Assert.Equal("text", error.Target);
        Assert.Equal(ModelValidationReason.ControlCharacter, error.Reason);
    }
}
