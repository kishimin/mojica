// Test plan source: docs/v1/api/models.md §8 RgbColor.
// These comments are unimplemented Small/Unit test cases, not executable or skipped tests.
// Namespace: Mojica.Api.Tests.Models
// Test class: RgbColorTests

// RGB-01 TODO(test): RgbColor_Create_WhenEveryComponentIsWithinRange_Succeeds
// Given: component combinations containing the boundaries 0 and 255 (Theory candidate)
// When: RgbColor creation is requested
// Then: creation succeeds and preserves red, green, and blue
// Priority: High

// RGB-02 TODO(test): RgbColor_Create_WhenAnyComponentIsBelowZero_ReturnsRangeError
// Given: red, green, or blue is -1 while the other components are valid (Theory candidate)
// When: RgbColor creation is requested
// Then: creation fails with the documented range error and identifies the invalid component
// Blocked by: models.md does not define the language-independent RGB range error code
// Priority: High

// RGB-03 TODO(test): RgbColor_Create_WhenAnyComponentExceedsTwoHundredFiftyFive_ReturnsRangeError
// Given: red, green, or blue is 256 while the other components are valid (Theory candidate)
// When: RgbColor creation is requested
// Then: creation fails with the documented range error and identifies the invalid component
// Blocked by: models.md does not define the language-independent RGB range error code
// Priority: High
