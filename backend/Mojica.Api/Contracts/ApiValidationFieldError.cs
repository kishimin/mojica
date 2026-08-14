using System.Text.Json.Serialization;

namespace Mojica.Api.Contracts;

public sealed record ApiValidationFieldError
{
    public ApiValidationFieldError(string field, string message)
    {
        ArgumentNullException.ThrowIfNull(field);
        ArgumentNullException.ThrowIfNull(message);

        Field = field;
        Message = message;
    }

    [JsonPropertyName("field")]
    public string Field { get; }

    [JsonPropertyName("message")]
    public string Message { get; }
}
