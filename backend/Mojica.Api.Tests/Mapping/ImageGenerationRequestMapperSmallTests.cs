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
    public void Map_WhenDtoIsNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => ImageGenerationRequestMapper.Map(null!));
    }

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
        Assert.Equal("type", result.Errors[0].Target);
        Assert.Equal("text", result.Errors[1].Target);
        Assert.Equal("foregroundCharacter", result.Errors[2].Target);
        Assert.Equal("foregroundColor", result.Errors[3].Target);
        Assert.Equal("backgroundCharacter", result.Errors[4].Target);
        Assert.Equal("backgroundColor", result.Errors[5].Target);
    }

    [Fact]
    public void Map_WhenTypeAndBothPatternValuesAreInvalid_ReturnsAllDetectedFieldErrors()
    {
        var dto = ValidDto() with
        {
            Type = "animated",
            ForegroundCharacter = " ",
            BackgroundCharacter = "\u200B",
        };

        var result = ImageGenerationRequestMapper.Map(dto);

        Assert.False(result.IsSuccess);
        Assert.Null(result.Request);
        Assert.Equal(3, result.Errors.Count);
        Assert.Equal("type", result.Errors[0].Target);
        Assert.Equal(ModelValidationReason.UnsupportedImageType, result.Errors[0].Reason);
        Assert.Equal("foregroundCharacter", result.Errors[1].Target);
        Assert.Equal(ModelValidationReason.VisibleCharacterRequired, result.Errors[1].Reason);
        Assert.Equal("backgroundCharacter", result.Errors[2].Target);
        Assert.Equal(ModelValidationReason.VisibleCharacterRequired, result.Errors[2].Reason);
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
        Assert.Equal("foregroundCharacter", result.Errors[0].Target);
        Assert.Equal("backgroundCharacter", result.Errors[1].Target);
    }

    private static ImageGenerationRequestDto ValidDto() => new(
        "standard",
        "Mojica",
        "@",
        "#FFFFFF",
        ".",
        "#000000");
}
