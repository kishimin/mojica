// Test plan source: docs/v1/api/models.md §7 HexColor.
// These comments are unimplemented Small/Unit test cases, not executable or skipped tests.
// Namespace: Mojica.Api.Tests.Models
// Test class: HexColorTests

// HEX-01 TODO(test): HexColor_Create_WhenInputUsesValidRrgGBbFormat_NormalizesToUppercase
// Given: valid uppercase, lowercase, and mixed-case #RRGGBB values (Theory candidate)
// When: HexColor creation is requested
// Then: creation succeeds and string representation uses uppercase #RRGGBB
// Priority: High

// HEX-02 TODO(test): HexColor_Create_WhenInputIsMissing_ReturnsRequiredError
// Given: a missing color value
// When: HexColor creation is requested
// Then: creation fails with code REQUIRED and the corresponding color target
// Priority: High

// HEX-03 TODO(test): HexColor_Create_WhenFormatIsInvalid_ReturnsInvalidHexColorError
// Given: values with a missing hash, wrong digit count, non-hex digits, or surrounding whitespace (Theory candidate)
// When: HexColor creation is requested
// Then: creation fails with code INVALID_HEX_COLOR and the corresponding color target
// Priority: High

// HEX-04 TODO(test): HexColor_ToRgb_WhenValueIsFf69b4_ReturnsExpectedComponents
// Given: normalized color #FF69B4
// When: it is converted to RgbColor
// Then: red is 255, green is 105, and blue is 180
// Priority: High

// HEX-05 TODO(test): HexColor_ToRgb_WhenComponentsAreAtBoundaries_ReturnsZeroAndTwoHundredFiftyFive
// Given: #000000 and #FFFFFF (Theory candidate)
// When: each color is converted to RgbColor
// Then: every component is respectively 0 or 255
// Priority: Medium

namespace Mojica.Api.Tests.Models;

public sealed class HexColorTests
{
    [Fact(Skip = "TODO: Implement from documented test plan.")]
    public void HexColor_Create_WhenInputUsesValidRrgGBbFormat_NormalizesToUppercase()
    {
        // TODO: Implement HEX-01.
    }

    [Fact(Skip = "TODO: Implement from documented test plan.")]
    public void HexColor_Create_WhenInputIsMissing_ReturnsRequiredError()
    {
        // TODO: Implement HEX-02.
    }

    [Fact(Skip = "TODO: Implement from documented test plan.")]
    public void HexColor_Create_WhenFormatIsInvalid_ReturnsInvalidHexColorError()
    {
        // TODO: Implement HEX-03.
    }

    [Fact(Skip = "TODO: Implement from documented test plan.")]
    public void HexColor_ToRgb_WhenValueIsFf69b4_ReturnsExpectedComponents()
    {
        // TODO: Implement HEX-04.
    }

    [Fact(Skip = "TODO: Implement from documented test plan.")]
    public void HexColor_ToRgb_WhenComponentsAreAtBoundaries_ReturnsZeroAndTwoHundredFiftyFive()
    {
        // TODO: Implement HEX-05.
    }
}
