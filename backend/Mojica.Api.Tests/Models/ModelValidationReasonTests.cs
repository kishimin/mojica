// Test plan source: docs/v1/api/models.md §11 ModelValidationReason.
// These comments are unimplemented Small/Unit test cases, not executable or skipped tests.

// ERROR-02 TODO(test): ModelValidationReason_WhenArbitraryValueIsRequested_CannotRepresentUndefinedReason
// Given: a value outside the closed set of ModelValidationReason values
// When: the domain attempts to represent the reason
// Then: an undefined reason cannot be created
// Priority: Medium
