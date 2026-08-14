namespace Mojica.Api.Tests.Contracts;

public sealed class ImageGenerationRequestDtoSmallTests
{
    [Fact(Skip = "TODO: Implement the request DTO value contract.")]
    public void Deserialize_WhenJsonContainsEveryRequestField_RetainsRawInputValues()
    {
        // ID: REQUEST-DTO-01
        // Source: docs/v1/api/controllers.md §3; docs/v1/api/api.md §5.
        // Given: JSON containing type, text, foregroundCharacter, foregroundColor, backgroundCharacter, and backgroundColor
        // When: System.Text.Json deserializes the body into the request DTO
        // Then: every raw string value is available to the input Mapper under the documented camelCase property name
        // Blocked by: define ImageGenerationRequestDto
        // Priority: High
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
