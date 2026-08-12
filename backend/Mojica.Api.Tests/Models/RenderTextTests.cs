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

    [Fact(Skip = "TODO: Implement from documented test plan.")]
    public void RenderText_Create_WhenInputContainsSixtyFourGraphemes_Succeeds()
    {
        // ID: RENDERTEXT-04
        // Source: docs/v1/api/models.md §5 RenderText.
        // Given: a string containing exactly 64 Unicode grapheme clusters
        // When: RenderText creation is requested
        // Then: creation succeeds and preserves the value
        // Priority: High
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

    [Fact(Skip = "TODO: Implement from documented test plan.")]
    public void RenderText_Create_WhenCharacterUsesCombiningMark_CountsItAsOneGrapheme()
    {
        // ID: RENDERTEXT-07
        // Source: docs/v1/api/models.md §5 RenderText.
        // Given: text containing a base character followed by a combining mark
        // When: RenderText validates its character count
        // Then: the combined character contributes one to the length
        // Priority: High
    }

    [Fact(Skip = "TODO: Blocked until the whitespace-only error code is defined.")]
    public void RenderText_Create_WhenInputIsOnlyWhitespace_ReturnsValidationError()
    {
        // ID: RENDERTEXT-08
        // Source: docs/v1/api/models.md §5 RenderText.
        // Given: whitespace-only values including spaces, tabs, and line separators (Theory candidate)
        // When: RenderText creation is requested
        // Then: creation fails with the documented whitespace validation reason and target text
        // Blocked by: models.md does not define the language-independent error code for whitespace-only RenderText
        // Priority: High
    }

    [Fact(Skip = "TODO: Implement from documented test plan.")]
    public void RenderText_Create_WhenInputContainsControlCharacter_ReturnsControlCharacterError()
    {
        // ID: RENDERTEXT-09
        // Source: docs/v1/api/models.md §5 RenderText.
        // Given: otherwise valid text containing a control character
        // When: RenderText creation is requested
        // Then: creation fails with code CONTROL_CHARACTER and target text
        // Priority: High
    }
}
