namespace Mojica.Api.Tests.Models;

public sealed class RgbColorSmallTests
{
    [Fact(Skip = "TODO: Implement from documented test plan.")]
    public void RgbColor_Create_WhenEveryComponentIsWithinRange_Succeeds()
    {
        // ID: RGB-01
        // Source: docs/v1/api/models.md §8 RgbColor.
        // Given: component combinations containing the boundaries 0 and 255 (Theory candidate)
        // When: RgbColor creation is requested
        // Then: creation succeeds and preserves red, green, and blue
        // Priority: High
    }

    [Fact(Skip = "TODO: Blocked until the RGB range error code is defined.")]
    public void RgbColor_Create_WhenAnyComponentIsBelowZero_ReturnsRangeError()
    {
        // ID: RGB-02
        // Source: docs/v1/api/models.md §8 RgbColor.
        // Given: red, green, or blue is -1 while the other components are valid (Theory candidate)
        // When: RgbColor creation is requested
        // Then: creation fails with the documented range error and identifies the invalid component
        // Blocked by: models.md does not define the language-independent RGB range error code
        // Priority: High
    }

    [Fact(Skip = "TODO: Blocked until the RGB range error code is defined.")]
    public void RgbColor_Create_WhenAnyComponentExceedsTwoHundredFiftyFive_ReturnsRangeError()
    {
        // ID: RGB-03
        // Source: docs/v1/api/models.md §8 RgbColor.
        // Given: red, green, or blue is 256 while the other components are valid (Theory candidate)
        // When: RgbColor creation is requested
        // Then: creation fails with the documented range error and identifies the invalid component
        // Blocked by: models.md does not define the language-independent RGB range error code
        // Priority: High
    }
}
