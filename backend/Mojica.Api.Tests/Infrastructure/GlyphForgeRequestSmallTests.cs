using Mojica.Api.Infrastructure;

namespace Mojica.Api.Tests.Infrastructure;

public sealed class GlyphForgeRequestSmallTests
{
    [Fact]
    public void Create_WhenFrameTextIsNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new GlyphForgeRequest(null!, "inner", "outer", [0, 0, 0], [0, 0, 0]));
    }

    [Fact]
    public void Create_WhenInnerTextIsNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new GlyphForgeRequest("frame", null!, "outer", [0, 0, 0], [0, 0, 0]));
    }

    [Fact]
    public void Create_WhenOuterTextIsNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new GlyphForgeRequest("frame", "inner", null!, [0, 0, 0], [0, 0, 0]));
    }

    [Fact]
    public void Create_WhenInnerColorIsNull_ThrowsArgumentNullExceptionForInnerColorParameter()
    {
        var exception = Assert.Throws<ArgumentNullException>(() =>
            new GlyphForgeRequest("frame", "inner", "outer", null!, [0, 0, 0]));

        Assert.Equal("innerColor", exception.ParamName);
    }

    [Fact]
    public void Create_WhenOuterColorIsNull_ThrowsArgumentNullExceptionForOuterColorParameter()
    {
        var exception = Assert.Throws<ArgumentNullException>(() =>
            new GlyphForgeRequest("frame", "inner", "outer", [0, 0, 0], null!));

        Assert.Equal("outerColor", exception.ParamName);
    }

    [Fact]
    public void Equality_WhenFrameTextDiffers_IsNotEqualAndHashCodeDiffers()
    {
        var first = new GlyphForgeRequest("KA", "inner", "outer", [0, 0, 0], [0, 0, 0]);
        var second = new GlyphForgeRequest("XY", "inner", "outer", [0, 0, 0], [0, 0, 0]);

        Assert.NotEqual(first, second);
        Assert.NotEqual(first.GetHashCode(), second.GetHashCode());
    }

    [Fact]
    public void Equality_WhenInnerTextDiffers_IsNotEqualAndHashCodeDiffers()
    {
        var first = new GlyphForgeRequest("frame", "🌻", "outer", [0, 0, 0], [0, 0, 0]);
        var second = new GlyphForgeRequest("frame", "☀", "outer", [0, 0, 0], [0, 0, 0]);

        Assert.NotEqual(first, second);
        Assert.NotEqual(first.GetHashCode(), second.GetHashCode());
    }

    [Fact]
    public void Equality_WhenOuterTextDiffers_IsNotEqualAndHashCodeDiffers()
    {
        var first = new GlyphForgeRequest("frame", "inner", "🌻", [0, 0, 0], [0, 0, 0]);
        var second = new GlyphForgeRequest("frame", "inner", "☀", [0, 0, 0], [0, 0, 0]);

        Assert.NotEqual(first, second);
        Assert.NotEqual(first.GetHashCode(), second.GetHashCode());
    }

    [Fact]
    public void Equality_WhenInnerColorDiffers_IsNotEqual()
    {
        var first = new GlyphForgeRequest("frame", "inner", "outer", [255, 0, 0], [0, 0, 0]);
        var second = new GlyphForgeRequest("frame", "inner", "outer", [0, 255, 0], [0, 0, 0]);

        Assert.NotEqual(first, second);
    }

    [Fact]
    public void Equality_WhenOuterColorDiffers_IsNotEqual()
    {
        var first = new GlyphForgeRequest("frame", "inner", "outer", [0, 0, 0], [255, 0, 0]);
        var second = new GlyphForgeRequest("frame", "inner", "outer", [0, 0, 0], [0, 255, 0]);

        Assert.NotEqual(first, second);
    }
}
