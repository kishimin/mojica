using Mojica.Api.Contracts;
using Mojica.Api.Localization;
using Mojica.Api.Models;
using Mojica.Api.Ports;

namespace Mojica.Api.Mapping;

public static class ApiErrorMapper
{
    public static ApiErrorMappingResult MapMalformedRequest(ApiLanguage language)
    {
        return new(
            StatusCodes.Status400BadRequest,
            new ApiErrorResponse(
                PublicApiErrorCode.BadRequest,
                ApiErrorMessageProvider.GetPublicMessage(language, PublicApiErrorCode.BadRequest)));
    }

    public static ApiErrorMappingResult MapValidationFailure(
        IEnumerable<ModelValidationError> errors,
        ApiLanguage language)
    {
        ArgumentNullException.ThrowIfNull(errors);

        var errorList = errors.ToList();
        if (errorList.Count == 0)
        {
            throw new ArgumentException(
                "Validation failure mapping requires at least one validation error.",
                nameof(errors));
        }

        var fieldErrors = errorList
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
                PublicApiErrorCode.ValidationError,
                ApiErrorMessageProvider.GetPublicMessage(language, PublicApiErrorCode.ValidationError),
                fieldErrors));
    }

    public static ApiErrorMappingResult MapPortFailure(
        ImageGenerationPortError error,
        ApiLanguage language)
    {
        ArgumentNullException.ThrowIfNull(error);

        var code = error.ErrorCode;
        return code switch
        {
            _ when code == ImageGenerationPortErrorCode.RateLimited => CreateError(
                StatusCodes.Status429TooManyRequests,
                PublicApiErrorCode.RateLimitExceeded,
                language,
                error.RetryAfter),
            _ when code == ImageGenerationPortErrorCode.Timeout => CreateError(
                StatusCodes.Status504GatewayTimeout,
                PublicApiErrorCode.ImageGenerationTimeout,
                language),
            _ when code == ImageGenerationPortErrorCode.Unavailable
                || code == ImageGenerationPortErrorCode.InvalidResponse
                || code == ImageGenerationPortErrorCode.Failed => CreateError(
                StatusCodes.Status502BadGateway,
                PublicApiErrorCode.ImageGenerationFailed,
                language),
            _ when code == ImageGenerationPortErrorCode.OutputSizeExceeded => CreateError(
                StatusCodes.Status422UnprocessableEntity,
                PublicApiErrorCode.ImageSizeLimitExceeded,
                language),
            _ => throw new InvalidOperationException(
                $"Port error '{code.Value}' is not mapped yet."),
        };
    }

    public static ApiErrorMappingResult MapUnexpectedFailure(ApiLanguage language)
    {
        return CreateError(
            StatusCodes.Status500InternalServerError,
            PublicApiErrorCode.InternalServerError,
            language);
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
