using Mojica.Api.Contracts;
using Mojica.Api.Localization;

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
}
