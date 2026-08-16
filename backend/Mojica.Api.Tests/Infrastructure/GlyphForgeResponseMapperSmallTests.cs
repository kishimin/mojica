using System.Net;
using Mojica.Api.Infrastructure;
using Mojica.Api.Ports;

namespace Mojica.Api.Tests.Infrastructure;

public sealed class GlyphForgeResponseMapperSmallTests
{
    [Fact]
    public void Map_WhenResponseIsNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => GlyphForgeResponseMapper.Map(null!));
    }

    [Fact]
    public void Map_WhenSuccessfulPngResponse_ReturnsGeneratedImageData()
    {
        var content = new byte[] { 0x89, 0x50, 0x4E, 0x47 };
        var response = new GlyphForgeResponse(HttpStatusCode.OK, "image/png", content);

        var result = GlyphForgeResponseMapper.Map(response);

        Assert.True(result.IsSuccess);
        Assert.Equal(new GeneratedImageData(content, "image/png"), result.Data);
    }

    [Fact]
    public void Map_WhenSuccessfulResponseHasNonPngContentType_ReturnsInvalidResponse()
    {
        var response = new GlyphForgeResponse(HttpStatusCode.OK, "text/plain", [1, 2, 3]);

        var result = GlyphForgeResponseMapper.Map(response);

        Assert.False(result.IsSuccess);
        Assert.Equal(ImageGenerationPortErrorCode.InvalidResponse, result.Error?.ErrorCode);
    }

    [Fact]
    public void Map_WhenSuccessfulResponseHasEmptyBody_ReturnsInvalidResponse()
    {
        var response = new GlyphForgeResponse(HttpStatusCode.OK, "image/png", []);

        var result = GlyphForgeResponseMapper.Map(response);

        Assert.False(result.IsSuccess);
        Assert.Equal(ImageGenerationPortErrorCode.InvalidResponse, result.Error?.ErrorCode);
    }

    [Fact]
    public void Map_WhenSuccessfulResponseIsCreated_ReturnsInvalidResponse()
    {
        var response = new GlyphForgeResponse(HttpStatusCode.Created, "image/png", [1, 2, 3]);

        var result = GlyphForgeResponseMapper.Map(response);

        Assert.Equal(ImageGenerationPortErrorCode.InvalidResponse, result.Error?.ErrorCode);
    }

    [Fact]
    public void Map_WhenResponseIsRateLimited_ReturnsRateLimitedError()
    {
        var response = new GlyphForgeResponse(HttpStatusCode.TooManyRequests, null, null, 7);

        var result = GlyphForgeResponseMapper.Map(response);

        Assert.False(result.IsSuccess);
        Assert.Equal(ImageGenerationPortErrorCode.RateLimited, result.Error?.ErrorCode);
        Assert.Equal(7, result.Error?.RetryAfter);
        Assert.Null(result.Error?.Details);
    }

    [Fact]
    public void Map_WhenOutputSizeIsRejected_ReturnsOutputSizeExceededWithoutExternalDetails()
    {
        var response = new GlyphForgeResponse(
            HttpStatusCode.UnprocessableEntity,
            "application/json",
            System.Text.Encoding.UTF8.GetBytes("internal stack trace"));

        var result = GlyphForgeResponseMapper.Map(response);

        Assert.False(result.IsSuccess);
        Assert.Equal(ImageGenerationPortErrorCode.OutputSizeExceeded, result.Error?.ErrorCode);
        Assert.Null(result.Error?.Details);
    }

    [Fact]
    public void Map_WhenResponseIndicatesUnavailable_ReturnsUnavailableError()
    {
        var response = new GlyphForgeResponse(HttpStatusCode.ServiceUnavailable, null, null);

        var result = GlyphForgeResponseMapper.Map(response);

        Assert.False(result.IsSuccess);
        Assert.Equal(ImageGenerationPortErrorCode.Unavailable, result.Error?.ErrorCode);
        Assert.Null(result.Error?.Details);
    }

    [Fact]
    public void Map_WhenResponseIsServerFailure_ReturnsFailedError()
    {
        var response = new GlyphForgeResponse(HttpStatusCode.BadGateway, null, null);

        var result = GlyphForgeResponseMapper.Map(response);

        Assert.False(result.IsSuccess);
        Assert.Equal(ImageGenerationPortErrorCode.Failed, result.Error?.ErrorCode);
        Assert.Null(result.Error?.Details);
    }

    [Fact]
    public void Map_WhenTimeoutOccurs_ReturnsTimeoutError()
    {
        var response = new GlyphForgeResponse(null, null, null, failure: GlyphForgeResponseFailure.Timeout);

        var result = GlyphForgeResponseMapper.Map(response);

        Assert.False(result.IsSuccess);
        Assert.Equal(ImageGenerationPortErrorCode.Timeout, result.Error?.ErrorCode);
        Assert.Null(result.Error?.Details);
    }

    [Fact]
    public void Map_WhenCommunicationFails_ReturnsUnavailableError()
    {
        var response = new GlyphForgeResponse(null, null, null, failure: GlyphForgeResponseFailure.Communication);

        var result = GlyphForgeResponseMapper.Map(response);

        Assert.False(result.IsSuccess);
        Assert.Equal(ImageGenerationPortErrorCode.Unavailable, result.Error?.ErrorCode);
        Assert.Null(result.Error?.Details);
    }

    [Fact]
    public void Map_WhenServerStatusIs511_ReturnsFailedError()
    {
        var response = new GlyphForgeResponse((HttpStatusCode)511, null, null);

        var result = GlyphForgeResponseMapper.Map(response);

        Assert.Equal(ImageGenerationPortErrorCode.Failed, result.Error?.ErrorCode);
    }

    [Fact]
    public void Response_WhenBinaryContentsMatch_UsesContentEquality()
    {
        var first = new GlyphForgeResponse(HttpStatusCode.OK, "image/png", [1, 2, 3]);
        var second = new GlyphForgeResponse(HttpStatusCode.OK, "image/png", [1, 2, 3]);

        Assert.Equal(first, second);
        Assert.Equal(first.GetHashCode(), second.GetHashCode());
    }

    [Fact]
    public void Response_WhenStatusCodeDiffers_IsNotEqualAndHashCodeDiffers()
    {
        var first = new GlyphForgeResponse(HttpStatusCode.OK, "image/png", [1, 2, 3]);
        var second = new GlyphForgeResponse(HttpStatusCode.BadGateway, "image/png", [1, 2, 3]);

        Assert.NotEqual(first, second);
        Assert.NotEqual(first.GetHashCode(), second.GetHashCode());
    }

    [Fact]
    public void Response_WhenMediaTypeDiffers_IsNotEqualAndHashCodeDiffers()
    {
        var first = new GlyphForgeResponse(HttpStatusCode.OK, "image/png", [1, 2, 3]);
        var second = new GlyphForgeResponse(HttpStatusCode.OK, "text/plain", [1, 2, 3]);

        Assert.NotEqual(first, second);
        Assert.NotEqual(first.GetHashCode(), second.GetHashCode());
    }

    [Fact]
    public void Response_WhenRetryAfterDiffers_IsNotEqualAndHashCodeDiffers()
    {
        var first = new GlyphForgeResponse(HttpStatusCode.TooManyRequests, null, null, retryAfter: 5);
        var second = new GlyphForgeResponse(HttpStatusCode.TooManyRequests, null, null, retryAfter: 10);

        Assert.NotEqual(first, second);
        Assert.NotEqual(first.GetHashCode(), second.GetHashCode());
    }

    [Fact]
    public void Response_WhenBothAreTimeoutFailuresWithNoContent_AreEqual()
    {
        var first = new GlyphForgeResponse(null, null, null, failure: GlyphForgeResponseFailure.Timeout);
        var second = new GlyphForgeResponse(null, null, null, failure: GlyphForgeResponseFailure.Timeout);

        Assert.Equal(first, second);
        Assert.Equal(first.GetHashCode(), second.GetHashCode());
    }

    [Fact]
    public void Response_WhenFailureDiffers_IsNotEqualAndHashCodeDiffers()
    {
        var first = new GlyphForgeResponse(null, null, null, failure: GlyphForgeResponseFailure.Timeout);
        var second = new GlyphForgeResponse(null, null, null, failure: GlyphForgeResponseFailure.Communication);

        Assert.NotEqual(first, second);
        Assert.NotEqual(first.GetHashCode(), second.GetHashCode());
    }

    [Fact]
    public void Response_WhenContentDiffers_IsNotEqual()
    {
        var first = new GlyphForgeResponse(HttpStatusCode.OK, "image/png", [1, 2, 3]);
        var second = new GlyphForgeResponse(HttpStatusCode.OK, "image/png", [4, 5, 6]);

        Assert.NotEqual(first, second);
    }

    [Fact]
    public void Response_WhenStatusAndFailureAreBothSpecified_RejectsAmbiguousState()
    {
        var exception = Assert.Throws<ArgumentException>(() => new GlyphForgeResponse(
            HttpStatusCode.BadGateway,
            null,
            null,
            failure: GlyphForgeResponseFailure.Failed));

        Assert.StartsWith(
            "A response cannot contain both an HTTP status and a transport failure.",
            exception.Message);
    }
}
