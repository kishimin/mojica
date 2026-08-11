using Mojica.Api.Models;

namespace Mojica.Api.Tests.Models;

public sealed class ModelValidationReasonTests
{
    [Fact]
    public void ModelValidationReason_WhenArbitraryValueIsRequested_CannotRepresentUndefinedReason()
    {
        // ID: ERROR-02
        // Source: docs/v1/api/models.md §11 ModelValidationReason.
        // Given: a value outside the closed set of ModelValidationReason values
        // When: the domain attempts to represent the reason
        // Then: an undefined reason cannot be created
        // Priority: Medium
        var expectedValues = new[]
        {
            "CONTROL_CHARACTER",
            "INVALID_HEX_COLOR",
            "LENGTH_OUT_OF_RANGE",
            "REQUIRED",
            "UNSUPPORTED_IMAGE_TYPE",
            "VISIBLE_CHARACTER_REQUIRED",
        };

        var actualValues = typeof(ModelValidationReason)
            .GetProperties()
            .Where(property => property.PropertyType == typeof(ModelValidationReason))
            .Select(property => ((ModelValidationReason)property.GetValue(null)!).Value)
            .Order(StringComparer.Ordinal);

        Assert.Equal(expectedValues, actualValues);
        Assert.Empty(typeof(ModelValidationReason).GetConstructors());
    }
}
