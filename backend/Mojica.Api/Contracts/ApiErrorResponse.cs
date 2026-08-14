using System.Text.Json.Serialization;

namespace Mojica.Api.Contracts;

public sealed record ApiErrorResponse
{
    public ApiErrorResponse(string code, string message)
    {
        ArgumentNullException.ThrowIfNull(code);
        ArgumentNullException.ThrowIfNull(message);

        Code = code;
        Message = message;
    }

    [JsonPropertyName("code")]
    public string Code { get; }

    [JsonPropertyName("message")]
    public string Message { get; }
}
