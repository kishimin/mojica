using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace Mojica.Api.Tests.Controllers;

public sealed class ImageUploadControllerMediumTests
{
    [Fact]
    public async Task PostFrameFile_WhenUnknownLengthBodyExceedsLimit_ReturnsLocalizedPayloadTooLarge()
    {
        using var factory = new ImageControllerFactory();
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.AcceptLanguage.ParseAdd("ja");
        using var content = new UnknownLengthContent(new byte[2 * 1024 * 1024 + 64 * 1024 + 1]);
        content.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");
        content.Headers.ContentType.Parameters.Add(new NameValueHeaderValue("boundary", "example"));

        using var response = await client.PostAsync("/images/frame-file", content);

        Assert.Equal(HttpStatusCode.RequestEntityTooLarge, response.StatusCode);
        using var document = System.Text.Json.JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.Equal("REQUEST_TOO_LARGE", document.RootElement.GetProperty("code").GetString());
    }
    [Fact]
    public async Task PostFrameFile_WhenUpstreamTimesOut_ReturnsGatewayTimeout()
    {
        using var handler = new TimeoutImageHandler();
        using var factory = new ImageControllerFactory().WithWebHostBuilder(builder =>
            builder.ConfigureTestServices(services =>
                services.AddHttpClient("GlyphForge").ConfigurePrimaryHttpMessageHandler(() => handler)));
        using var client = factory.CreateClient();
        using var request = new MultipartFormDataContent();
        request.Add(new ByteArrayContent([1]), "frameImage", "frame.png");
        request.Add(new StringContent("@"), "foregroundCharacter");
        request.Add(new StringContent("#87CEEB"), "foregroundColor");
        request.Add(new StringContent("."), "backgroundCharacter");
        request.Add(new StringContent("#FFFFFF"), "backgroundColor");

        using var response = await client.PostAsync("/images/frame-file", request);

        Assert.Equal(HttpStatusCode.GatewayTimeout, response.StatusCode);
    }
    [Theory]
    [InlineData("ja", "生成される画像がサイズ上限を超えます。元画像を小さくして再度お試しください。")]
    [InlineData("en", "The generated image exceeds the size limit. Reduce the source image dimensions and try again.")]
    public async Task PostFrameFile_WhenOutputIsTooLarge_ReturnsLocalizedGuidance(string language, string message)
    {
        using var handler = new ErrorImageHandler("OUTPUT_SIZE_EXCEEDED");
        using var factory = new ImageControllerFactory().WithWebHostBuilder(builder =>
            builder.ConfigureTestServices(services =>
                services.AddHttpClient("GlyphForge").ConfigurePrimaryHttpMessageHandler(() => handler)));
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.AcceptLanguage.ParseAdd(language);
        using var request = new MultipartFormDataContent();
        request.Add(new ByteArrayContent([1]), "frameImage", "frame.png");
        request.Add(new StringContent("@"), "foregroundCharacter");
        request.Add(new StringContent("#87CEEB"), "foregroundColor");
        request.Add(new StringContent("."), "backgroundCharacter");
        request.Add(new StringContent("#FFFFFF"), "backgroundColor");

        using var response = await client.PostAsync("/images/frame-file", request);

        using var document = System.Text.Json.JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.Equal(message, document.RootElement.GetProperty("message").GetString());
    }
    [Fact]
    public async Task PostFrameFile_WhenFileIsMissing_ReturnsLocalizedBadRequest()
    {
        using var factory = new ImageControllerFactory();
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.AcceptLanguage.ParseAdd("ja");
        using var request = new MultipartFormDataContent();
        request.Add(new StringContent("@"), "foregroundCharacter");
        request.Add(new StringContent("#87CEEB"), "foregroundColor");
        request.Add(new StringContent("."), "backgroundCharacter");
        request.Add(new StringContent("#FFFFFF"), "backgroundColor");

        using var response = await client.PostAsync("/images/frame-file", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        using var document = System.Text.Json.JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.Equal("BAD_REQUEST", document.RootElement.GetProperty("code").GetString());
        Assert.Equal("リクエストの形式が正しくありません。", document.RootElement.GetProperty("message").GetString());
    }

    [Fact]
    public async Task PostFrameFile_WhenBackgroundIsWhitespace_ForwardsIt()
    {
        using var handler = new SuccessfulImageHandler();
        using var factory = new ImageControllerFactory().WithWebHostBuilder(builder =>
            builder.ConfigureTestServices(services =>
                services.AddHttpClient("GlyphForge").ConfigurePrimaryHttpMessageHandler(() => handler)));
        using var client = factory.CreateClient();
        using var request = new MultipartFormDataContent();
        request.Add(new ByteArrayContent([1]), "frameImage", "frame.png");
        request.Add(new StringContent("@"), "foregroundCharacter");
        request.Add(new StringContent("#87CEEB"), "foregroundColor");
        request.Add(new StringContent(" "), "backgroundCharacter");
        request.Add(new StringContent("#FFFFFF"), "backgroundColor");

        using var response = await client.PostAsync("/images/frame-file", request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task PostFrameFile_WhenMultipartBodyExceedsTransportLimit_ReturnsPayloadTooLarge()
    {
        using var handler = new SuccessfulImageHandler();
        using var factory = new ImageControllerFactory().WithWebHostBuilder(builder =>
            builder.ConfigureTestServices(services =>
                services.AddHttpClient("GlyphForge").ConfigurePrimaryHttpMessageHandler(() => handler)));
        using var client = factory.CreateClient();
        using var request = new MultipartFormDataContent();
        request.Add(new ByteArrayContent(new byte[2 * 1024 * 1024 + 64 * 1024 + 1]), "frameImage", "frame.png");
        request.Add(new StringContent("@"), "foregroundCharacter");
        request.Add(new StringContent("#87CEEB"), "foregroundColor");
        request.Add(new StringContent("."), "backgroundCharacter");
        request.Add(new StringContent("#FFFFFF"), "backgroundColor");

        using var response = await client.PostAsync("/images/frame-file", request);

        Assert.Equal(HttpStatusCode.RequestEntityTooLarge, response.StatusCode);
        Assert.Null(handler.RequestPath);
    }
    [Theory]
    [InlineData(HttpStatusCode.TooManyRequests, HttpStatusCode.TooManyRequests)]
    [InlineData(HttpStatusCode.ServiceUnavailable, HttpStatusCode.BadGateway)]
    public async Task PostFrameFile_WhenUpstreamIsBusy_PreservesRetryAfter(
        HttpStatusCode upstreamStatus, HttpStatusCode expectedStatus)
    {
        using var handler = new BusyImageHandler(upstreamStatus);
        using var factory = new ImageControllerFactory().WithWebHostBuilder(builder =>
            builder.ConfigureTestServices(services =>
                services.AddHttpClient("GlyphForge").ConfigurePrimaryHttpMessageHandler(() => handler)));
        using var client = factory.CreateClient();
        using var request = new MultipartFormDataContent();
        request.Add(new ByteArrayContent([1]), "frameImage", "frame.png");
        request.Add(new StringContent("@"), "foregroundCharacter");
        request.Add(new StringContent("#87CEEB"), "foregroundColor");
        request.Add(new StringContent("."), "backgroundCharacter");
        request.Add(new StringContent("#FFFFFF"), "backgroundColor");

        using var response = await client.PostAsync("/images/frame-file", request);

        Assert.Equal(expectedStatus, response.StatusCode);
        Assert.Equal(TimeSpan.FromSeconds(12), response.Headers.RetryAfter?.Delta);
    }

    [Fact]
    public async Task PostFrameFile_WhenUpstreamRejectsRequestSize_ReturnsPayloadTooLarge()
    {
        using var handler = new BusyImageHandler(HttpStatusCode.RequestEntityTooLarge);
        using var factory = new ImageControllerFactory().WithWebHostBuilder(builder =>
            builder.ConfigureTestServices(services =>
                services.AddHttpClient("GlyphForge").ConfigurePrimaryHttpMessageHandler(() => handler)));
        using var client = factory.CreateClient();
        using var request = new MultipartFormDataContent();
        request.Add(new ByteArrayContent([1]), "frameImage", "frame.png");
        request.Add(new StringContent("@"), "foregroundCharacter");
        request.Add(new StringContent("#87CEEB"), "foregroundColor");
        request.Add(new StringContent("."), "backgroundCharacter");
        request.Add(new StringContent("#FFFFFF"), "backgroundColor");

        using var response = await client.PostAsync("/images/frame-file", request);

        Assert.Equal(HttpStatusCode.RequestEntityTooLarge, response.StatusCode);
        using var document = System.Text.Json.JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.Equal("REQUEST_TOO_LARGE", document.RootElement.GetProperty("code").GetString());
    }

    [Theory]
    [InlineData("INVALID_IMAGE", "INVALID_IMAGE")]
    [InlineData("OUTPUT_SIZE_EXCEEDED", "IMAGE_OUTPUT_SIZE_EXCEEDED")]
    [InlineData("UNKNOWN", "IMAGE_REQUEST_INVALID")]
    [InlineData(null, "IMAGE_REQUEST_INVALID")]
    public async Task PostFrameFile_WhenUpstreamRejectsImage_ReturnsClassifiedError(
        string? upstreamCode, string expectedCode)
    {
        using var handler = new ErrorImageHandler(upstreamCode);
        using var factory = new ImageControllerFactory().WithWebHostBuilder(builder =>
            builder.ConfigureTestServices(services =>
                services.AddHttpClient("GlyphForge").ConfigurePrimaryHttpMessageHandler(() => handler)));
        using var client = factory.CreateClient();
        using var request = new MultipartFormDataContent();
        request.Add(new ByteArrayContent([1]), "frameImage", "frame.png");
        request.Add(new StringContent("@"), "foregroundCharacter");
        request.Add(new StringContent("#87CEEB"), "foregroundColor");
        request.Add(new StringContent("."), "backgroundCharacter");
        request.Add(new StringContent("#FFFFFF"), "backgroundColor");

        using var response = await client.PostAsync("/images/frame-file", request);

        Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        var body = await response.Content.ReadAsStringAsync();
        using var document = System.Text.Json.JsonDocument.Parse(body);
        Assert.Equal(expectedCode, document.RootElement.GetProperty("code").GetString());
        Assert.DoesNotContain("upstream secret", body);
    }

    [Fact]
    public async Task PostFrameFile_WhenUpstreamReturnsPng_ReturnsPng()
    {
        using var handler = new SuccessfulImageHandler();
        using var factory = new ImageControllerFactory().WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
                services.AddHttpClient("GlyphForge").ConfigurePrimaryHttpMessageHandler(() => handler));
        });
        using var client = factory.CreateClient();
        using var request = new MultipartFormDataContent();
        request.Add(new ByteArrayContent([0x89, 0x50, 0x4e, 0x47]), "frameImage", "frame.png");
        request.Add(new StringContent("@"), "foregroundCharacter");
        request.Add(new StringContent("#87CEEB"), "foregroundColor");
        request.Add(new StringContent("."), "backgroundCharacter");
        request.Add(new StringContent("#FFFFFF"), "backgroundColor");

        using var response = await client.PostAsync("/images/frame-file", request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("image/png", response.Content.Headers.ContentType?.MediaType);
        Assert.Equal(new byte[] { 0x89, 0x50, 0x4e, 0x47 }, await response.Content.ReadAsByteArrayAsync());
        Assert.Equal("/images/frame-file", handler.RequestPath);
        Assert.Contains("name=frame_image", handler.RequestBody);
        Assert.Contains("name=inner_text", handler.RequestBody);
        Assert.Contains("135,206,235", handler.RequestBody);
    }

    private sealed class SuccessfulImageHandler : HttpMessageHandler
    {
        public string? RequestPath { get; private set; }
        public string? RequestBody { get; private set; }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            RequestPath = request.RequestUri?.AbsolutePath;
            RequestBody = await request.Content!.ReadAsStringAsync(cancellationToken);
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent([0x89, 0x50, 0x4e, 0x47])
                {
                    Headers = { ContentType = new MediaTypeHeaderValue("image/png") },
                },
            };
        }
    }

    private sealed class ErrorImageHandler(string? code) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(HttpStatusCode.UnprocessableEntity)
            {
                Content = new StringContent("upstream secret"),
            };
            if (code is not null)
            {
                response.Headers.Add("X-Glyph-Forge-Error", code);
            }
            return Task.FromResult(response);
        }
    }

    private sealed class BusyImageHandler(HttpStatusCode status) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(status) { Content = new StringContent("busy") };
            response.Headers.RetryAfter = new RetryConditionHeaderValue(TimeSpan.FromSeconds(12));
            return Task.FromResult(response);
        }
    }

    private sealed class TimeoutImageHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken) =>
            Task.FromException<HttpResponseMessage>(new TaskCanceledException());
    }

    private sealed class UnknownLengthContent(byte[] bytes) : HttpContent
    {
        protected override Task SerializeToStreamAsync(Stream stream, TransportContext? context) =>
            stream.WriteAsync(bytes).AsTask();

        protected override bool TryComputeLength(out long length)
        {
            length = 0;
            return false;
        }
    }
}
