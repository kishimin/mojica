using Mojica.Api.Contracts;

namespace Mojica.Api.Mapping;

public sealed record ApiErrorMappingResult(int StatusCode, ApiErrorResponse Response);
