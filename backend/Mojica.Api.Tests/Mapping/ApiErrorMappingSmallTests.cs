using System.Text.Json;
using Mojica.Api.Contracts;
using Mojica.Api.Localization;
using Mojica.Api.Mapping;
using Mojica.Api.Models;
using Mojica.Api.Ports;

namespace Mojica.Api.Tests.Mapping;

public sealed class ApiErrorMappingSmallTests
{
    [Fact]
    public void Map_WhenRequestIsMalformed_ReturnsBadRequestContract()
    {
        var result = ApiErrorMapper.MapMalformedRequest(ApiLanguage.English);

        Assert.Equal(400, result.StatusCode);
        Assert.Equal("BAD_REQUEST", result.Response.Code);
        Assert.Equal("The request format is invalid.", result.Response.Message);
    }

    [Fact]
    public void Map_WhenValidationFails_ReturnsValidationErrorContract()
    {
        var validationErrors = new[]
        {
            new ModelValidationError("text", ModelValidationReason.Required),
        };

        var result = ApiErrorMapper.MapValidationFailure(
            validationErrors,
            ApiLanguage.English);

        var response = Assert.IsType<ApiValidationErrorResponse>(result.Response);
        Assert.Equal(422, result.StatusCode);
        Assert.Equal("VALIDATION_ERROR", response.Code);
        Assert.Equal("The input contains validation errors.", response.Message);
        var fieldError = Assert.Single(response.Errors);
        Assert.Equal("text", fieldError.Field);
        Assert.Equal("The text field is required.", fieldError.Message);
    }

    [Fact]
    public void Map_WhenValidationErrorsAreEmpty_ThrowsArgumentException()
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            ApiErrorMapper.MapValidationFailure(
                Array.Empty<ModelValidationError>(),
                ApiLanguage.English));

        Assert.Equal("errors", exception.ParamName);
        Assert.StartsWith(
            "Validation failure mapping requires at least one validation error.",
            exception.Message);
    }

    [Fact]
    public void Map_WhenValidationFails_SerializesFieldErrorsThroughDeclaredResponseType()
    {
        var validationErrors = new[]
        {
            new ModelValidationError("text", ModelValidationReason.Required),
        };

        var result = ApiErrorMapper.MapValidationFailure(
            validationErrors,
            ApiLanguage.English);

        IApiErrorResponse declaredResponse = result.Response;
        using var document = JsonDocument.Parse(JsonSerializer.Serialize(declaredResponse));
        var errors = document.RootElement.GetProperty("errors");

        var fieldError = Assert.Single(errors.EnumerateArray());
        Assert.Equal("text", fieldError.GetProperty("field").GetString());
        Assert.Equal("The text field is required.", fieldError.GetProperty("message").GetString());
    }

    [Fact]
    public void Map_WhenOutputSizeIsExceeded_ReturnsOutputSizeLimitContract()
    {
        var portError = new ImageGenerationPortError(
            ImageGenerationPortErrorCode.OutputSizeExceeded,
            details: "Glyph Forge internal pixel limit: 12345");

        var result = ApiErrorMapper.MapPortFailure(portError, ApiLanguage.English);

        var response = Assert.IsType<ApiErrorResponse>(result.Response);
        Assert.Equal(422, result.StatusCode);
        Assert.Equal("IMAGE_SIZE_LIMIT_EXCEEDED", response.Code);
        Assert.Equal(
            "The generated image would exceed the size limit. Reduce the input text.",
            response.Message);
    }

    [Fact]
    public void Map_WhenGenerationIsRateLimited_ReturnsRateLimitContract()
    {
        var portError = new ImageGenerationPortError(
            ImageGenerationPortErrorCode.RateLimited,
            retryAfter: 7);

        var result = ApiErrorMapper.MapPortFailure(portError, ApiLanguage.English);

        var response = Assert.IsType<ApiErrorResponse>(result.Response);
        Assert.Equal(429, result.StatusCode);
        Assert.Equal("RATE_LIMIT_EXCEEDED", response.Code);
        Assert.Equal(7, result.RetryAfter);
    }

    [Fact]
    public void Map_WhenGenerationTimesOut_ReturnsTimeoutContract()
    {
        var portError = new ImageGenerationPortError(
            ImageGenerationPortErrorCode.Timeout,
            details: "internal timeout diagnostics");

        var result = ApiErrorMapper.MapPortFailure(portError, ApiLanguage.English);

        var response = Assert.IsType<ApiErrorResponse>(result.Response);
        Assert.Equal(504, result.StatusCode);
        Assert.Equal("IMAGE_GENERATION_TIMEOUT", response.Code);
        Assert.Equal(
            "Image generation is taking too long. Please try again later.",
            response.Message);
    }

    [Theory]
    [InlineData("UNAVAILABLE")]
    [InlineData("INVALID_RESPONSE")]
    [InlineData("FAILED")]
    public void Map_WhenProviderFailureIsClassified_ReturnsGenerationFailureContract(
        string failureCode)
    {
        var portError = CreateProviderFailure(failureCode);

        var result = ApiErrorMapper.MapPortFailure(portError, ApiLanguage.English);

        var response = Assert.IsType<ApiErrorResponse>(result.Response);
        Assert.Equal(502, result.StatusCode);
        Assert.Equal("IMAGE_GENERATION_FAILED", response.Code);
        Assert.Equal(
            "Image generation failed. Please try again later.",
            response.Message);
    }

    [Fact]
    public void Map_WhenUnexpectedExceptionIsClassified_ReturnsInternalServerErrorContract()
    {
        var result = ApiErrorMapper.MapUnexpectedFailure(ApiLanguage.English);

        var response = Assert.IsType<ApiErrorResponse>(result.Response);
        Assert.Equal(500, result.StatusCode);
        Assert.Equal("INTERNAL_SERVER_ERROR", response.Code);
        Assert.Equal(
            "An unexpected error occurred while generating the image.",
            response.Message);
    }

    private static ImageGenerationPortError CreateProviderFailure(string failureCode)
    {
        return failureCode switch
        {
            "UNAVAILABLE" => new ImageGenerationPortError(
                ImageGenerationPortErrorCode.Unavailable,
                details: "connection refused"),
            "INVALID_RESPONSE" => new ImageGenerationPortError(
                ImageGenerationPortErrorCode.InvalidResponse,
                details: "provider body and internal URL"),
            "FAILED" => new ImageGenerationPortError(
                ImageGenerationPortErrorCode.Failed,
                details: "provider stack trace"),
            _ => throw new ArgumentOutOfRangeException(nameof(failureCode), failureCode, null),
        };
    }
}
