using Mojica.Api.Models;

namespace Mojica.Api.Tests.Models;

public sealed class ImageGenerationRequestSmallTests
{
    [Fact]
    public void ImageGenerationRequest_Create_WhenAllValuesAreValid_Succeeds()
    {
        var values = CreateValidValues();

        var succeeded = ImageGenerationRequest.TryCreate(
            values.Type,
            values.Text,
            values.ForegroundCharacter,
            values.ForegroundColor,
            values.BackgroundCharacter,
            values.BackgroundColor,
            out var request,
            out var error);

        Assert.True(succeeded);
        Assert.NotNull(request);
        Assert.Same(values.Type, request.Type);
        Assert.Same(values.Text, request.Text);
        Assert.Same(values.ForegroundCharacter, request.ForegroundCharacter);
        Assert.Same(values.ForegroundColor, request.ForegroundColor);
        Assert.Same(values.BackgroundCharacter, request.BackgroundCharacter);
        Assert.Same(values.BackgroundColor, request.BackgroundColor);
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
        var values = CreateValidValues();

        var succeeded = ImageGenerationRequest.TryCreate(
            target == "type" ? null : values.Type,
            target == "text" ? null : values.Text,
            target == "foregroundCharacter" ? null : values.ForegroundCharacter,
            target == "foregroundColor" ? null : values.ForegroundColor,
            target == "backgroundCharacter" ? null : values.BackgroundCharacter,
            target == "backgroundColor" ? null : values.BackgroundColor,
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
        var values = CreateValidValues(" ", "\u3000");

        var succeeded = ImageGenerationRequest.TryCreate(
            values.Type,
            values.Text,
            values.ForegroundCharacter,
            values.ForegroundColor,
            values.BackgroundCharacter,
            values.BackgroundColor,
            out var request,
            out var error);

        Assert.False(succeeded);
        Assert.Null(request);
        Assert.NotNull(error);
        Assert.Equal(ModelValidationReason.VisibleCharacterRequired, error.Reason);
        Assert.Equal("VISIBLE_CHARACTER_REQUIRED", error.Code);
        Assert.Equal("foregroundCharacter,backgroundCharacter", error.Target);
    }

    [Theory]
    [InlineData("@", " ")]
    [InlineData(" ", "@")]
    public void ImageGenerationRequest_Create_WhenEitherPatternIsVisible_Succeeds(
        string foregroundValue,
        string backgroundValue)
    {
        var values = CreateValidValues(foregroundValue, backgroundValue);

        var succeeded = ImageGenerationRequest.TryCreate(
            values.Type,
            values.Text,
            values.ForegroundCharacter,
            values.ForegroundColor,
            values.BackgroundCharacter,
            values.BackgroundColor,
            out var request,
            out var error);

        Assert.True(succeeded);
        Assert.NotNull(request);
        Assert.Null(error);
    }

    private static (
        ImageType Type,
        RenderText Text,
        PatternCharacter ForegroundCharacter,
        HexColor ForegroundColor,
        PatternCharacter BackgroundCharacter,
        HexColor BackgroundColor) CreateValidValues(
            string foregroundCharacter = "@",
            string backgroundCharacter = ".")
    {
        Assert.True(ImageType.TryCreate("standard", out var type, out _));
        Assert.True(RenderText.TryCreate("Mojica", out var text, out _));
        Assert.True(PatternCharacter.TryCreate(
            foregroundCharacter,
            out var validatedForegroundCharacter,
            out _));
        Assert.True(HexColor.TryCreate("#FF69B4", out var foregroundColor, out _));
        Assert.True(PatternCharacter.TryCreate(
            backgroundCharacter,
            out var validatedBackgroundCharacter,
            out _));
        Assert.True(HexColor.TryCreate("#000000", out var backgroundColor, out _));

        return (
            type,
            text,
            validatedForegroundCharacter,
            foregroundColor,
            validatedBackgroundCharacter,
            backgroundColor);
    }
}
