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

    [Fact(Skip = "TODO: Implement from documented test plan.")]
    public void PatternCharacter_Create_WhenInputContainsOneOrOneHundredTwentyEightGraphemes_Succeeds()
    {
        // ID: PATTERN-03
        // Source: docs/v1/api/models.md §6 PatternCharacter.
        // Given: exactly 1 or exactly 128 Unicode grapheme clusters (Theory candidate)
        // When: PatternCharacter creation is requested
        // Then: creation succeeds and preserves the value
        // Priority: High
    }

    [Fact(Skip = "TODO: Implement from documented test plan.")]
    public void PatternCharacter_Create_WhenInputContainsOneHundredTwentyNineGraphemes_ReturnsLengthOutOfRangeError()
    {
        // ID: PATTERN-04
        // Source: docs/v1/api/models.md §6 PatternCharacter.
        // Given: exactly 129 Unicode grapheme clusters
        // When: PatternCharacter creation is requested
        // Then: creation fails with code LENGTH_OUT_OF_RANGE and the corresponding attribute target
        // Priority: High
    }

    [Fact(Skip = "TODO: Implement from documented test plan.")]
    public void PatternCharacter_Create_WhenInputIsOnlyWhitespace_Succeeds()
    {
        // ID: PATTERN-05
        // Source: docs/v1/api/models.md §6 PatternCharacter.
        // Given: a non-empty string consisting only of whitespace
        // When: PatternCharacter creation is requested independently
        // Then: creation succeeds because visibility is an ImageGenerationRequest cross-field invariant
        // Priority: High
    }

    [Fact(Skip = "TODO: Implement from documented test plan.")]
    public void PatternCharacter_Create_WhenEmojiOrCombiningCharacterIsUsed_CountsGraphemeClusters()
    {
        // ID: PATTERN-06
        // Source: docs/v1/api/models.md §6 PatternCharacter.
        // Given: surrogate-pair emoji and combining-character inputs near the length boundary (Theory candidate)
        // When: PatternCharacter validates its character count
        // Then: each perceived character contributes one to the length
        // Priority: High
    }

    [Fact(Skip = "TODO: Implement from documented test plan.")]
    public void PatternCharacter_Create_WhenInputContainsControlCharacter_ReturnsControlCharacterError()
    {
        // ID: PATTERN-07
        // Source: docs/v1/api/models.md §6 PatternCharacter.
        // Given: an otherwise valid pattern containing a control character
        // When: PatternCharacter creation is requested
        // Then: creation fails with code CONTROL_CHARACTER and the corresponding attribute target
        // Priority: High
    }
}
