using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Mojica.Api.Contracts;

public sealed record ApiValidationErrorResponse
{
    public ApiValidationErrorResponse(
        string code,
        string message,
        IEnumerable<ApiValidationFieldError> errors)
    {
        ArgumentNullException.ThrowIfNull(code);
        ArgumentNullException.ThrowIfNull(message);
        ArgumentNullException.ThrowIfNull(errors);

        Code = code;
        Message = message;
        Errors = new ReadOnlyCollection<ApiValidationFieldError>(errors.ToList());
    }

    [JsonPropertyName("code")]
    public string Code { get; }

    [JsonPropertyName("message")]
    public string Message { get; }

    [JsonPropertyName("errors")]
    public IReadOnlyList<ApiValidationFieldError> Errors { get; }
}
