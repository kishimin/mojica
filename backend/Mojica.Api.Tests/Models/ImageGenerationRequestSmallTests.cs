using Mojica.Api.Models;

namespace Mojica.Api.Tests.Models;

public sealed class ImageGenerationRequestSmallTests
{
    [Fact]
    public void ImageGenerationRequest_Create_WhenAllValuesAreValid_Succeeds()
    {
        Assert.True(ImageType.TryCreate("standard", out var type, out _));
        Assert.True(RenderText.TryCreate("Mojica", out var text, out _));
        Assert.True(PatternCharacter.TryCreate("@", out var foregroundCharacter, out _));
        Assert.True(HexColor.TryCreate("#FF69B4", out var foregroundColor, out _));
        Assert.True(PatternCharacter.TryCreate(".", out var backgroundCharacter, out _));
        Assert.True(HexColor.TryCreate("#000000", out var backgroundColor, out _));

        var succeeded = ImageGenerationRequest.TryCreate(
            type,
            text,
            foregroundCharacter,
            foregroundColor,
            backgroundCharacter,
            backgroundColor,
            out var request,
            out var error);

        Assert.True(succeeded);
        Assert.NotNull(request);
        Assert.Same(type, request.Type);
        Assert.Same(text, request.Text);
        Assert.Same(foregroundCharacter, request.ForegroundCharacter);
        Assert.Same(foregroundColor, request.ForegroundColor);
        Assert.Same(backgroundCharacter, request.BackgroundCharacter);
        Assert.Same(backgroundColor, request.BackgroundColor);
        Assert.Null(error);
    }

    [Theory]
    [InlineData("type")]
    [InlineData("text")]
    [InlineData("foregroundCharacter")]
    [InlineData("foregroundColor")]
    [InlineData("backgroundCharacter")]
    [InlineData("backgroundColor")]
    public void ImageGenerationRequest_Create_WhenRequiredValueIsMissing_ReturnsRequiredError(
        string target)
    {
        Assert.True(ImageType.TryCreate("standard", out var type, out _));
        Assert.True(RenderText.TryCreate("Mojica", out var text, out _));
        Assert.True(PatternCharacter.TryCreate("@", out var foregroundCharacter, out _));
        Assert.True(HexColor.TryCreate("#FF69B4", out var foregroundColor, out _));
        Assert.True(PatternCharacter.TryCreate(".", out var backgroundCharacter, out _));
        Assert.True(HexColor.TryCreate("#000000", out var backgroundColor, out _));

        var succeeded = ImageGenerationRequest.TryCreate(
            target == "type" ? null : type,
            target == "text" ? null : text,
            target == "foregroundCharacter" ? null : foregroundCharacter,
            target == "foregroundColor" ? null : foregroundColor,
            target == "backgroundCharacter" ? null : backgroundCharacter,
            target == "backgroundColor" ? null : backgroundColor,
            out var request,
            out var error);

        Assert.False(succeeded);
        Assert.Null(request);
        Assert.NotNull(error);
        Assert.Equal(ModelValidationReason.Required, error.Reason);
        Assert.Equal("REQUIRED", error.Code);
        Assert.Equal(target, error.Target);
    }

    [Fact]
    public void ImageGenerationRequest_Create_WhenBothPatternValuesAreOnlyWhitespace_ReturnsVisibleCharacterRequiredError()
    {
        Assert.True(ImageType.TryCreate("standard", out var type, out _));
        Assert.True(RenderText.TryCreate("Mojica", out var text, out _));
        Assert.True(PatternCharacter.TryCreate(" ", out var foregroundCharacter, out _));
        Assert.True(HexColor.TryCreate("#FF69B4", out var foregroundColor, out _));
        Assert.True(PatternCharacter.TryCreate("\u3000", out var backgroundCharacter, out _));
        Assert.True(HexColor.TryCreate("#000000", out var backgroundColor, out _));

        var succeeded = ImageGenerationRequest.TryCreate(
            type,
            text,
            foregroundCharacter,
            foregroundColor,
            backgroundCharacter,
            backgroundColor,
            out var request,
            out var error);

        Assert.False(succeeded);
        Assert.Null(request);
        Assert.NotNull(error);
        Assert.Equal(ModelValidationReason.VisibleCharacterRequired, error.Reason);
        Assert.Equal("VISIBLE_CHARACTER_REQUIRED", error.Code);
        Assert.Equal("foregroundCharacter,backgroundCharacter", error.Target);
    }

    [Fact(Skip = "TODO: Implement from documented test plan.")]
    public void ImageGenerationRequest_Create_WhenOnlyForegroundPatternIsVisible_Succeeds()
    {
        // ID: REQUEST-04
        // Source: docs/v1/api/models.md §9 ImageGenerationRequest.
        // Given: a visible foregroundCharacter and a whitespace-only backgroundCharacter
        // When: ImageGenerationRequest creation is requested
        // Then: creation succeeds
        // Priority: High
    }

    [Fact(Skip = "TODO: Implement from documented test plan.")]
    public void ImageGenerationRequest_Create_WhenOnlyBackgroundPatternIsVisible_Succeeds()
    {
        // ID: REQUEST-05
        // Source: docs/v1/api/models.md §9 ImageGenerationRequest.
        // Given: a whitespace-only foregroundCharacter and a visible backgroundCharacter
        // When: ImageGenerationRequest creation is requested
        // Then: creation succeeds
        // Priority: High
    }

    [Fact(Skip = "TODO: Implement from documented test plan.")]
    public void ImageGenerationRequest_Create_WhenValueObjectCreationFailed_DoesNotCreateAggregate()
    {
        // ID: REQUEST-06
        // Source: docs/v1/api/models.md §9 ImageGenerationRequest.
        // Given: each Value Object creation failure in turn
        // When: the caller attempts to proceed to ImageGenerationRequest creation
        // Then: no ImageGenerationRequest is produced and the original ModelValidationError remains classifiable
        // Priority: High
    }
}
