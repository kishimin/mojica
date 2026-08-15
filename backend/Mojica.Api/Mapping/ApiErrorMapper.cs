using Mojica.Api.Contracts;
using Mojica.Api.Localization;
using Mojica.Api.Models;

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
}
