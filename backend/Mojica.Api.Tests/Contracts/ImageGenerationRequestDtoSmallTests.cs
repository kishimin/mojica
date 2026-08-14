using System.Text.Json;
using Mojica.Api.Contracts;

namespace Mojica.Api.Tests.Contracts;

public sealed class ImageGenerationRequestDtoSmallTests
{
    [Fact]
    public void Deserialize_WhenJsonContainsEveryRequestField_RetainsRawInputValues()
    {
        const string json = """
            {
              "type": "x-icon",
              "text": "KA",
              "foregroundCharacter": "🌻",
              "foregroundColor": "#FFD400",
              "backgroundCharacter": "☀",
              "backgroundColor": "#FF69B4"
            }
            """;

        var request = JsonSerializer.Deserialize<ImageGenerationRequestDto>(json);

        Assert.NotNull(request);
        Assert.Equal("x-icon", request.Type);
        Assert.Equal("KA", request.Text);
        Assert.Equal("🌻", request.ForegroundCharacter);
        Assert.Equal("#FFD400", request.ForegroundColor);
        Assert.Equal("☀", request.BackgroundCharacter);
        Assert.Equal("#FF69B4", request.BackgroundColor);
    }

    [Theory]
    [InlineData("type")]
    [InlineData("text")]
    [InlineData("foregroundCharacter")]
    [InlineData("foregroundColor")]
    [InlineData("backgroundCharacter")]
    [InlineData("backgroundColor")]
    public void Deserialize_WhenRequiredFieldIsOmitted_LeavesThatDtoValueMissing(
        string omittedField)
    {
        var values = new Dictionary<string, string>
        {
            ["type"] = "standard",
            ["text"] = "Mojica",
            ["foregroundCharacter"] = "@",
            ["foregroundColor"] = "#FFFFFF",
            ["backgroundCharacter"] = ".",
            ["backgroundColor"] = "#000000",
        };
        values.Remove(omittedField);
        var json = JsonSerializer.Serialize(values);

        var request = JsonSerializer.Deserialize<ImageGenerationRequestDto>(json);

        Assert.NotNull(request);
        var omittedValue = omittedField switch
        {
            "type" => request.Type,
            "text" => request.Text,
            "foregroundCharacter" => request.ForegroundCharacter,
            "foregroundColor" => request.ForegroundColor,
            "backgroundCharacter" => request.BackgroundCharacter,
            "backgroundColor" => request.BackgroundColor,
            _ => throw new ArgumentOutOfRangeException(nameof(omittedField)),
        };
        Assert.Null(omittedValue);
    }

    [Fact(Skip = "TODO: Keep unsupported image type values available for validation.")]
    public void Deserialize_WhenTypeIsUnsupported_RetainsTheRawTypeValue()
    {
        // ID: REQUEST-DTO-03
        // Source: docs/v1/api/api.md §5 type and §11 Invalid type.
        // Given: parseable JSON whose type string is not standard, x-background, or x-icon
        // When: System.Text.Json deserializes the body into the request DTO
        // Then: the unsupported string reaches the Mapper so it can return UNSUPPORTED_IMAGE_TYPE for field type
        // Error: do not collapse an unsupported value into a JSON-format error before Domain validation
        // Blocked by: define ImageGenerationRequestDto
        // Priority: High
    }
}
