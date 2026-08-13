using Mojica.Api.Models;

namespace Mojica.Api.Tests.Models;

public sealed class PatternCharacterTests
{
    [Theory]
    [InlineData("foregroundCharacter")]
    [InlineData("backgroundCharacter")]
    public void PatternCharacter_Create_WhenInputIsMissing_ReturnsRequiredError(string target)
    {
        var succeeded = PatternCharacter.TryCreate(
            null,
            target,
            out var patternCharacter,
            out var error);

        Assert.False(succeeded);
        Assert.Null(patternCharacter);
        Assert.NotNull(error);
        Assert.Equal("REQUIRED", error.Code);
        Assert.Equal(target, error.Target);
        Assert.Equal(ModelValidationReason.Required, error.Reason);
    }

    [Fact]
    public void PatternCharacter_Create_WhenInputIsEmpty_ReturnsLengthOutOfRangeError()
    {
        var succeeded = PatternCharacter.TryCreate(
            string.Empty,
            "foregroundCharacter",
            out var patternCharacter,
            out var error);

        Assert.False(succeeded);
        Assert.Null(patternCharacter);
        Assert.NotNull(error);
        Assert.Equal("LENGTH_OUT_OF_RANGE", error.Code);
        Assert.Equal("foregroundCharacter", error.Target);
        Assert.Equal(ModelValidationReason.LengthOutOfRange, error.Reason);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(128)]
    public void PatternCharacter_Create_WhenInputContainsOneOrOneHundredTwentyEightGraphemes_Succeeds(int length)
    {
        var value = new string('A', length);

        var succeeded = PatternCharacter.TryCreate(
            value,
            "foregroundCharacter",
            out var patternCharacter,
            out var error);

        Assert.True(succeeded);
        Assert.NotNull(patternCharacter);
        Assert.Equal(value, patternCharacter.Value);
        Assert.Null(error);
    }

    [Fact]
    public void PatternCharacter_Create_WhenInputContainsOneHundredTwentyNineGraphemes_ReturnsLengthOutOfRangeError()
    {
        var value = new string('A', 129);

        var succeeded = PatternCharacter.TryCreate(
            value,
            "backgroundCharacter",
            out var patternCharacter,
            out var error);

        Assert.False(succeeded);
        Assert.Null(patternCharacter);
        Assert.NotNull(error);
        Assert.Equal("LENGTH_OUT_OF_RANGE", error.Code);
        Assert.Equal("backgroundCharacter", error.Target);
        Assert.Equal(ModelValidationReason.LengthOutOfRange, error.Reason);
    }

    [Fact]
    public void PatternCharacter_Create_WhenInputIsOnlyWhitespace_Succeeds()
    {
        const string value = "   ";

        var succeeded = PatternCharacter.TryCreate(
            value,
            "foregroundCharacter",
            out var patternCharacter,
            out var error);

        Assert.True(succeeded);
        Assert.NotNull(patternCharacter);
        Assert.Equal(value, patternCharacter.Value);
        Assert.Null(error);
    }

    [Theory]
    [InlineData("😀")]
    [InlineData("e\u0301")]
    public void PatternCharacter_Create_WhenEmojiOrCombiningCharacterIsUsed_CountsGraphemeClusters(string grapheme)
    {
        var value = string.Concat(Enumerable.Repeat(grapheme, 128));

        var succeeded = PatternCharacter.TryCreate(
            value,
            "backgroundCharacter",
            out var patternCharacter,
            out var error);

        Assert.True(succeeded);
        Assert.NotNull(patternCharacter);
        Assert.Equal(value, patternCharacter.Value);
        Assert.Null(error);
    }

    [Fact]
    public void PatternCharacter_Create_WhenInputContainsControlCharacter_ReturnsControlCharacterError()
    {
        const string value = "A\u0000";

        var succeeded = PatternCharacter.TryCreate(
            value,
            "foregroundCharacter",
            out var patternCharacter,
            out var error);

        Assert.False(succeeded);
        Assert.Null(patternCharacter);
        Assert.NotNull(error);
        Assert.Equal("CONTROL_CHARACTER", error.Code);
        Assert.Equal("foregroundCharacter", error.Target);
        Assert.Equal(ModelValidationReason.ControlCharacter, error.Reason);
    }
}
