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

    [Fact]
    public async Task ImageGenerationService_GenerateAsync_WhenPortSucceeds_PreservesContentAndMediaType()
    {
        var request = CreateValidRequest();
        byte[] content = [0x89, 0x50, 0x4E, 0x47];
        const string mediaType = "image/png";
        var imageData = new GeneratedImageData(content, mediaType);
        var port = new RecordingImageGenerationPort(
            ImageGenerationPortResult.Success(imageData));
        var service = new ImageGenerationService(port);

        var result = await service.GenerateAsync(request, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Image);
        Assert.Same(content, result.Image.Content);
        Assert.Equal(mediaType, result.Image.MediaType);
        Assert.Null(result.Error);
    }

    [Theory]
    [InlineData("standard", "mojica-standard-550e8400-e29b-41d4-a716-446655440000.png")]
    [InlineData("x-background", "mojica-x-background-550e8400-e29b-41d4-a716-446655440000.png")]
    [InlineData("x-icon", "mojica-x-icon-550e8400-e29b-41d4-a716-446655440000.png")]
    public async Task ImageGenerationService_GenerateAsync_ForEachImageType_UsesTypeAndUuidOnlyInFileName(
        string imageTypeValue,
        string expectedFileName)
    {
        Assert.True(ImageType.TryCreate(imageTypeValue, out var imageType, out _));
        var request = CreateValidRequest(imageType);
        var port = new RecordingImageGenerationPort(CreateSuccessfulPortResult());
        var uuidProvider = new StubUuidProvider(
            Guid.Parse("550e8400-e29b-41d4-a716-446655440000"));
        var service = new ImageGenerationService(port, uuidProvider);

        var result = await service.GenerateAsync(request, CancellationToken.None);

        Assert.NotNull(result.Image);
        Assert.Equal(expectedFileName, result.Image.FileName);
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

    private sealed class StubUuidProvider(Guid value) : UuidProvider
    {
        public Guid Create()
        {
            return value;
        }
    }
}
