using Mojica.Api.Models;

namespace Mojica.Api.Tests.Models;

public sealed class ModelValidationErrorTests
{
    [Fact]
    public void ModelValidationError_Create_WhenValidationFails_ExposesMachineDetectableFields()
    {
        // ID: ERROR-01
        // Source: docs/v1/api/models.md §11-12 ModelValidationError.
        // Given: a representative validation failure
        // When: ModelValidationError is returned
        // Then: code, target, closed ModelValidationReason, and safe optional details are available without a display message
        // Priority: High
        var details = new Dictionary<string, string>
        {
            ["minimumLength"] = "1",
        };

        var error = new ModelValidationError(
            "text",
            ModelValidationReason.Required,
            details);

        Assert.Equal("REQUIRED", error.Code);
        Assert.Equal("text", error.Target);
        Assert.Same(ModelValidationReason.Required, error.Reason);
        Assert.Equal("1", error.Details?["minimumLength"]);
        Assert.Null(typeof(ModelValidationError).GetProperty("Message"));
    }

    [Fact]
    public void ModelValidationError_Create_WhenSourceDetailsChange_PreservesOriginalDetails()
    {
        var details = new Dictionary<string, string>
        {
            ["minimumLength"] = "1",
        };
        var error = new ModelValidationError(
            "text",
            ModelValidationReason.Required,
            details);

        details["minimumLength"] = "999";

        Assert.Equal("1", error.Details?["minimumLength"]);
    }

    [Fact]
    public void ModelValidationError_Create_WhenDetailsAreOmitted_ExposesEmptyDetails()
    {
        var error = new ModelValidationError(
            "text",
            ModelValidationReason.Required);

        Assert.Empty(error.Details);
    }

    [Fact]
    public void ModelValidationError_Equality_WhenValuesMatch_UsesDetailContents()
    {
        // ID: ERROR-03
        // Source: docs/v1/api/models.md §11 ModelValidationError.
        // Given: two validation errors with the same values in separately-created detail collections
        // When: the errors are compared as Domain values
        // Then: they are equal and produce the same hash code regardless of detail insertion order
        // Priority: Medium
        var first = new ModelValidationError(
            "text",
            ModelValidationReason.Required,
            new Dictionary<string, string>
            {
                ["minimumLength"] = "1",
                ["actualLength"] = "0",
            });
        var second = new ModelValidationError(
            "text",
            ModelValidationReason.Required,
            new Dictionary<string, string>
            {
                ["actualLength"] = "0",
                ["minimumLength"] = "1",
            });

        Assert.Equal(first, second);
        Assert.Equal(first.GetHashCode(), second.GetHashCode());
    }
}
