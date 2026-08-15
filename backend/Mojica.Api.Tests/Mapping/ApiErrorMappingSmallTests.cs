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

    [Fact(Skip = "TODO: implement API error mapping")]
    public void Map_WhenUpstreamIsUnavailable_ReturnsGenerationFailureContract()
    {
        // ID: API-ERROR-MAP-S-006
        // Source: docs/v1/api/controllers.md §6, §9-10; docs/v1/api/api.md §11
        // Given: A service result classified as UNAVAILABLE
        // When: The API error mapping converts the failure to the public contract
        // Then: The result represents HTTP 502 with code IMAGE_GENERATION_FAILED and no internal details
        // Priority: High
        // Theory candidate: share the public contract with INVALID_RESPONSE and FAILED while keeping source classifications explicit
    }

    [Fact(Skip = "TODO: implement API error mapping")]
    public void Map_WhenUpstreamResponseIsInvalid_ReturnsGenerationFailureContract()
    {
        // ID: API-ERROR-MAP-S-007
        // Source: docs/v1/api/controllers.md §6, §10; docs/v1/api/api.md §11
        // Given: A service result classified as INVALID_RESPONSE
        // When: The API error mapping converts the failure to the public contract
        // Then: The result represents HTTP 502 with code IMAGE_GENERATION_FAILED and no upstream body or URL
        // Priority: High
    }

    [Fact(Skip = "TODO: implement API error mapping")]
    public void Map_WhenGenerationFailsUnexpectedly_ReturnsGenerationFailureContract()
    {
        // ID: API-ERROR-MAP-S-008
        // Source: docs/v1/api/controllers.md §6, §9-10; docs/v1/api/api.md §11
        // Given: A service result classified as FAILED
        // When: The API error mapping converts the failure to the public contract
        // Then: The result represents HTTP 502 with code IMAGE_GENERATION_FAILED and no internal details
        // Priority: High
    }

    [Fact(Skip = "TODO: implement API error mapping")]
    public void Map_WhenUnexpectedExceptionIsClassified_ReturnsInternalServerErrorContract()
    {
        // ID: API-ERROR-MAP-S-009
        // Source: docs/v1/api/controllers.md §9-10; docs/v1/api/api.md §12
        // Given: An unexpected application failure classified for public error conversion
        // When: The API error mapping converts the failure to the public contract
        // Then: The result represents HTTP 500 with code INTERNAL_SERVER_ERROR and a safe localized message
        // Error: Log details internally but never expose exception messages, stack traces, URLs, or credentials
        // Priority: High
    }
}
