namespace Mojica.Api.Tests.Models;

public sealed class ModelValidationErrorTests
{
    [Fact(Skip = "TODO: Implement from documented test plan.")]
    public void ModelValidationError_Create_WhenValidationFails_ExposesMachineDetectableFields()
    {
        // ID: ERROR-01
        // Source: docs/v1/api/models.md §11-12 ModelValidationError.
        // Given: a representative validation failure
        // When: ModelValidationError is returned
        // Then: code, target, closed ModelValidationReason, and safe optional details are available without a display message
        // Priority: High
    }
}
