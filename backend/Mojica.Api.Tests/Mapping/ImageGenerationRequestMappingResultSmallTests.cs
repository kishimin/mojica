using Mojica.Api.Mapping;
using Mojica.Api.Models;

namespace Mojica.Api.Tests.Mapping;

public sealed class ImageGenerationRequestMappingResultSmallTests
{
    [Fact]
    public void Success_WhenRequestIsNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            ImageGenerationRequestMappingResult.Success(null!));
    }

    [Fact]
    public void Failure_WhenErrorsAreNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            ImageGenerationRequestMappingResult.Failure(null!));
    }

    [Fact]
    public void Failure_WhenErrorsAreEmpty_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
            ImageGenerationRequestMappingResult.Failure(
                Array.Empty<ModelValidationError>()));
    }

    [Fact]
    public void Success_WhenCreatedWithTheSameRequest_IsValueEqual()
    {
        Assert.True(ImageType.TryCreate("standard", out var type, out _));
        Assert.True(RenderText.TryCreate("Mojica", out var text, out _));
        Assert.True(PatternCharacter.TryCreate("@", out var foregroundCharacter, out _));
        Assert.True(HexColor.TryCreate("#FFFFFF", out var foregroundColor, out _));
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

        var first = ImageGenerationRequestMappingResult.Success(request);
        var second = ImageGenerationRequestMappingResult.Success(request);

        Assert.Equal(first, second);
    }
}
