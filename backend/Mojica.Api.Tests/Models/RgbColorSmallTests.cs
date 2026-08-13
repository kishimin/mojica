using Mojica.Api.Models;

namespace Mojica.Api.Tests.Models;

public sealed class RgbColorSmallTests
{
    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(255, 255, 255)]
    [InlineData(0, 128, 255)]
    public void RgbColor_Create_WhenEveryComponentIsWithinRange_Succeeds(
        int red,
        int green,
        int blue)
    {
        var succeeded = RgbColor.TryCreate(
            red,
            green,
            blue,
            out var color,
            out var error);

        Assert.True(succeeded);
        Assert.NotNull(color);
        Assert.Equal(red, color.Red);
        Assert.Equal(green, color.Green);
        Assert.Equal(blue, color.Blue);
        Assert.Null(error);
    }

    [Theory]
    [InlineData(-1, 0, 0, "red")]
    [InlineData(0, -1, 0, "green")]
    [InlineData(0, 0, -1, "blue")]
    public void RgbColor_Create_WhenAnyComponentIsBelowZero_ReturnsRangeError(
        int red,
        int green,
        int blue,
        string expectedTarget)
    {
        var succeeded = RgbColor.TryCreate(
            red,
            green,
            blue,
            out var color,
            out var error);

        Assert.False(succeeded);
        Assert.Null(color);
        Assert.NotNull(error);
        Assert.Equal("VALUE_OUT_OF_RANGE", error.Code);
        Assert.Equal(expectedTarget, error.Target);
        Assert.Equal(ModelValidationReason.ValueOutOfRange, error.Reason);
        Assert.Equal("0", error.Details["minimum"]);
        Assert.Equal("255", error.Details["maximum"]);
        Assert.Equal("-1", error.Details["actual"]);
    }

    [Fact(Skip = "TODO: Blocked until the RGB range error code is defined.")]
    public void RgbColor_Create_WhenAnyComponentExceedsTwoHundredFiftyFive_ReturnsRangeError()
    {
        // ID: RGB-03
        // Source: docs/v1/api/models.md §8 RgbColor.
        // Given: red, green, or blue is 256 while the other components are valid (Theory candidate)
        // When: RgbColor creation is requested
        // Then: creation fails with the documented range error and identifies the invalid component
        // Blocked by: models.md does not define the language-independent RGB range error code
        // Priority: High
    }
}
