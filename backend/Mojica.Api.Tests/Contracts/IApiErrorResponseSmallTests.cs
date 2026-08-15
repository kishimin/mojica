using System.Text.Json;
using Mojica.Api.Contracts;

namespace Mojica.Api.Tests.Contracts;

public sealed class IApiErrorResponseSmallTests
{
    [Fact]
    public void Deserialize_WhenJsonHasNoErrorsField_ReturnsApiErrorResponse()
    {
        const string json = """{"code":"BAD_REQUEST","message":"The request format is invalid."}""";

        var response = JsonSerializer.Deserialize<IApiErrorResponse>(json);

        var errorResponse = Assert.IsType<ApiErrorResponse>(response);
        Assert.Equal("BAD_REQUEST", errorResponse.Code);
        Assert.Equal("The request format is invalid.", errorResponse.Message);
    }

    [Fact]
    public void Deserialize_WhenJsonHasErrorsField_ReturnsApiValidationErrorResponse()
    {
        const string json = """
            {
                "code": "VALIDATION_ERROR",
                "message": "The input contains validation errors.",
                "errors": [{"field": "text", "message": "The text field is required."}]
            }
            """;

        var response = JsonSerializer.Deserialize<IApiErrorResponse>(json);

        var validationResponse = Assert.IsType<ApiValidationErrorResponse>(response);
        Assert.Equal("VALIDATION_ERROR", validationResponse.Code);
        var fieldError = Assert.Single(validationResponse.Errors);
        Assert.Equal("text", fieldError.Field);
        Assert.Equal("The text field is required.", fieldError.Message);
    }

    [Fact]
    public void RoundTrip_WhenResponseIsSerializedAndDeserializedThroughDeclaredType_PreservesShape()
    {
        IApiErrorResponse original = new ApiValidationErrorResponse(
            "VALIDATION_ERROR",
            "The input contains validation errors.",
            [new ApiValidationFieldError("text", "The text field is required.")]);

        var json = JsonSerializer.Serialize(original);
        var roundTripped = JsonSerializer.Deserialize<IApiErrorResponse>(json);

        Assert.Equal(original, roundTripped);
    }
}
