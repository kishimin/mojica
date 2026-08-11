using Mojica.Api.Models;

namespace Mojica.Api.Tests.Models;

public sealed class ModelValidationErrorTests
{
    [Fact]
    public void ModelValidationError_Create_WhenValidationFails_ExposesMachineDetectableFields()
    {
        var details = new Dictionary<string, string>
        {
            ["minimumLength"] = "1",
        };

        var error = new ModelValidationError(
            "text",
            ModelValidationReason.Required,
            details);

        Assert.Equal(error.Reason.Value, error.Code);
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
}
