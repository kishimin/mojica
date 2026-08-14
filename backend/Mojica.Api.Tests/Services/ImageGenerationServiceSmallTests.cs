using Mojica.Api.Models;
using Mojica.Api.Ports;
using Mojica.Api.Services;

namespace Mojica.Api.Tests.Services;

public sealed class ImageGenerationServiceSmallTests
{
    [Fact]
    public async Task ImageGenerationService_GenerateAsync_WhenRequestIsValid_PassesSameRequestToPortOnce()
    {
        var request = CreateValidRequest();
        var port = new RecordingImageGenerationPort(CreateSuccessfulPortResult());
        var service = new ImageGenerationService(port);

        await service.GenerateAsync(request, CancellationToken.None);

        Assert.Same(request, port.ReceivedRequest);
        Assert.Equal(1, port.CallCount);
    }

    [Fact(Skip = "TODO: Implement when ImageGenerationService is introduced.")]
    public void ImageGenerationService_Generate_WhenPortSucceeds_PreservesContentAndMediaType()
    {
        // ID: SERVICE-03
        // Source: docs/v1/api/services.md sections 7 and 10
        // Given: a Port success containing known image bytes and media type
        // When: the Service completes the successful result
        // Then: GeneratedImage contains the same image content and media type from the Port result
        // Blocked by: ImageGenerationService is not implemented
        // Priority: High
    }

    [Fact(Skip = "TODO: Implement as a Theory when ImageGenerationService is introduced.")]
    public void ImageGenerationService_Generate_ForEachImageType_UsesNormalizedTypeAndUuidInFileName()
    {
        // ID: SERVICE-04
        // Source: docs/v1/api/services.md section 6 and section 10
        // Given: standard, x-background, or x-icon and a fixed UUID from a controllable source
        // When: the Service completes a successful Port result
        // Then: the filename is mojica-{normalizedImageType}-{UUID}.png for every documented image type
        // Blocked by: ImageGenerationService and its controllable UUID boundary are not implemented
        // Priority: High
        // Theory candidate: image type and exact expected filename vary; behavior is otherwise identical
    }

    [Fact(Skip = "TODO: Implement when ImageGenerationService is introduced.")]
    public void ImageGenerationService_Generate_WhenCalledTwiceSuccessfully_UsesNewUuidForEachFileName()
    {
        // ID: SERVICE-05
        // Source: docs/v1/api/services.md section 6
        // Given: two successful executions and a controllable UUID source returning two known UUIDs
        // When: the Service completes both results
        // Then: each filename contains the UUID generated for that execution and the filenames differ
        // Blocked by: ImageGenerationService and its controllable UUID boundary are not implemented
        // Priority: Medium
    }

    [Fact(Skip = "TODO: Implement when ImageGenerationService is introduced.")]
    public void ImageGenerationService_Generate_WhenRequestContainsUserText_ExcludesUserValuesFromFileName()
    {
        // ID: SERVICE-06
        // Source: docs/v1/api/services.md section 6
        // Given: a valid request with recognizable user-provided text, pattern characters, and colors
        // When: the Service completes a successful Port result with a fixed UUID
        // Then: the filename contains only the mojica prefix, normalized image type, UUID, and .png extension
        // Blocked by: ImageGenerationService and its controllable UUID boundary are not implemented
        // Priority: High
    }

    [Fact(Skip = "TODO: Implement as a Theory when ImageGenerationService is introduced.")]
    public void ImageGenerationService_Generate_WhenPortFails_ReturnsSamePortError()
    {
        // ID: SERVICE-07
        // Source: docs/v1/api/services.md sections 8 and 10
        // Given: a Port failure classified as RATE_LIMITED, TIMEOUT, UNAVAILABLE, INVALID_RESPONSE, or FAILED
        // When: the Service generates an image
        // Then: the Service returns the same ImageGenerationPortError instance with code and retryAfter unchanged
        // Error: preserve each documented Port failure without translation
        // Blocked by: ImageGenerationService is not implemented
        // Priority: High
        // Theory candidate: error code and optional retryAfter vary; propagation behavior is identical
    }

    [Fact(Skip = "TODO: Implement when ImageGenerationService is introduced.")]
    public void ImageGenerationService_Generate_WhenPortFails_DoesNotGenerateFileName()
    {
        // ID: SERVICE-08
        // Source: docs/v1/api/services.md section 10
        // Given: a failing fake ImageGenerationPort and an observable UUID source
        // When: the Service receives the Port failure
        // Then: the UUID source is not called and no GeneratedImage or filename is produced
        // Error: Port failure ends processing before successful result completion
        // Blocked by: ImageGenerationService and its controllable UUID boundary are not implemented
        // Priority: High
    }

    [Fact(Skip = "TODO: Implement when ImageGenerationService is introduced.")]
    public void ImageGenerationService_Generate_WhenPortFails_DoesNotRetry()
    {
        // ID: SERVICE-09
        // Source: docs/v1/api/services.md sections 9 and 10
        // Given: a fake ImageGenerationPort that returns a failure on its first call
        // When: the Service generates an image once
        // Then: the Port is called exactly once and no automatic retry is attempted
        // Error: retrying a side-effecting generation request could create duplicate images
        // Blocked by: ImageGenerationService is not implemented
        // Priority: High
    }

    private static ImageGenerationRequest CreateValidRequest(ImageType? type = null)
    {
        Assert.True(RenderText.TryCreate("Mojica", out var text, out _));
        Assert.True(PatternCharacter.TryCreate("@", out var foregroundCharacter, out _));
        Assert.True(HexColor.TryCreate("#FF69B4", out var foregroundColor, out _));
        Assert.True(PatternCharacter.TryCreate(".", out var backgroundCharacter, out _));
        Assert.True(HexColor.TryCreate("#000000", out var backgroundColor, out _));
        Assert.True(ImageGenerationRequest.TryCreate(
            type ?? ImageType.Standard,
            text,
            foregroundCharacter,
            foregroundColor,
            backgroundCharacter,
            backgroundColor,
            out var request,
            out _));

        return request;
    }

    private static ImageGenerationPortResult CreateSuccessfulPortResult()
    {
        return ImageGenerationPortResult.Success(
            new GeneratedImageData([0x89, 0x50, 0x4E, 0x47], "image/png"));
    }

    private sealed class RecordingImageGenerationPort(
        ImageGenerationPortResult result) : ImageGenerationPort
    {
        public int CallCount { get; private set; }

        public ImageGenerationRequest? ReceivedRequest { get; private set; }

        public Task<ImageGenerationPortResult> GenerateAsync(
            ImageGenerationRequest request,
            CancellationToken cancellationToken)
        {
            CallCount++;
            ReceivedRequest = request;
            return Task.FromResult(result);
        }
    }
}
