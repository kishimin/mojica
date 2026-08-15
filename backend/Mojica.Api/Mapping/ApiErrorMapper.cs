using Mojica.Api.Contracts;
using Mojica.Api.Localization;
using Mojica.Api.Models;
using Mojica.Api.Ports;

namespace Mojica.Api.Mapping;

public static class ApiErrorMapper
{
    public static ApiErrorMappingResult MapMalformedRequest(ApiLanguage language)
    {
        const string code = "BAD_REQUEST";
        return new(
            StatusCodes.Status400BadRequest,
            new ApiErrorResponse(code, ApiErrorMessageProvider.GetPublicMessage(language, code)));
    }

    public static ApiErrorMappingResult MapValidationFailure(
        IEnumerable<ModelValidationError> errors,
        ApiLanguage language)
    {
        ArgumentNullException.ThrowIfNull(errors);

        const string code = "VALIDATION_ERROR";
        var fieldErrors = errors
            .Select(error => new ApiValidationFieldError(
                error.Target,
                ApiErrorMessageProvider.GetValidationMessage(
                    language,
                    error.Reason,
                    error.Target)))
            .ToArray();

        return new(
            StatusCodes.Status422UnprocessableEntity,
            new ApiValidationErrorResponse(
                code,
                ApiErrorMessageProvider.GetPublicMessage(language, code),
                fieldErrors));
    }

    public static ApiErrorMappingResult MapPortFailure(
        ImageGenerationPortError error,
        ApiLanguage language)
    {
        ArgumentNullException.ThrowIfNull(error);

        return error.ErrorCode.Value switch
        {
            "RATE_LIMITED" => CreateError(
                StatusCodes.Status429TooManyRequests,
                "RATE_LIMIT_EXCEEDED",
                language,
                error.RetryAfter),
            "TIMEOUT" => CreateError(
                StatusCodes.Status504GatewayTimeout,
                "IMAGE_GENERATION_TIMEOUT",
                language),
            "UNAVAILABLE" => CreateError(
                StatusCodes.Status502BadGateway,
                "IMAGE_GENERATION_FAILED",
                language),
            "INVALID_RESPONSE" => CreateError(
                StatusCodes.Status502BadGateway,
                "IMAGE_GENERATION_FAILED",
                language),
            "OUTPUT_SIZE_EXCEEDED" => CreateError(
                StatusCodes.Status422UnprocessableEntity,
                "IMAGE_SIZE_LIMIT_EXCEEDED",
                language),
            _ => throw new InvalidOperationException(
                $"Port error '{error.ErrorCode.Value}' is not mapped yet."),
        };
    }

    private static ApiErrorMappingResult CreateError(
        int statusCode,
        string code,
        ApiLanguage language,
        int? retryAfter = null)
    {
        return new(
            statusCode,
            new ApiErrorResponse(code, ApiErrorMessageProvider.GetPublicMessage(language, code)),
            retryAfter);
    }
}
