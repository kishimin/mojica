namespace Mojica.Api.Tests.Contracts;

public sealed class ImageGenerationSuccessResponseSmallTests
{
    [Fact(Skip = "TODO: Implement the stable successful image response contract.")]
    public void Create_WhenGeneratedImageIsValid_RetainsContentMediaTypeAndFileName()
    {
        // ID: SUCCESS-RESPONSE-01
        // Source: docs/v1/api/controllers.md §7; docs/v1/api/implementation-plan.md §4 branch 7B.
        // Given: generated PNG bytes, image/png media type, and the Service-generated safe filename
        // When: the successful public response contract is created
        // Then: it retains the exact content, media type, and filename for the Endpoint to return
        // Blocked by: define ImageGenerationSuccessResponse
        // Priority: High
    }

    [Fact(Skip = "TODO: Require complete successful image response data.")]
    public void Create_WhenRequiredValueIsNull_ThrowsArgumentNullException()
    {
        // ID: SUCCESS-RESPONSE-02
        // Source: docs/v1/api/controllers.md §7; ADR-0026 result-variant invariant principle.
        // Given: null for content, media type, or filename in turn (Theory candidate)
        // When: the successful public response contract is created
        // Then: construction throws ArgumentNullException for the missing required value
        // Error: no successful response instance may omit content, media type, or filename
        // Blocked by: define ImageGenerationSuccessResponse
        // Priority: Medium
    }
}
