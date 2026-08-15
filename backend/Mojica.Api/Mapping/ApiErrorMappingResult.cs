using Mojica.Api.Contracts;

namespace Mojica.Api.Mapping;

public sealed record ApiErrorMappingResult
{
    public ApiErrorMappingResult(int statusCode, IApiErrorResponse response, int? retryAfter = null)
    {
        ArgumentNullException.ThrowIfNull(response);

        StatusCode = statusCode;
        Response = response;
        RetryAfter = retryAfter;
    }

    public int StatusCode { get; }

    public IApiErrorResponse Response { get; }

    public int? RetryAfter { get; }
}
