// Test plan source: docs/v1/api/models.md §11 ModelValidationReason.
// These comments are unimplemented Small/Unit test cases, not executable or skipped tests.
// Namespace: Mojica.Api.Tests.Models
// Test class: ModelValidationReasonTests

// ERROR-02 TODO(test): ModelValidationReason_WhenArbitraryValueIsRequested_CannotRepresentUndefinedReason
// Given: a value outside the closed set of ModelValidationReason values
// When: the domain attempts to represent the reason
// Then: an undefined reason cannot be created
// Priority: Medium

namespace Mojica.Api.Tests.Models;

public sealed class ModelValidationReasonTests
{
    [Fact(Skip = "TODO: Implement from documented test plan.")]
    public void ModelValidationReason_WhenArbitraryValueIsRequested_CannotRepresentUndefinedReason()
    {
        // TODO: Implement ERROR-02.
    }
}
