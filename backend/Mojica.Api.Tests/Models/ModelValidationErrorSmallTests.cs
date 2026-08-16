using Mojica.Api.Models;

namespace Mojica.Api.Tests.Models;

public sealed class ModelValidationErrorSmallTests
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
    public void ModelValidationError_Create_WhenTargetsIsNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new ModelValidationError((IReadOnlyList<string>)null!, ModelValidationReason.Required));
    }

    [Fact]
    public void ModelValidationError_Create_WhenTargetsAreEmpty_ThrowsArgumentException()
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            new ModelValidationError(Array.Empty<string>(), ModelValidationReason.Required));

        Assert.StartsWith("At least one validation target is required.", exception.Message);
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

    [Fact]
    public void ModelValidationError_Equality_WhenValuesDiffer_DistinguishesErrors()
    {
        // ID: ERROR-04
        // Source: docs/v1/api/models.md §11 ModelValidationError.
        // Given: validation errors that differ by target or detail contents
        // When: each error is compared with a baseline Domain value
        // Then: only the same instance is equal and every distinct value is unequal
        // Priority: Medium
        var baseline = new ModelValidationError(
            "text",
            ModelValidationReason.Required,
            new Dictionary<string, string>
            {
                ["minimumLength"] = "1",
            });

        Assert.True(baseline.Equals(baseline));
        Assert.False(baseline.Equals(null));

        var differentTarget = new ModelValidationError(
            "foregroundCharacter",
            ModelValidationReason.Required,
            baseline.Details);
        Assert.NotEqual(baseline, differentTarget);
        Assert.NotEqual(baseline.GetHashCode(), differentTarget.GetHashCode());

        Assert.NotEqual(
            baseline,
            new ModelValidationError("text", ModelValidationReason.Required));
        Assert.NotEqual(
            baseline,
            new ModelValidationError(
                "text",
                ModelValidationReason.Required,
                new Dictionary<string, string>
                {
                    ["maximumLength"] = "1",
                }));
        Assert.NotEqual(
            baseline,
            new ModelValidationError(
                "text",
                ModelValidationReason.Required,
                new Dictionary<string, string>
                {
                    ["minimumLength"] = "2",
                }));
    }

    [Fact]
    public void ModelValidationError_Equality_WhenReasonDiffers_IsNotEqualAndHashCodeDiffers()
    {
        var details = new Dictionary<string, string> { ["minimumLength"] = "1" };
        var first = new ModelValidationError("text", ModelValidationReason.Required, details);
        var second = new ModelValidationError("text", ModelValidationReason.LengthOutOfRange, details);

        Assert.NotEqual(first, second);
        Assert.NotEqual(first.GetHashCode(), second.GetHashCode());
    }

    [Fact]
    public void ModelValidationError_Equality_WhenDetailKeyDiffers_HashCodeDiffers()
    {
        var first = new ModelValidationError(
            "text",
            ModelValidationReason.Required,
            new Dictionary<string, string> { ["minimumLength"] = "1" });
        var second = new ModelValidationError(
            "text",
            ModelValidationReason.Required,
            new Dictionary<string, string> { ["maximumLength"] = "1" });

        Assert.NotEqual(first.GetHashCode(), second.GetHashCode());
    }

    [Fact]
    public void ModelValidationError_Equality_WhenDetailValueDiffers_HashCodeDiffers()
    {
        var first = new ModelValidationError(
            "text",
            ModelValidationReason.Required,
            new Dictionary<string, string> { ["minimumLength"] = "1" });
        var second = new ModelValidationError(
            "text",
            ModelValidationReason.Required,
            new Dictionary<string, string> { ["minimumLength"] = "2" });

        Assert.NotEqual(first.GetHashCode(), second.GetHashCode());
    }
}
