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
    public void Failure_WhenErrorsAreNull_ThrowsArgumentNullExceptionForErrorsParameter()
    {
        var exception = Assert.Throws<ArgumentNullException>(() =>
            ImageGenerationRequestMappingResult.Failure(null!));

        Assert.Equal("errors", exception.ParamName);
    }

    [Fact]
    public void Failure_WhenErrorsAreEmpty_ThrowsArgumentException()
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            ImageGenerationRequestMappingResult.Failure(
                Array.Empty<ModelValidationError>()));

        Assert.StartsWith("A failed mapping requires at least one error.", exception.Message);
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

    [Fact]
    public void Failure_WhenErrorsHaveMatchingContents_UsesErrorValues()
    {
        var first = ImageGenerationRequestMappingResult.Failure(
            [new ModelValidationError("text", ModelValidationReason.Required)]);
        var second = ImageGenerationRequestMappingResult.Failure(
            [new ModelValidationError("text", ModelValidationReason.Required)]);

        Assert.Equal(first, second);
        Assert.Equal(first.GetHashCode(), second.GetHashCode());
    }

    [Fact]
    public void Failure_WhenErrorsDiffer_IsNotEqualAndHashCodeDiffers()
    {
        var first = ImageGenerationRequestMappingResult.Failure(
            [new ModelValidationError("text", ModelValidationReason.Required)]);
        var second = ImageGenerationRequestMappingResult.Failure(
            [new ModelValidationError("foregroundColor", ModelValidationReason.InvalidHexColor)]);

        Assert.NotEqual(first, second);
        Assert.NotEqual(first.GetHashCode(), second.GetHashCode());
    }

    [Fact]
    public void Success_WhenRequestsDiffer_IsNotEqualAndHashCodeDiffers()
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
            out var firstRequest,
            out _));
        Assert.True(RenderText.TryCreate("Different", out var differentText, out _));
        Assert.True(ImageGenerationRequest.TryCreate(
            type,
            differentText,
            foregroundCharacter,
            foregroundColor,
            backgroundCharacter,
            backgroundColor,
            out var secondRequest,
            out _));

        var first = ImageGenerationRequestMappingResult.Success(firstRequest);
        var second = ImageGenerationRequestMappingResult.Success(secondRequest);

        Assert.NotEqual(first, second);
        Assert.NotEqual(first.GetHashCode(), second.GetHashCode());
    }
}
