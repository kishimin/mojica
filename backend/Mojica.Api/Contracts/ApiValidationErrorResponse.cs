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

        var errorList = errors.ToList();
        if (errorList.Count == 0)
        {
            throw new ArgumentException(
                "A validation error response requires at least one field error.",
                nameof(errors));
        }

        Code = code;
        Message = message;
        Errors = new ReadOnlyCollection<ApiValidationFieldError>(errorList);
    }

    [JsonPropertyName("code")]
    public string Code { get; }

    [JsonPropertyName("message")]
    public string Message { get; }

    [JsonPropertyName("errors")]
    public IReadOnlyList<ApiValidationFieldError> Errors { get; }
}
