namespace Mojica.Api.Tests.Models;

public sealed class HexColorSmallTests
{
    [Fact(Skip = "TODO: Implement from documented test plan.")]
    public void HexColor_Create_WhenInputUsesValidRrgGBbFormat_NormalizesToUppercase()
    {
        // ID: HEX-01
        // Source: docs/v1/api/models.md §7 HexColor.
        // Given: valid uppercase, lowercase, and mixed-case #RRGGBB values (Theory candidate)
        // When: HexColor creation is requested
        // Then: creation succeeds and string representation uses uppercase #RRGGBB
        // Priority: High
    }

    [Fact(Skip = "TODO: Implement from documented test plan.")]
    public void HexColor_Create_WhenInputIsMissing_ReturnsRequiredError()
    {
        // ID: HEX-02
        // Source: docs/v1/api/models.md §7 HexColor.
        // Given: a missing color value
        // When: HexColor creation is requested
        // Then: creation fails with code REQUIRED and the corresponding color target
        // Priority: High
    }

    [Fact(Skip = "TODO: Implement from documented test plan.")]
    public void HexColor_Create_WhenFormatIsInvalid_ReturnsInvalidHexColorError()
    {
        // ID: HEX-03
        // Source: docs/v1/api/models.md §7 HexColor.
        // Given: values with a missing hash, wrong digit count, non-hex digits, or surrounding whitespace (Theory candidate)
        // When: HexColor creation is requested
        // Then: creation fails with code INVALID_HEX_COLOR and the corresponding color target
        // Priority: High
    }

    [Fact(Skip = "TODO: Implement from documented test plan.")]
    public void HexColor_ToRgb_WhenValueIsFf69b4_ReturnsExpectedComponents()
    {
        // ID: HEX-04
        // Source: docs/v1/api/models.md §7 HexColor.
        // Given: normalized color #FF69B4
        // When: it is converted to RgbColor
        // Then: red is 255, green is 105, and blue is 180
        // Priority: High
    }

    [Fact(Skip = "TODO: Implement from documented test plan.")]
    public void HexColor_ToRgb_WhenComponentsAreAtBoundaries_ReturnsZeroAndTwoHundredFiftyFive()
    {
        // ID: HEX-05
        // Source: docs/v1/api/models.md §7 HexColor.
        // Given: #000000 and #FFFFFF (Theory candidate)
        // When: each color is converted to RgbColor
        // Then: every component is respectively 0 or 255
        // Priority: Medium
    }
}
