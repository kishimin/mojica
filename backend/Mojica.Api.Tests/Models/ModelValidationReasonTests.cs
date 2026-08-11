namespace Mojica.Api.Tests.Models;

public sealed class ModelValidationReasonTests
{
    [Fact(Skip = "TODO: Implement from documented test plan.")]
    public void ModelValidationReason_WhenArbitraryValueIsRequested_CannotRepresentUndefinedReason()
    {
        // ID: ERROR-02
        // Source: docs/v1/api/models.md §11 ModelValidationReason.
        // Given: a value outside the closed set of ModelValidationReason values
        // When: the domain attempts to represent the reason
        // Then: an undefined reason cannot be created
        // Priority: Medium
    }
}
