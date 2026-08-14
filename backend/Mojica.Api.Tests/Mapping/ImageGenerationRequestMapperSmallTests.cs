using Mojica.Api.Contracts;
using Mojica.Api.Mapping;
using Mojica.Api.Models;

namespace Mojica.Api.Tests.Mapping;

public sealed class ImageGenerationRequestMapperSmallTests
{
    public static TheoryData<ImageGenerationRequestDto, string, ModelValidationReason>
        InvalidFieldCases => new()
        {
            { ValidDto() with { Type = "animated" }, "type", ModelValidationReason.UnsupportedImageType },
            { ValidDto() with { Text = " " }, "text", ModelValidationReason.NotBlank },
            { ValidDto() with { ForegroundCharacter = "\n" }, "foregroundCharacter", ModelValidationReason.ControlCharacter },
            { ValidDto() with { ForegroundColor = "FFFFFF" }, "foregroundColor", ModelValidationReason.InvalidHexColor },
            { ValidDto() with { BackgroundCharacter = new string('x', 129) }, "backgroundCharacter", ModelValidationReason.LengthOutOfRange },
            { ValidDto() with { BackgroundColor = "#GGGGGG" }, "backgroundColor", ModelValidationReason.InvalidHexColor },
        };

    [Fact]
    public void Map_WhenAllValuesAreValid_ReturnsImageGenerationRequest()
    {
        var dto = new ImageGenerationRequestDto(
            "x-icon",
            "KA",
            "🌻",
            "#FFD400",
            "☀",
            "#FF69B4");

        var result = ImageGenerationRequestMapper.Map(dto);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Request);
        Assert.Empty(result.Errors);
        Assert.Equal("x-icon", result.Request.Type.Value);
        Assert.Equal("KA", result.Request.Text.Value);
        Assert.Equal("🌻", result.Request.ForegroundCharacter.Value);
        Assert.Equal("#FFD400", result.Request.ForegroundColor.Value);
        Assert.Equal("☀", result.Request.BackgroundCharacter.Value);
        Assert.Equal("#FF69B4", result.Request.BackgroundColor.Value);
    }

    [Theory]
    [MemberData(nameof(InvalidFieldCases))]
    public void Map_WhenOneValueObjectCannotBeCreated_ReturnsReasonForItsRequestField(
        ImageGenerationRequestDto dto,
        string target,
        ModelValidationReason reason)
    {
        var result = ImageGenerationRequestMapper.Map(dto);

        Assert.False(result.IsSuccess);
        Assert.Null(result.Request);
        var error = Assert.Single(result.Errors);
        Assert.Equal(target, error.Target);
        Assert.Equal(reason, error.Reason);
    }

    [Fact]
    public void Map_WhenMultipleValuesAreInvalid_ReturnsAllIndependentFieldErrors()
    {
        var dto = new ImageGenerationRequestDto(
            "animated",
            " ",
            "\n",
            "FFFFFF",
            new string('x', 129),
            "#GGGGGG");

        var result = ImageGenerationRequestMapper.Map(dto);

        Assert.False(result.IsSuccess);
        Assert.Null(result.Request);
        Assert.Equal(6, result.Errors.Count);
        Assert.Equal(
            [
                "backgroundCharacter",
                "backgroundColor",
                "foregroundCharacter",
                "foregroundColor",
                "text",
                "type",
            ],
            result.Errors.Select(error => error.Target).Order());
    }

    [Fact]
    public void Map_WhenBothPatternValuesContainNoVisibleCharacter_ReturnsErrorsForBothFields()
    {
        var dto = ValidDto() with
        {
            ForegroundCharacter = " ",
            BackgroundCharacter = "\u200B",
        };

        var result = ImageGenerationRequestMapper.Map(dto);

        Assert.False(result.IsSuccess);
        Assert.Null(result.Request);
        Assert.Equal(2, result.Errors.Count);
        Assert.All(
            result.Errors,
            error => Assert.Equal(
                ModelValidationReason.VisibleCharacterRequired,
                error.Reason));
        Assert.Equal(
            ["backgroundCharacter", "foregroundCharacter"],
            result.Errors.Select(error => error.Target).Order());
    }

    private static ImageGenerationRequestDto ValidDto() => new(
        "standard",
        "Mojica",
        "@",
        "#FFFFFF",
        ".",
        "#000000");
}
