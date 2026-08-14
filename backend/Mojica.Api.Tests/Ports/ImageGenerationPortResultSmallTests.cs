using Mojica.Api.Ports;

namespace Mojica.Api.Tests.Ports;

public sealed class ImageGenerationPortResultSmallTests
{
    [Fact]
    public void ImageGenerationPortResult_Success_WhenDataIsNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            ImageGenerationPortResult.Success(null!));
    }

    [Fact]
    public void ImageGenerationPortResult_Failure_WhenErrorIsNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            ImageGenerationPortResult.Failure(null!));
    }
}
