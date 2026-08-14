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

    [Fact(Skip = "TODO: Represent omitted values for Domain validation.")]
    public void Deserialize_WhenRequiredFieldIsOmitted_LeavesThatDtoValueMissing()
    {
        // ID: REQUEST-DTO-02
        // Source: docs/v1/api/controllers.md §3-5; docs/v1/api/api.md §6 and §11.
        // Given: parseable JSON with one required request field omitted (Theory candidate: all six fields)
        // When: System.Text.Json deserializes the body into the request DTO
        // Then: deserialization produces a DTO whose omitted value can be classified as REQUIRED by the input Mapper
        // Error: omission is a Domain validation input here; malformed JSON and incompatible JSON types remain Endpoint Medium-test responsibilities
        // Blocked by: define ImageGenerationRequestDto
        // Priority: High
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
