namespace Mojica.Api.Tests.Endpoints;

public sealed class ImageGenerationEndpointMediumTests
{
    [Fact(Skip = "TODO: Implement after the image generation Service and POST /images endpoint exist.")]
    public void PostImages_WhenRequestValueIsInvalid_ReturnsUnprocessableEntityWithoutCallingService()
    {
        // ID: REQUEST-ENDPOINT-01
        // Source: docs/v1/api/controllers.md §4-5 and §10; docs/v1/api/api.md §6.
        // Given: parseable POST /images JSON containing each Domain-invalid attribute in turn and a controlled Service fake (Theory candidate)
        // When: the client sends the request through WebApplicationFactory
        // Then: the API returns 422 VALIDATION_ERROR with the affected field and does not invoke the image generation Service
        // Error: 422 Unprocessable Entity; code VALIDATION_ERROR; field matches the invalid request attribute
        // Blocked by: feature/add-image-generation-service, feature/add-image-api-contracts, feature/add-api-error-mapping, and feature/add-image-generation-endpoint
        // Priority: High
    }
}
