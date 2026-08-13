using Mojica.Api.Models;

namespace Mojica.Api.Tests.Models;

public sealed class HexColorSmallTests
{
    [Theory]
    [InlineData("#FF69B4")]
    [InlineData("#ff69b4")]
    [InlineData("#Ff69b4")]
    public void HexColor_Create_WhenInputUsesValidRrgGBbFormat_NormalizesToUppercase(
        string value)
    {
        var succeeded = HexColor.TryCreate(
            value,
            out var color,
            out var reason);

        Assert.True(succeeded);
        Assert.NotNull(color);
        Assert.Equal("#FF69B4", color.Value);
        Assert.Equal("#FF69B4", color.ToString());
        Assert.Null(reason);
    }

    [Fact]
    public void HexColor_Create_WhenInputIsMissing_ReturnsRequiredError()
    {
        var succeeded = HexColor.TryCreate(
            null,
            out var color,
            out var reason);

        Assert.False(succeeded);
        Assert.Null(color);
        Assert.Equal(ModelValidationReason.Required, reason);
    }

    [Theory]
    [InlineData("FF69B4")]
    [InlineData("#FFF")]
    [InlineData("#FF69B40")]
    [InlineData("#GG69B4")]
    [InlineData(" #FF69B4")]
    [InlineData("#FF69B4 ")]
    public void HexColor_Create_WhenFormatIsInvalid_ReturnsInvalidHexColorError(
        string value)
    {
        var succeeded = HexColor.TryCreate(
            value,
            out var color,
            out var reason);

        Assert.False(succeeded);
        Assert.Null(color);
        Assert.Equal(ModelValidationReason.InvalidHexColor, reason);
    }

    [Fact]
    public void HexColor_ToRgb_WhenValueIsFf69b4_ReturnsExpectedComponents()
    {
        Assert.True(HexColor.TryCreate("#FF69B4", out var color, out var reason));

        var rgbColor = color.ToRgb();

        Assert.Equal(255, rgbColor.Red);
        Assert.Equal(105, rgbColor.Green);
        Assert.Equal(180, rgbColor.Blue);
        Assert.Null(reason);
    }

    [Theory]
    [InlineData("#000000", 0)]
    [InlineData("#FFFFFF", 255)]
    public void HexColor_ToRgb_WhenComponentsAreAtBoundaries_ReturnsExpectedComponents(
        string value,
        int expectedComponent)
    {
        Assert.True(HexColor.TryCreate(value, out var color, out var reason));

        var rgbColor = color.ToRgb();

        Assert.Equal(expectedComponent, rgbColor.Red);
        Assert.Equal(expectedComponent, rgbColor.Green);
        Assert.Equal(expectedComponent, rgbColor.Blue);
        Assert.Null(reason);
    }
}
