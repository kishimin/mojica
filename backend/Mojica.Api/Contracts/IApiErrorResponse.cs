using System.Text.Json.Serialization;

namespace Mojica.Api.Contracts;

[JsonPolymorphic]
[JsonDerivedType(typeof(ApiErrorResponse))]
[JsonDerivedType(typeof(ApiValidationErrorResponse))]
public interface IApiErrorResponse
{
    string Code { get; }

    string Message { get; }
}
