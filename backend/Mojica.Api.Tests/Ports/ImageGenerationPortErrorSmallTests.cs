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

    [Fact(Skip = "TODO: Implement after the safe Port error details contract exists.")]
    public void ImageGenerationPortError_Create_WhenFailureContainsCommunicationDetails_DoesNotExposeSensitiveDetails()
    {
        // ID: PORT-ERROR-04
        // Source: docs/v1/api/ports.md §3-4.
        // Given: a failure context containing credentials, an internal URL, a stack trace, and transport details
        // When: the failure is represented as an ImageGenerationPortError
        // Then: none of those communication details are present in the error's public attributes
        // Error: expose only supplementary information that is safe for an external caller
        // Blocked by: feature/add-image-generation-port must define how safe public details are constructed
        // Priority: High
    }
}
