using Mojica.Api.Models;
using Mojica.Api.Ports;

namespace Mojica.Api.Tests.Ports;

public sealed class ImageGenerationPortSmallTests
{
    [Fact]
    public async Task ImageGenerationPort_Generate_WhenRequestIsValid_ReturnsGeneratedImageData()
    {
        var request = CreateValidRequest();
        var imageData = new GeneratedImageData(
            [0x89, 0x50, 0x4E, 0x47],
            "image/png");
        ImageGenerationPort port = new StubImageGenerationPort(
            ImageGenerationPortResult.Success(imageData));

        var result = await port.GenerateAsync(request, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Same(imageData, result.Data);
        Assert.Null(result.Error);
    }

    [Fact(Skip = "TODO: Implement with the first ImageGenerationPort implementation.")]
    public void ImageGenerationPort_Generate_WhenGenerationFails_ReturnsClassifiedPortError()
    {
        // ID: PORT-02
        // Source: docs/v1/api/ports.md §3-4.
        // Given: a validated ImageGenerationRequest and each failure condition in turn (Theory candidate)
        // When: image generation is requested through ImageGenerationPort
        // Then: the Port returns RATE_LIMITED, TIMEOUT, UNAVAILABLE, INVALID_RESPONSE, or FAILED for the corresponding condition
        // Error: preserve a safe retryAfter value only when its retry period can be determined
        // Blocked by: feature/add-glyph-forge-adapter must provide the first observable Port implementation
        // Priority: High
    }

    private static ImageGenerationRequest CreateValidRequest()
    {
        Assert.True(ImageType.TryCreate("standard", out var type, out _));
        Assert.True(RenderText.TryCreate("Mojica", out var text, out _));
        Assert.True(PatternCharacter.TryCreate("@", out var foregroundCharacter, out _));
        Assert.True(HexColor.TryCreate("#FF69B4", out var foregroundColor, out _));
        Assert.True(PatternCharacter.TryCreate(".", out var backgroundCharacter, out _));
        Assert.True(HexColor.TryCreate("#000000", out var backgroundColor, out _));
        Assert.True(ImageGenerationRequest.TryCreate(
            type,
            text,
            foregroundCharacter,
            foregroundColor,
            backgroundCharacter,
            backgroundColor,
            out var request,
            out _));

        return request;
    }

    private sealed class StubImageGenerationPort(
        ImageGenerationPortResult result) : ImageGenerationPort
    {
        public Task<ImageGenerationPortResult> GenerateAsync(
            ImageGenerationRequest request,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(result);
        }
    }
}
