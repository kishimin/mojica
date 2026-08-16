using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mojica.Api.Models;
using Mojica.Api.Ports;
using Mojica.Api.Services;

namespace Mojica.Api.Tests.Endpoints;

public sealed class ImageGenerationEndpointMediumTests : IClassFixture<ImageGenerationEndpointFactory>
{
    private readonly HttpClient client;

    public ImageGenerationEndpointMediumTests(ImageGenerationEndpointFactory factory)
    {
        client = factory.CreateClient();
    }

    public static TheoryData<string, string?> InvalidFieldCases => new()
    {
        { "type", "unsupported-type" },
        { "text", "" },
        { "foregroundColor", "not-a-hex-color" },
    };

    [Theory]
    [MemberData(nameof(InvalidFieldCases))]
    public async Task PostImages_WhenRequestValueIsInvalid_ReturnsUnprocessableEntityWithoutCallingService(
        string invalidField,
        string? invalidValue)
    {
        // ID: REQUEST-ENDPOINT-01
        // Source: docs/v1/api/controllers.md §4-5 and §10; docs/v1/api/api.md §6.
        var body = ValidRequestBody();
        body[invalidField] = invalidValue;
        var port = new RecordingImageGenerationPort(SuccessfulPortResult());
        using var factory = CreateFactory(port);
        using var invalidClient = factory.CreateClient();

        using var response = await invalidClient.PostAsJsonAsync("/images", body);

        Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        using var document = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
        Assert.Equal("VALIDATION_ERROR", document.RootElement.GetProperty("code").GetString());
        var fields = document.RootElement.GetProperty("errors").EnumerateArray()
            .Select(error => error.GetProperty("field").GetString());
        Assert.Contains(invalidField, fields);
        Assert.Equal(0, port.CallCount);
    }

    [Fact]
    public async Task PostImages_WhenBodyIsNotValidJson_ReturnsBadRequestWithoutInvokingService()
    {
        // ID: REQUEST-ENDPOINT-02
        // Source: docs/v1/api/controllers.md §5; docs/v1/api/api.md §11 (400 Bad Request), §14 step 1.
        using var content = new StringContent("{ this is not valid json", Encoding.UTF8, "application/json");

        using var response = await client.PostAsync("/images", content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        using var document = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
        Assert.Equal("BAD_REQUEST", document.RootElement.GetProperty("code").GetString());
    }

    [Fact]
    public async Task PostImages_WhenContentTypeIsNotJson_ReturnsBadRequest()
    {
        // ID: REQUEST-ENDPOINT-03
        // Source: docs/v1/api/api.md §5 (Headers), §11 (400 Bad Request); docs/v1/api/controllers.md §3, §5.
        using var content = new StringContent(
            JsonSerializer.Serialize(ValidRequestBody()), Encoding.UTF8, "text/plain");
        using var factory = CreateFactory();
        using var textPlainClient = factory.CreateClient();

        using var response = await textPlainClient.PostAsync("/images", content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        using var document = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
        Assert.Equal("BAD_REQUEST", document.RootElement.GetProperty("code").GetString());
    }

    [Fact]
    public async Task PostImages_WhenMultipleFieldsAreInvalid_ReturnsAllErrorsInErrorsArray()
    {
        // ID: REQUEST-ENDPOINT-04
        // Source: docs/v1/api/api.md §6 ("Whenever possible, all detected validation errors are included"), §11 (422 Unprocessable Entity); docs/v1/api/controllers.md §4 ("collect them in the errors array").
        var body = ValidRequestBody();
        body["text"] = "";
        body["foregroundColor"] = "not-a-hex-color";
        using var factory = CreateFactory();
        using var multiFieldClient = factory.CreateClient();

        using var response = await multiFieldClient.PostAsJsonAsync("/images", body);

        Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        using var document = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
        var fields = document.RootElement.GetProperty("errors").EnumerateArray()
            .Select(error => error.GetProperty("field").GetString())
            .ToList();
        Assert.Contains("text", fields);
        Assert.Contains("foregroundColor", fields);
    }

    [Fact]
    public async Task PostImages_WhenBothPatternCharactersAreWhitespaceOnly_ReturnsErrorsForBothFields()
    {
        // ID: REQUEST-ENDPOINT-05
        // Source: docs/v1/api/api.md §6 (Character Combination), §11 (Character Combination Error); docs/v1/api/controllers.md §4.
        var body = ValidRequestBody();
        body["foregroundCharacter"] = " ";
        body["backgroundCharacter"] = " ";
        using var factory = CreateFactory();
        using var whitespaceClient = factory.CreateClient();

        using var response = await whitespaceClient.PostAsJsonAsync("/images", body);

        Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        using var document = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
        var fields = document.RootElement.GetProperty("errors").EnumerateArray()
            .Select(error => error.GetProperty("field").GetString())
            .ToList();
        Assert.Contains("foregroundCharacter", fields);
        Assert.Contains("backgroundCharacter", fields);
    }

    [Fact]
    public async Task PostImages_WhenAcceptLanguageIsJapanese_ReturnsJapaneseValidationMessage()
    {
        // ID: REQUEST-ENDPOINT-06
        // Source: docs/v1/api/api.md §9 (Language Selection), §11 (Required Field Error, Japanese); docs/v1/api/controllers.md §8.
        var body = ValidRequestBody();
        body["text"] = null;

        using var response = await PostImagesAsync(client, body, "ja");

        using var document = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
        Assert.Equal(
            "描画する文字列は必須です。",
            document.RootElement.GetProperty("errors")[0].GetProperty("message").GetString());
    }

    [Fact]
    public async Task PostImages_WhenAcceptLanguageIsEnglish_ReturnsEnglishValidationMessage()
    {
        // ID: REQUEST-ENDPOINT-07
        // Source: docs/v1/api/api.md §9 (Language Selection), §11 (Required Field Error, English); docs/v1/api/controllers.md §8.
        var body = ValidRequestBody();
        body["text"] = null;

        using var response = await PostImagesAsync(client, body, "en");

        using var document = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
        Assert.Equal(
            "The text field is required.",
            document.RootElement.GetProperty("errors")[0].GetProperty("message").GetString());
    }

    [Fact]
    public async Task PostImages_WhenAcceptLanguageIsOmitted_FallsBackToJapanese()
    {
        // ID: REQUEST-ENDPOINT-08
        // Source: docs/v1/api/api.md §5 ("If Accept-Language is not specified, Japanese is used by default"), §9; docs/v1/api/controllers.md §8 (Language Selection table, "Omitted" -> Japanese).
        var body = ValidRequestBody();
        body["text"] = null;

        using var response = await PostImagesAsync(client, body, acceptLanguage: null);

        using var document = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
        Assert.Equal(
            "描画する文字列は必須です。",
            document.RootElement.GetProperty("errors")[0].GetProperty("message").GetString());
    }

    [Fact]
    public async Task PostImages_WhenAcceptLanguageIsUnsupported_FallsBackToJapanese()
    {
        // ID: REQUEST-ENDPOINT-09
        // Source: docs/v1/api/api.md §5 ("If an unsupported language is specified, the API also falls back to Japanese"), §9; docs/v1/api/controllers.md §8 (Language Selection table, "Unsupported value" -> Japanese).
        var body = ValidRequestBody();
        body["text"] = null;

        using var response = await PostImagesAsync(client, body, "fr");

        using var document = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
        Assert.Equal(
            "描画する文字列は必須です。",
            document.RootElement.GetProperty("errors")[0].GetProperty("message").GetString());
    }

    [Fact]
    public async Task PostImages_WhenAcceptLanguageChanges_KeepsCodeAndFieldStable()
    {
        // ID: REQUEST-ENDPOINT-10
        // Source: docs/v1/api/api.md §9 ("code and field are fixed values that do not depend on the selected language").
        var body = ValidRequestBody();
        body["text"] = null;

        using var japaneseResponse = await PostImagesAsync(client, body, "ja");
        using var japaneseDocument = await JsonDocument.ParseAsync(await japaneseResponse.Content.ReadAsStreamAsync());
        using var englishResponse = await PostImagesAsync(client, body, "en");
        using var englishDocument = await JsonDocument.ParseAsync(await englishResponse.Content.ReadAsStreamAsync());

        Assert.Equal(
            japaneseDocument.RootElement.GetProperty("code").GetString(),
            englishDocument.RootElement.GetProperty("code").GetString());
        Assert.Equal(
            japaneseDocument.RootElement.GetProperty("errors")[0].GetProperty("field").GetString(),
            englishDocument.RootElement.GetProperty("errors")[0].GetProperty("field").GetString());
        Assert.NotEqual(
            japaneseDocument.RootElement.GetProperty("errors")[0].GetProperty("message").GetString(),
            englishDocument.RootElement.GetProperty("errors")[0].GetProperty("message").GetString());
    }

    [Fact]
    public async Task PostImages_WhenServiceSucceeds_ReturnsGeneratedImageWithContentDisposition()
    {
        // ID: REQUEST-ENDPOINT-11
        // Source: docs/v1/api/api.md §10 (Successful Response); docs/v1/api/controllers.md §7 (Success Response).
        var body = ValidRequestBody();
        byte[] pngContent = [0x89, 0x50, 0x4E, 0x47];
        var port = new RecordingImageGenerationPort(
            ImageGenerationPortResult.Success(new GeneratedImageData(pngContent, "image/png")));
        using var factory = CreateFactory(port);
        using var successClient = factory.CreateClient();

        using var response = await successClient.PostAsJsonAsync("/images", body);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("image/png", response.Content.Headers.ContentType?.MediaType);
        Assert.Equal(pngContent, await response.Content.ReadAsByteArrayAsync());
        Assert.Equal("attachment", response.Content.Headers.ContentDisposition?.DispositionType);
        Assert.False(string.IsNullOrEmpty(response.Content.Headers.ContentDisposition?.FileName));
    }

    [Fact]
    public async Task PostImages_WhenRequestIsValid_InvokesServiceExactlyOnceWithMappedDomainRequest()
    {
        // ID: REQUEST-ENDPOINT-12
        // Source: docs/v1/api/controllers.md §4 (steps 4-7: convert to Value Objects, create ImageGenerationRequest, pass validated request to Service).
        var body = ValidRequestBody();
        var port = new RecordingImageGenerationPort(SuccessfulPortResult());
        using var factory = CreateFactory(port);
        using var invokeClient = factory.CreateClient();

        using var response = await invokeClient.PostAsJsonAsync("/images", body);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(1, port.CallCount);
        Assert.NotNull(port.ReceivedRequest);
        Assert.Equal(ImageType.Standard, port.ReceivedRequest!.Type);
        Assert.Equal(body["text"], port.ReceivedRequest.Text.Value);
    }

    public static TheoryData<ImageGenerationPortErrorCode, int?, HttpStatusCode, string> PortFailureCases => new()
    {
        { ImageGenerationPortErrorCode.RateLimited, 60, HttpStatusCode.TooManyRequests, "RATE_LIMIT_EXCEEDED" },
        { ImageGenerationPortErrorCode.Timeout, null, HttpStatusCode.GatewayTimeout, "IMAGE_GENERATION_TIMEOUT" },
        { ImageGenerationPortErrorCode.Unavailable, 30, HttpStatusCode.BadGateway, "IMAGE_GENERATION_FAILED" },
        { ImageGenerationPortErrorCode.InvalidResponse, null, HttpStatusCode.BadGateway, "IMAGE_GENERATION_FAILED" },
        { ImageGenerationPortErrorCode.Failed, null, HttpStatusCode.BadGateway, "IMAGE_GENERATION_FAILED" },
    };

    [Theory]
    [MemberData(nameof(PortFailureCases))]
    public async Task PostImages_WhenServiceReturnsPortFailure_ReturnsMappedHttpErrorWithRetryAfter(
        ImageGenerationPortErrorCode errorCode,
        int? retryAfter,
        HttpStatusCode expectedStatus,
        string expectedCode)
    {
        // ID: REQUEST-ENDPOINT-13..16, 18
        // Source: docs/v1/api/controllers.md §6 (Service Result Conversion table) and "Convert retryAfter to the Retry-After header"; docs/v1/api/api.md §11 (429/502/504).
        var body = ValidRequestBody();
        var error = new ImageGenerationPortError(errorCode, retryAfter);
        var port = new RecordingImageGenerationPort(ImageGenerationPortResult.Failure(error));
        using var factory = CreateFactory(port);
        using var failureClient = factory.CreateClient();

        using var response = await failureClient.PostAsJsonAsync("/images", body);

        Assert.Equal(expectedStatus, response.StatusCode);
        Assert.Equal(
            retryAfter.HasValue ? TimeSpan.FromSeconds(retryAfter.Value) : null,
            response.Headers.RetryAfter?.Delta);
        using var document = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
        Assert.Equal(expectedCode, document.RootElement.GetProperty("code").GetString());
    }

    [Fact]
    public async Task PostImages_WhenServiceReturnsOutputSizeExceeded_ReturnsUnprocessableEntityWithoutFieldTarget()
    {
        // ID: REQUEST-ENDPOINT-17
        // Source: docs/v1/api/controllers.md §5 ("return a top-level error without assigning the failure to one request field") and §6 (OUTPUT_SIZE_EXCEEDED -> 422 -> IMAGE_SIZE_LIMIT_EXCEEDED); docs/v1/api/api.md §11 (Generated Image Size Error).
        var body = ValidRequestBody();
        var error = new ImageGenerationPortError(ImageGenerationPortErrorCode.OutputSizeExceeded);
        var port = new RecordingImageGenerationPort(ImageGenerationPortResult.Failure(error));
        using var factory = CreateFactory(port);
        using var outputSizeClient = factory.CreateClient();

        using var response = await outputSizeClient.PostAsJsonAsync("/images", body);

        Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        using var document = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
        Assert.Equal("IMAGE_SIZE_LIMIT_EXCEEDED", document.RootElement.GetProperty("code").GetString());
        Assert.False(document.RootElement.TryGetProperty("errors", out _));
    }

    [Fact]
    public async Task PostImages_WhenServiceThrowsUnexpectedException_ReturnsInternalServerErrorWithoutInternalDetails()
    {
        // ID: REQUEST-ENDPOINT-19
        // Source: docs/v1/api/controllers.md §9 (Unexpected Exceptions); docs/v1/api/api.md §11 (500 Internal Server Error).
        var body = ValidRequestBody();
        var port = new RecordingImageGenerationPort(
            (ImageGenerationRequest request, CancellationToken cancellationToken) =>
                throw new InvalidOperationException("secret internal detail that must not leak"));
        using var factory = CreateFactory(port);
        using var exceptionClient = factory.CreateClient();

        using var response = await exceptionClient.PostAsJsonAsync("/images", body);

        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.DoesNotContain("secret internal detail", responseBody);
        Assert.DoesNotContain("InvalidOperationException", responseBody);
        using var document = JsonDocument.Parse(responseBody);
        Assert.Equal("INTERNAL_SERVER_ERROR", document.RootElement.GetProperty("code").GetString());
    }

    [Fact]
    public async Task PostImages_WhenLocalRateLimitIsExceeded_ReturnsTooManyRequestsWithoutCallingGlyphForge()
    {
        // ID: REQUEST-ENDPOINT-20
        // Source: docs/v1/api/api.md §13 (Rate Limiting), §14 step 5; docs/v1/api/implementation-plan.md Branch 11 ("wire all production dependencies"); ImageGenerationRateLimiterPolicy.PolicyName (backend/Mojica.Api/Infrastructure/ImageGenerationRateLimiterPolicy.cs).
        var body = ValidRequestBody();
        var port = new RecordingImageGenerationPort(SuccessfulPortResult());
        using var factory = CreateFactory(port, new Dictionary<string, string?>
        {
            ["RateLimit:PermitLimit"] = "1",
            ["RateLimit:Window"] = "00:01:00",
            ["RateLimit:QueueLimit"] = "0",
        });
        using var rateLimitedClient = factory.CreateClient();

        using var first = await rateLimitedClient.PostAsJsonAsync("/images", body);
        using var second = await rateLimitedClient.PostAsJsonAsync("/images", body);

        Assert.Equal(HttpStatusCode.OK, first.StatusCode);
        Assert.Equal(HttpStatusCode.TooManyRequests, second.StatusCode);
        Assert.NotNull(second.Headers.RetryAfter);
        using var document = await JsonDocument.ParseAsync(await second.Content.ReadAsStreamAsync());
        Assert.Equal("RATE_LIMIT_EXCEEDED", document.RootElement.GetProperty("code").GetString());
        Assert.Equal(1, port.CallCount);
    }

    [Fact]
    public async Task PostImages_WhenWithinLocalRateLimit_ProceedsToService()
    {
        // ID: REQUEST-ENDPOINT-21
        // Source: docs/v1/api/api.md §13, §14; docs/v1/api/implementation-plan.md Branch 11.
        var body = ValidRequestBody();
        var port = new RecordingImageGenerationPort(SuccessfulPortResult());
        using var factory = CreateFactory(port, new Dictionary<string, string?>
        {
            ["RateLimit:PermitLimit"] = "5",
            ["RateLimit:Window"] = "00:01:00",
            ["RateLimit:QueueLimit"] = "0",
        });
        using var withinLimitClient = factory.CreateClient();

        using var response = await withinLimitClient.PostAsJsonAsync("/images", body);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(1, port.CallCount);
    }

    public static TheoryData<string, string> TypeRoutingCases => new()
    {
        { "standard", "/images" },
        { "x-background", "/images/background" },
        { "x-icon", "/images/x-icon" },
    };

    [Theory]
    [MemberData(nameof(TypeRoutingCases))]
    public async Task PostImages_WhenTypeIsGiven_RoutesToMatchingGlyphForgeEndpoint(
        string type,
        string expectedPath)
    {
        // ID: REQUEST-ENDPOINT-22..24
        // Source: docs/v1/api/api.md §7 (Endpoint Routing table); docs/v1/api/implementation-plan.md Branch 11 ("wire all production dependencies").
        var body = ValidRequestBody();
        body["type"] = type;
        var handler = new RecordingHttpMessageHandler(SuccessfulGlyphForgeResponse());
        using var factory = CreateGlyphForgeFactory(handler);
        using var routingClient = factory.CreateClient();

        using var response = await routingClient.PostAsJsonAsync("/images", body);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(handler.LastRequest);
        Assert.Equal(expectedPath, handler.LastRequest!.RequestUri!.AbsolutePath);
    }

    [Fact]
    public async Task PostImages_WhenRequestIsValid_ConvertsHexColorsToRgbBeforeCallingGlyphForge()
    {
        // ID: REQUEST-ENDPOINT-25
        // Source: docs/v1/api/api.md §8 (Color Conversion), §14 step 6; docs/v1/api/controllers.md dependency direction (Controller -> Service -> Port <- Adapter).
        // Note: unit-level HEX-to-RGB conversion is already covered by GlyphForgeRequestMapperSmallTests; this case only proves the endpoint wires the real conversion path end to end, and should not re-assert every HEX/RGB pair.
        var body = ValidRequestBody();
        body["foregroundColor"] = "#FF69B4";
        body["backgroundColor"] = "#000000";
        var handler = new RecordingHttpMessageHandler(SuccessfulGlyphForgeResponse());
        using var factory = CreateGlyphForgeFactory(handler);
        using var conversionClient = factory.CreateClient();

        using var response = await conversionClient.PostAsJsonAsync("/images", body);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(handler.LastRequestBody);
        Assert.Contains("\"inner_color\":[255,105,180]", handler.LastRequestBody);
        Assert.Contains("\"outer_color\":[0,0,0]", handler.LastRequestBody);
    }

    [Fact]
    public void PostImages_WhenApplicationStarts_ResolvesFullDependencyChainWithoutThrowing()
    {
        // ID: REQUEST-ENDPOINT-26
        // Source: docs/v1/api/implementation-plan.md Branch 11 ("wire all production dependencies"); docs/v1/api/controllers.md §2 (Dependency Direction).
        using var factory = CreateFactory();

        using var scope = factory.Services.CreateScope();
        Assert.NotNull(scope.ServiceProvider.GetRequiredService<IImageGenerationService>());
        Assert.NotNull(scope.ServiceProvider.GetRequiredService<ImageGenerationPort>());
    }

    private static async Task<HttpResponseMessage> PostImagesAsync(
        HttpClient client,
        Dictionary<string, string?> body,
        string? acceptLanguage)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, "/images")
        {
            Content = JsonContent.Create(body),
        };
        if (acceptLanguage is not null)
        {
            request.Headers.AcceptLanguage.ParseAdd(acceptLanguage);
        }

        return await client.SendAsync(request);
    }

    private static Dictionary<string, string?> ValidRequestBody() => new()
    {
        ["type"] = "standard",
        ["text"] = "Mojica",
        ["foregroundCharacter"] = "@",
        ["foregroundColor"] = "#FF69B4",
        ["backgroundCharacter"] = ".",
        ["backgroundColor"] = "#000000",
    };

    private static ImageGenerationPortResult SuccessfulPortResult() =>
        ImageGenerationPortResult.Success(new GeneratedImageData([0x89, 0x50, 0x4E, 0x47], "image/png"));

    private static HttpResponseMessage SuccessfulGlyphForgeResponse() => new(HttpStatusCode.OK)
    {
        Content = new ByteArrayContent([0x89, 0x50, 0x4E, 0x47])
        {
            Headers = { ContentType = new MediaTypeHeaderValue("image/png") },
        },
    };

    private static WebApplicationFactory<Program> CreateGlyphForgeFactory(RecordingHttpMessageHandler handler)
    {
        return new ImageGenerationEndpointFactory().WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
                services.AddHttpClient("GlyphForge").ConfigurePrimaryHttpMessageHandler(() => handler));
        });
    }

    private static WebApplicationFactory<Program> CreateFactory(
        ImageGenerationPort? port = null,
        IDictionary<string, string?>? configOverrides = null)
    {
        return new ImageGenerationEndpointFactory().WithWebHostBuilder(builder =>
        {
            if (configOverrides is not null)
            {
                builder.ConfigureAppConfiguration((_, configuration) =>
                    configuration.AddInMemoryCollection(configOverrides));
            }

            if (port is not null)
            {
                builder.ConfigureTestServices(services => services.AddSingleton(port));
            }
        });
    }

    private sealed class RecordingImageGenerationPort : ImageGenerationPort
    {
        private readonly Func<ImageGenerationRequest, CancellationToken, Task<ImageGenerationPortResult>> handler;

        public RecordingImageGenerationPort(ImageGenerationPortResult result)
            : this((_, _) => Task.FromResult(result))
        {
        }

        public RecordingImageGenerationPort(
            Func<ImageGenerationRequest, CancellationToken, Task<ImageGenerationPortResult>> handler)
        {
            this.handler = handler;
        }

        public int CallCount { get; private set; }

        public ImageGenerationRequest? ReceivedRequest { get; private set; }

        public Task<ImageGenerationPortResult> GenerateAsync(
            ImageGenerationRequest request,
            CancellationToken cancellationToken)
        {
            CallCount++;
            ReceivedRequest = request;
            return handler(request, cancellationToken);
        }
    }

    private sealed class RecordingHttpMessageHandler(HttpResponseMessage response) : HttpMessageHandler
    {
        public HttpRequestMessage? LastRequest { get; private set; }

        public string? LastRequestBody { get; private set; }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            LastRequest = request;
            if (request.Content is not null)
            {
                LastRequestBody = await request.Content.ReadAsStringAsync(cancellationToken);
            }

            return response;
        }
    }
}

public sealed class ImageGenerationEndpointFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, configuration) =>
            configuration.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["GlyphForge:BaseUrl"] = "https://glyph-forge.example/",
                ["GlyphForge:Timeout"] = "00:00:35",
                ["RateLimit:PermitLimit"] = "1000",
                ["RateLimit:Window"] = "00:01:00",
                ["RateLimit:QueueLimit"] = "0"
            }));
    }
}
