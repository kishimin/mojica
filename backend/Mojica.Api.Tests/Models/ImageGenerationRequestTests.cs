// Test plan source: docs/v1/api/models.md §9 ImageGenerationRequest.
// These comments are unimplemented Small/Unit test cases, not executable or skipped tests.
// Namespace: Mojica.Api.Tests.Models
// Test class: ImageGenerationRequestTests

// REQUEST-01 TODO(test): ImageGenerationRequest_Create_WhenAllValuesAreValid_Succeeds
// Given: valid ImageType, RenderText, PatternCharacter, and HexColor values for every required attribute
// When: ImageGenerationRequest creation is requested
// Then: creation succeeds and exposes the complete validated request
// Priority: High

// REQUEST-02 TODO(test): ImageGenerationRequest_Create_WhenRequiredValueIsMissing_ReturnsRequiredError
// Given: each required attribute is missing in turn (Theory candidate)
// When: ImageGenerationRequest creation is requested
// Then: creation fails with code REQUIRED and identifies the missing attribute
// Priority: High

// REQUEST-03 TODO(test): ImageGenerationRequest_Create_WhenBothPatternValuesAreOnlyWhitespace_ReturnsVisibleCharacterRequiredError
// Given: independently valid foregroundCharacter and backgroundCharacter values that both contain only whitespace
// When: ImageGenerationRequest creation is requested
// Then: creation fails with code VISIBLE_CHARACTER_REQUIRED and targets the combination of both character attributes
// Priority: High

// REQUEST-04 TODO(test): ImageGenerationRequest_Create_WhenOnlyForegroundPatternIsVisible_Succeeds
// Given: a visible foregroundCharacter and a whitespace-only backgroundCharacter
// When: ImageGenerationRequest creation is requested
// Then: creation succeeds
// Priority: High

// REQUEST-05 TODO(test): ImageGenerationRequest_Create_WhenOnlyBackgroundPatternIsVisible_Succeeds
// Given: a whitespace-only foregroundCharacter and a visible backgroundCharacter
// When: ImageGenerationRequest creation is requested
// Then: creation succeeds
// Priority: High

// REQUEST-06 TODO(test): ImageGenerationRequest_Create_WhenValueObjectCreationFailed_DoesNotCreateAggregate
// Given: each Value Object creation failure in turn
// When: the caller attempts to proceed to ImageGenerationRequest creation
// Then: no ImageGenerationRequest is produced and the original ModelValidationError remains classifiable
// Priority: High
