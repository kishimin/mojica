namespace Mojica.Api.Tests.Mapping;

public sealed class ImageGenerationRequestMapperSmallTests
{
    [Fact(Skip = "TODO: Implement after the image request DTO and Mapper contracts exist.")]
    public void ImageGenerationRequestMapper_Map_WhenValueObjectCreationFails_ReturnsClassifiableFieldErrors()
    {
        // ID: REQUEST-MAPPING-01
        // Source: docs/v1/api/controllers.md §3-4; ADR-0022.
        // Given: each invalid request attribute in turn, including context-free PatternCharacter and HexColor failures (Theory candidate)
        // When: the input Mapper converts the HTTP DTO values into an ImageGenerationRequest
        // Then: no aggregate is returned, each reason remains machine-classifiable, and each error identifies its request attribute
        // Error: preserve the original ModelValidationReason and assign type, text, foregroundCharacter, foregroundColor, backgroundCharacter, or backgroundColor as Target
        // Blocked by: feature/add-image-api-contracts must define the request DTO and input Mapper boundary
        // Priority: High
    }
}
