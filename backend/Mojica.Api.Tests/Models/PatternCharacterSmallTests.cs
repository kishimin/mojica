using Mojica.Api.Models;

namespace Mojica.Api.Tests.Models;

public sealed class PatternCharacterSmallTests
{
    [Fact]
    public void PatternCharacter_Create_WhenInputIsMissing_ReturnsRequiredError()
    {
        var succeeded = PatternCharacter.TryCreate(
            null,
            out var patternCharacter,
            out var reason);

        Assert.False(succeeded);
        Assert.Null(patternCharacter);
        Assert.NotNull(reason);
        Assert.Equal("REQUIRED", reason.Value);
        Assert.Equal(ModelValidationReason.Required, reason);
    }

    [Fact]
    public void PatternCharacter_Create_WhenInputIsEmpty_ReturnsLengthOutOfRangeError()
    {
        var succeeded = PatternCharacter.TryCreate(
            string.Empty,
            out var patternCharacter,
            out var reason);

        Assert.False(succeeded);
        Assert.Null(patternCharacter);
        Assert.NotNull(reason);
        Assert.Equal("LENGTH_OUT_OF_RANGE", reason.Value);
        Assert.Equal(ModelValidationReason.LengthOutOfRange, reason);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(128)]
    public void PatternCharacter_Create_WhenInputContainsOneOrOneHundredTwentyEightGraphemes_Succeeds(int length)
    {
        var value = new string('A', length);

        var succeeded = PatternCharacter.TryCreate(
            value,
            out var patternCharacter,
            out var reason);

        Assert.True(succeeded);
        Assert.NotNull(patternCharacter);
        Assert.Equal(value, patternCharacter.Value);
        Assert.Null(reason);
    }

    [Fact]
    public void PatternCharacter_Create_WhenInputContainsOneHundredTwentyNineGraphemes_ReturnsLengthOutOfRangeError()
    {
        var value = new string('A', 129);

        var succeeded = PatternCharacter.TryCreate(
            value,
            out var patternCharacter,
            out var reason);

        Assert.False(succeeded);
        Assert.Null(patternCharacter);
        Assert.NotNull(reason);
        Assert.Equal("LENGTH_OUT_OF_RANGE", reason.Value);
        Assert.Equal(ModelValidationReason.LengthOutOfRange, reason);
    }

    [Fact]
    public void PatternCharacter_Create_WhenInputGreatlyExceedsMaximum_DoesNotAllocateForEveryGrapheme()
    {
        var value = new string('A', 1_000_000);

        var before = GC.GetAllocatedBytesForCurrentThread();
        var succeeded = PatternCharacter.TryCreate(value, out var patternCharacter, out var reason);
        var allocatedBytes = GC.GetAllocatedBytesForCurrentThread() - before;

        Assert.False(succeeded);
        Assert.Null(patternCharacter);
        Assert.NotNull(reason);
        Assert.Equal(ModelValidationReason.LengthOutOfRange, reason);
        Assert.True(allocatedBytes < 100_000, $"Allocated {allocatedBytes:N0} bytes.");
    }

    [Fact]
    public void PatternCharacter_Create_WhenInputIsOnlyWhitespace_Succeeds()
    {
        const string value = "   ";

        var succeeded = PatternCharacter.TryCreate(
            value,
            out var patternCharacter,
            out var reason);

        Assert.True(succeeded);
        Assert.NotNull(patternCharacter);
        Assert.Equal(value, patternCharacter.Value);
        Assert.Null(reason);
    }

    [Theory]
    [InlineData("😀")]
    [InlineData("e\u0301")]
    public void PatternCharacter_Create_WhenEmojiOrCombiningCharacterIsUsed_CountsGraphemeClusters(string grapheme)
    {
        var value = string.Concat(Enumerable.Repeat(grapheme, 128));

        var succeeded = PatternCharacter.TryCreate(
            value,
            out var patternCharacter,
            out var reason);

        Assert.True(succeeded);
        Assert.NotNull(patternCharacter);
        Assert.Equal(value, patternCharacter.Value);
        Assert.Null(reason);
    }

    [Fact]
    public void PatternCharacter_Create_WhenLengthEqualsDefensiveCeilingWithinGraphemeLimit_Succeeds()
    {
        // TextValueValidation rejects input whose UTF-16 length exceeds
        // maximumGraphemes(128) * 1024 code units as a defensive ceiling, before ever
        // counting graphemes. This value sits exactly on that ceiling (127 single-unit
        // graphemes plus one grapheme built from a base character and enough combining
        // marks to reach the boundary) while still totalling only 128 graphemes, so it
        // must be accepted rather than rejected by the length-based fast path alone.
        var value = new string('A', 127) + "A" + new string('́', 131072 - 128);

        var succeeded = PatternCharacter.TryCreate(
            value,
            out var patternCharacter,
            out var reason);

        Assert.True(succeeded);
        Assert.NotNull(patternCharacter);
        Assert.Null(reason);
    }

    [Fact]
    public void PatternCharacter_Create_WhenInputContainsControlCharacter_ReturnsControlCharacterError()
    {
        const string value = "A\u0000";

        var succeeded = PatternCharacter.TryCreate(
            value,
            out var patternCharacter,
            out var reason);

        Assert.False(succeeded);
        Assert.Null(patternCharacter);
        Assert.NotNull(reason);
        Assert.Equal("CONTROL_CHARACTER", reason.Value);
        Assert.Equal(ModelValidationReason.ControlCharacter, reason);
    }
}
