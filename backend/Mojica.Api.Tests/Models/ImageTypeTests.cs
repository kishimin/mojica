// Test plan source: docs/v1/api/models.md §4 ImageType.
// These comments are unimplemented Small/Unit test cases, not executable or skipped tests.
// Namespace: Mojica.Api.Tests.Models
// Test class: ImageTypeTests

// IMGTYPE-01 TODO(test): ImageType_Create_WhenValueIsSupported_ReturnsDefinedImageType
// Given: each supported value "standard", "x-background", and "x-icon" (Theory candidate)
// When: ImageType creation is requested
// Then: creation succeeds and preserves the corresponding predefined value
// Error: none
// Priority: High

// IMGTYPE-02 TODO(test): ImageType_Create_WhenValueIsUndefined_ReturnsUnsupportedImageTypeError
// Given: an arbitrary value that is not one of the three supported values
// When: ImageType creation is requested
// Then: creation fails with code UNSUPPORTED_IMAGE_TYPE, target type, and a closed ModelValidationReason
// Priority: High
