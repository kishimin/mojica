using Mojica.Api.Ports;

namespace Mojica.Api.Tests.Ports;

public sealed class ImageGenerationPortErrorSmallTests
{
    [Fact]
    public void ImageGenerationPortErrorCode_DocumentedCodes_ExposeExpectedValues()
    {
        Assert.Equal("RATE_LIMITED", ImageGenerationPortErrorCode.RateLimited.Value);
        Assert.Equal("TIMEOUT", ImageGenerationPortErrorCode.Timeout.Value);
        Assert.Equal("UNAVAILABLE", ImageGenerationPortErrorCode.Unavailable.Value);
        Assert.Equal("INVALID_RESPONSE", ImageGenerationPortErrorCode.InvalidResponse.Value);
        Assert.Equal("FAILED", ImageGenerationPortErrorCode.Failed.Value);
    }

    [Fact]
    public void ImageGenerationPortError_Create_WhenRetryPeriodIsKnown_PreservesRetryAfterSeconds()
    {
        var error = new ImageGenerationPortError(
            ImageGenerationPortErrorCode.RateLimited,
            retryAfter: 60);

        Assert.Same(ImageGenerationPortErrorCode.RateLimited, error.ErrorCode);
        Assert.Equal("RATE_LIMITED", error.Code);
        Assert.Equal(60, error.RetryAfter);
    }

    [Fact]
    public void ImageGenerationPortError_Create_WhenRetryPeriodIsUnavailable_LeavesRetryAfterUnset()
    {
        var error = new ImageGenerationPortError(
            ImageGenerationPortErrorCode.Timeout);

        Assert.Null(error.RetryAfter);
    }

    [Fact]
    public void ImageGenerationPortError_Create_DoesNotExposeCommunicationDetails()
    {
        var error = new ImageGenerationPortError(
            ImageGenerationPortErrorCode.Failed);

        Assert.Null(error.Details);
    }
}
