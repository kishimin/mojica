using Mojica.Api.Models;
using Mojica.Api.Ports;

namespace Mojica.Api.Tests.Ports;

public sealed class ImageGenerationPortSmallTests
{
    public static TheoryData<ImageGenerationPortErrorCode> FailureCodes => new()
    {
        ImageGenerationPortErrorCode.RateLimited,
        ImageGenerationPortErrorCode.Timeout,
        ImageGenerationPortErrorCode.Unavailable,
        ImageGenerationPortErrorCode.InvalidResponse,
        ImageGenerationPortErrorCode.OutputSizeExceeded,
        ImageGenerationPortErrorCode.Failed,
    };

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

    [Theory]
    [MemberData(nameof(FailureCodes))]
    public async Task ImageGenerationPort_Generate_WhenGenerationFails_ReturnsClassifiedPortError(
        ImageGenerationPortErrorCode errorCode)
    {
        var request = CreateValidRequest();
        var error = new ImageGenerationPortError(errorCode);
        ImageGenerationPort port = new StubImageGenerationPort(
            ImageGenerationPortResult.Failure(error));

        var result = await port.GenerateAsync(request, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Null(result.Data);
        Assert.Same(error, result.Error);
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
