using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mojica.Api.Contracts;

public sealed class ApiErrorResponseJsonConverter : JsonConverter<IApiErrorResponse>
{
    public override bool CanConvert(Type typeToConvert) => typeToConvert == typeof(IApiErrorResponse);

    public override IApiErrorResponse? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        using var document = JsonDocument.ParseValue(ref reader);
        var root = document.RootElement;

        return root.TryGetProperty("errors", out _)
            ? root.Deserialize<ApiValidationErrorResponse>(options)
            : root.Deserialize<ApiErrorResponse>(options);
    }

    public override void Write(Utf8JsonWriter writer, IApiErrorResponse value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case ApiValidationErrorResponse validationErrorResponse:
                JsonSerializer.Serialize(writer, validationErrorResponse, options);
                break;
            case ApiErrorResponse errorResponse:
                JsonSerializer.Serialize(writer, errorResponse, options);
                break;
            default:
                throw new NotSupportedException(
                    $"'{value.GetType()}' is not a supported IApiErrorResponse implementation.");
        }
    }
}
