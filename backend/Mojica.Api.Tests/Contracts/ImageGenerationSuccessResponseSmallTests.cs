using Mojica.Api.Contracts;

namespace Mojica.Api.Tests.Contracts;

public sealed class ImageGenerationSuccessResponseSmallTests
{
    public static TheoryData<byte[]?, string?, string?> MissingValues => new()
    {
        { null, "image/png", "mojica-standard-123.png" },
        { [], null, "mojica-standard-123.png" },
        { [], "image/png", null },
    };

    [Fact]
    public void Create_WhenGeneratedImageIsValid_RetainsContentMediaTypeAndFileName()
    {
        // ID: SUCCESS-RESPONSE-01
        // Source: docs/v1/api/controllers.md §7; docs/v1/api/implementation-plan.md §4 branch 7B.
        // Given: generated PNG bytes, image/png media type, and the Service-generated safe filename
        // When: the successful public response contract is created
        // Then: it retains the exact content, media type, and filename for the Endpoint to return
        byte[] content = [0x89, 0x50, 0x4E, 0x47];

        var response = new ImageGenerationSuccessResponse(
            content,
            "image/png",
            "mojica-standard-123.png");

        Assert.Same(content, response.Content);
        Assert.Equal("image/png", response.MediaType);
        Assert.Equal("mojica-standard-123.png", response.FileName);

        // Priority: High
    }

    [Theory]
    [MemberData(nameof(MissingValues))]
    public void Create_WhenRequiredValueIsNull_ThrowsArgumentNullException(
        byte[]? content,
        string? mediaType,
        string? fileName)
    {
        // ID: SUCCESS-RESPONSE-02
        // Source: docs/v1/api/controllers.md §7; ADR-0026 result-variant invariant principle.
        // Given: null for content, media type, or filename in turn
        // When: the successful public response contract is created
        // Then: construction throws ArgumentNullException for the missing required value
        // Error: no successful response instance may omit content, media type, or filename
        Assert.Throws<ArgumentNullException>(() =>
            new ImageGenerationSuccessResponse(content!, mediaType!, fileName!));

        // Priority: Medium
    }
}
