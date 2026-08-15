using System.Text.Json.Serialization;

namespace Mojica.Api.Contracts;

[JsonConverter(typeof(ApiErrorResponseJsonConverter))]
public interface IApiErrorResponse
{
    string Code { get; }

    string Message { get; }
}
