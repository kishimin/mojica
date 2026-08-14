using Mojica.Api.Models;
using Mojica.Api.Ports;
using Mojica.Api.Services;

namespace Mojica.Api.Tests.Services;

public sealed class ImageGenerationServiceSmallTests
{
    public static TheoryData<ImageGenerationPortErrorCode, int?> PortFailures => new()
    {
        { ImageGenerationPortErrorCode.RateLimited, 60 },
        { ImageGenerationPortErrorCode.Timeout, null },
        { ImageGenerationPortErrorCode.Unavailable, null },
        { ImageGenerationPortErrorCode.InvalidResponse, null },
        { ImageGenerationPortErrorCode.Failed, null },
    };

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
        var uuidProvider = new RecordingUuidProvider(
            Guid.Parse("550e8400-e29b-41d4-a716-446655440000"));
        var service = new ImageGenerationService(port, uuidProvider);

        var result = await service.GenerateAsync(request, CancellationToken.None);

        Assert.NotNull(result.Image);
        Assert.Equal(expectedFileName, result.Image.FileName);
    }

    [Fact]
    public async Task ImageGenerationService_GenerateAsync_WhenCalledTwiceSuccessfully_UsesNewUuidForEachFileName()
    {
        var request = CreateValidRequest();
        var port = new RecordingImageGenerationPort(CreateSuccessfulPortResult());
        var uuidProvider = new RecordingUuidProvider(
            Guid.Parse("550e8400-e29b-41d4-a716-446655440000"),
            Guid.Parse("6ba7b810-9dad-11d1-80b4-00c04fd430c8"));
        var service = new ImageGenerationService(port, uuidProvider);

        var first = await service.GenerateAsync(request, CancellationToken.None);
        var second = await service.GenerateAsync(request, CancellationToken.None);

        Assert.NotNull(first.Image);
        Assert.NotNull(second.Image);
        Assert.Equal(
            "mojica-standard-550e8400-e29b-41d4-a716-446655440000.png",
            first.Image.FileName);
        Assert.Equal(
            "mojica-standard-6ba7b810-9dad-11d1-80b4-00c04fd430c8.png",
            second.Image.FileName);
        Assert.NotEqual(first.Image.FileName, second.Image.FileName);
    }

    [Theory]
    [MemberData(nameof(PortFailures))]
    public async Task ImageGenerationService_GenerateAsync_WhenPortFails_PropagatesFailureWithoutUuidOrRetry(
        ImageGenerationPortErrorCode errorCode,
        int? retryAfter)
    {
        var request = CreateValidRequest();
        var error = new ImageGenerationPortError(errorCode, retryAfter);
        var port = new RecordingImageGenerationPort(
            ImageGenerationPortResult.Failure(error));
        var uuidProvider = new RecordingUuidProvider(Guid.NewGuid());
        var service = new ImageGenerationService(port, uuidProvider);

        var result = await service.GenerateAsync(request, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Null(result.Image);
        Assert.Same(error, result.Error);
        Assert.Equal(0, uuidProvider.CallCount);
        Assert.Equal(1, port.CallCount);
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

    private sealed class RecordingUuidProvider(params Guid[] values) : UuidProvider
    {
        private readonly Queue<Guid> remainingValues = new(values);

        public int CallCount { get; private set; }

        public Guid Create()
        {
            CallCount++;
            return remainingValues.Dequeue();
        }
    }
}
