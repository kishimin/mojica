using System.Net;
using Mojica.Api.Infrastructure;
using Mojica.Api.Ports;

namespace Mojica.Api.Tests.Infrastructure;

public sealed class GlyphForgeResponseMapperSmallTests
{
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
        var response = new GlyphForgeResponse(null, null, null, Failure: GlyphForgeResponseFailure.Timeout);

        var result = GlyphForgeResponseMapper.Map(response);

        Assert.False(result.IsSuccess);
        Assert.Equal(ImageGenerationPortErrorCode.Timeout, result.Error?.ErrorCode);
        Assert.Null(result.Error?.Details);
    }

    [Fact]
    public void Map_WhenCommunicationFails_ReturnsUnavailableError()
    {
        var response = new GlyphForgeResponse(null, null, null, Failure: GlyphForgeResponseFailure.Communication);

        var result = GlyphForgeResponseMapper.Map(response);

        Assert.False(result.IsSuccess);
        Assert.Equal(ImageGenerationPortErrorCode.Unavailable, result.Error?.ErrorCode);
        Assert.Null(result.Error?.Details);
    }
}
