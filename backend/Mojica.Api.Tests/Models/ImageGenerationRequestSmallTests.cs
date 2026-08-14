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
    [InlineData(MissingValue.Type, "type")]
    [InlineData(MissingValue.Text, "text")]
    [InlineData(MissingValue.ForegroundCharacter, "foregroundCharacter")]
    [InlineData(MissingValue.ForegroundColor, "foregroundColor")]
    [InlineData(MissingValue.BackgroundCharacter, "backgroundCharacter")]
    [InlineData(MissingValue.BackgroundColor, "backgroundColor")]
    public void ImageGenerationRequest_Create_WhenRequiredValueIsMissing_ReturnsRequiredError(
        MissingValue missingValue,
        string target)
    {
        var values = CreateValidValues().Without(missingValue);

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
        Assert.Equal(ModelValidationReason.Required, error.Reason);
        Assert.Equal("REQUIRED", error.Code);
        Assert.Equal(target, error.Target);
    }

    [Theory]
    [InlineData(" ", "\u3000")]
    [InlineData("\u200B", " ")]
    public void ImageGenerationRequest_Create_WhenBothPatternValuesLackVisibleCharacters_ReturnsVisibleCharacterRequiredError(
        string foregroundCharacter,
        string backgroundCharacter)
    {
        var values = CreateValidValues(foregroundCharacter, backgroundCharacter);

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
        Assert.Equal(
            ["foregroundCharacter", "backgroundCharacter"],
            error.Targets);
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

    private static RequestValuesBuilder CreateValidValues(
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

        return new RequestValuesBuilder(
            type,
            text,
            validatedForegroundCharacter,
            foregroundColor,
            validatedBackgroundCharacter,
            backgroundColor);
    }

    public enum MissingValue
    {
        Type,
        Text,
        ForegroundCharacter,
        ForegroundColor,
        BackgroundCharacter,
        BackgroundColor,
    }

    private sealed record RequestValuesBuilder(
        ImageType? Type,
        RenderText? Text,
        PatternCharacter? ForegroundCharacter,
        HexColor? ForegroundColor,
        PatternCharacter? BackgroundCharacter,
        HexColor? BackgroundColor)
    {
        public RequestValuesBuilder Without(MissingValue missingValue)
        {
            return missingValue switch
            {
                MissingValue.Type => this with { Type = null },
                MissingValue.Text => this with { Text = null },
                MissingValue.ForegroundCharacter => this with { ForegroundCharacter = null },
                MissingValue.ForegroundColor => this with { ForegroundColor = null },
                MissingValue.BackgroundCharacter => this with { BackgroundCharacter = null },
                MissingValue.BackgroundColor => this with { BackgroundColor = null },
                _ => throw new ArgumentOutOfRangeException(
                    nameof(missingValue),
                    missingValue,
                    null),
            };
        }
    }
}
