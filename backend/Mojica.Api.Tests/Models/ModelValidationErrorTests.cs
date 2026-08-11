// Test plan source: docs/v1/api/models.md §11-12 ModelValidationError.
// These comments are unimplemented Small/Unit test cases, not executable or skipped tests.

// ERROR-01 TODO(test): ModelValidationError_Create_WhenValidationFails_ExposesMachineDetectableFields
// Given: a representative validation failure
// When: ModelValidationError is returned
// Then: code, target, closed ModelValidationReason, and safe optional details are available without a display message
// Priority: High
