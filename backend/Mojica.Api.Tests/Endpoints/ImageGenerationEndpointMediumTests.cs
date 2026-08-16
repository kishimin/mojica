using System.Net;
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

    [Fact(Skip = "TODO: Implement the POST /images multi-field validation collection.")]
    public void PostImages_WhenMultipleFieldsAreInvalid_ReturnsAllErrorsInErrorsArray()
    {
        // ID: REQUEST-ENDPOINT-04
        // Source: docs/v1/api/api.md §6 ("Whenever possible, all detected validation errors are included"), §11 (422 Unprocessable Entity); docs/v1/api/controllers.md §4 ("collect them in the errors array").
        // Given: a POST /images request with at least two independently invalid fields (for example an invalid foregroundColor and a too-long text) and a controlled Service fake
        // When: the client sends the request through WebApplicationFactory
        // Then: the API returns 422 VALIDATION_ERROR whose errors array contains one entry per invalid field and does not invoke the Service
        // Error: 422 Unprocessable Entity; code VALIDATION_ERROR; errors.Count matches the number of independently invalid fields
        // Priority: High
    }

    [Fact(Skip = "TODO: Implement the POST /images whitespace-only character combination rule.")]
    public void PostImages_WhenBothPatternCharactersAreWhitespaceOnly_ReturnsErrorsForBothFields()
    {
        // ID: REQUEST-ENDPOINT-05
        // Source: docs/v1/api/api.md §6 (Character Combination), §11 (Character Combination Error); docs/v1/api/controllers.md §4.
        // Given: a POST /images request whose foregroundCharacter and backgroundCharacter are both whitespace-only, and a controlled Service fake
        // When: the client sends the request through WebApplicationFactory
        // Then: the API returns 422 VALIDATION_ERROR with one error entry targeting foregroundCharacter and one targeting backgroundCharacter, and does not invoke the Service
        // Error: 422 Unprocessable Entity; code VALIDATION_ERROR; errors contain both "foregroundCharacter" and "backgroundCharacter" targets
        // Priority: High
    }

    [Fact(Skip = "TODO: Implement Accept-Language wiring for the 422 validation contract.")]
    public void PostImages_WhenAcceptLanguageIsJapanese_ReturnsJapaneseValidationMessage()
    {
        // ID: REQUEST-ENDPOINT-06
        // Source: docs/v1/api/api.md §9 (Language Selection), §11 (Required Field Error, Japanese); docs/v1/api/controllers.md §8.
        // Given: an invalid POST /images request with header Accept-Language: ja and a controlled Service fake
        // When: the client sends the request through WebApplicationFactory
        // Then: the 422 response body's message and errors[].message are in Japanese
        // Priority: High
    }

    [Fact(Skip = "TODO: Implement Accept-Language wiring for the 422 validation contract.")]
    public void PostImages_WhenAcceptLanguageIsEnglish_ReturnsEnglishValidationMessage()
    {
        // ID: REQUEST-ENDPOINT-07
        // Source: docs/v1/api/api.md §9 (Language Selection), §11 (Required Field Error, English); docs/v1/api/controllers.md §8.
        // Given: an invalid POST /images request with header Accept-Language: en and a controlled Service fake
        // When: the client sends the request through WebApplicationFactory
        // Then: the 422 response body's message and errors[].message are in English
        // Priority: High
    }

    [Fact(Skip = "TODO: Implement Accept-Language fallback wiring for the 422 validation contract.")]
    public void PostImages_WhenAcceptLanguageIsOmitted_FallsBackToJapanese()
    {
        // ID: REQUEST-ENDPOINT-08
        // Source: docs/v1/api/api.md §5 ("If Accept-Language is not specified, Japanese is used by default"), §9; docs/v1/api/controllers.md §8 (Language Selection table, "Omitted" -> Japanese).
        // Given: an invalid POST /images request with no Accept-Language header and a controlled Service fake
        // When: the client sends the request through WebApplicationFactory
        // Then: the 422 response body's message and errors[].message are in Japanese
        // Priority: High
    }

    [Fact(Skip = "TODO: Implement Accept-Language fallback wiring for the 422 validation contract.")]
    public void PostImages_WhenAcceptLanguageIsUnsupported_FallsBackToJapanese()
    {
        // ID: REQUEST-ENDPOINT-09
        // Source: docs/v1/api/api.md §5 ("If an unsupported language is specified, the API also falls back to Japanese"), §9; docs/v1/api/controllers.md §8 (Language Selection table, "Unsupported value" -> Japanese).
        // Given: an invalid POST /images request with header Accept-Language: fr (unsupported) and a controlled Service fake
        // When: the client sends the request through WebApplicationFactory
        // Then: the 422 response body's message and errors[].message are in Japanese
        // Priority: Medium
    }

    [Fact(Skip = "TODO: Implement the language-independent code/field contract for 422 responses.")]
    public void PostImages_WhenAcceptLanguageChanges_KeepsCodeAndFieldStable()
    {
        // ID: REQUEST-ENDPOINT-10
        // Source: docs/v1/api/api.md §9 ("code and field are fixed values that do not depend on the selected language").
        // Given: the same invalid POST /images request sent once with Accept-Language: ja and once with Accept-Language: en, with a controlled Service fake
        // When: the client sends both requests through WebApplicationFactory
        // Then: both responses share the same top-level code and the same errors[].field values; only message and errors[].message differ
        // Priority: Medium
    }

    [Fact(Skip = "TODO: Implement the POST /images success response wiring.")]
    public void PostImages_WhenServiceSucceeds_ReturnsGeneratedImageWithContentDisposition()
    {
        // ID: REQUEST-ENDPOINT-11
        // Source: docs/v1/api/api.md §10 (Successful Response); docs/v1/api/controllers.md §7 (Success Response).
        // Given: a valid POST /images request and a controlled Service fake that returns a successful GeneratedImage(content, mediaType, fileName)
        // When: the client sends the request through WebApplicationFactory
        // Then: the response is 200 OK, Content-Type image/png, the body bytes equal GeneratedImage.Content, and Content-Disposition is attachment with GeneratedImage.FileName
        // Priority: High
    }

    [Fact(Skip = "TODO: Implement the Controller-to-Service handoff for valid requests.")]
    public void PostImages_WhenRequestIsValid_InvokesServiceExactlyOnceWithMappedDomainRequest()
    {
        // ID: REQUEST-ENDPOINT-12
        // Source: docs/v1/api/controllers.md §4 (steps 4-7: convert to Value Objects, create ImageGenerationRequest, pass validated request to Service).
        // Given: a valid POST /images request and a controlled Service fake that records its invocation count and received request
        // When: the client sends the request through WebApplicationFactory
        // Then: the Service fake was invoked exactly once with an ImageGenerationRequest whose values match the request body
        // Priority: High
    }

    [Fact(Skip = "TODO: Implement RATE_LIMITED Service-result to HTTP mapping wiring.")]
    public void PostImages_WhenServiceReturnsRateLimited_ReturnsTooManyRequestsWithRetryAfter()
    {
        // ID: REQUEST-ENDPOINT-13
        // Source: docs/v1/api/controllers.md §6 (Service Result Conversion table, RATE_LIMITED -> 429 -> RATE_LIMIT_EXCEEDED) and "Convert retryAfter to the Retry-After header"; docs/v1/api/api.md §11 (429 Too Many Requests).
        // Given: a valid POST /images request and a controlled Service fake that returns a RATE_LIMITED failure with a retryAfter value
        // When: the client sends the request through WebApplicationFactory
        // Then: the response is 429 with code RATE_LIMIT_EXCEEDED and a Retry-After header equal to the retryAfter value
        // Error: 429 Too Many Requests; code RATE_LIMIT_EXCEEDED
        // Priority: High

        // Theory candidate: REQUEST-ENDPOINT-13..18 share the same shape (PortErrorCode -> expected status/code) and can be consolidated into one [Theory] once fixtures exist.
    }

    [Fact(Skip = "TODO: Implement TIMEOUT Service-result to HTTP mapping wiring.")]
    public void PostImages_WhenServiceReturnsTimeout_ReturnsGatewayTimeout()
    {
        // ID: REQUEST-ENDPOINT-14
        // Source: docs/v1/api/controllers.md §6 (TIMEOUT -> 504 -> IMAGE_GENERATION_TIMEOUT); docs/v1/api/api.md §11 (504 Gateway Timeout).
        // Given: a valid POST /images request and a controlled Service fake that returns a TIMEOUT failure
        // When: the client sends the request through WebApplicationFactory
        // Then: the response is 504 with code IMAGE_GENERATION_TIMEOUT
        // Error: 504 Gateway Timeout; code IMAGE_GENERATION_TIMEOUT
        // Priority: High
    }

    [Fact(Skip = "TODO: Implement UNAVAILABLE Service-result to HTTP mapping wiring.")]
    public void PostImages_WhenServiceReturnsUnavailable_ReturnsBadGateway()
    {
        // ID: REQUEST-ENDPOINT-15
        // Source: docs/v1/api/controllers.md §6 (UNAVAILABLE -> 502 -> IMAGE_GENERATION_FAILED); docs/v1/api/api.md §11 (502 Bad Gateway).
        // Given: a valid POST /images request and a controlled Service fake that returns an UNAVAILABLE failure with a retryAfter value
        // When: the client sends the request through WebApplicationFactory
        // Then: the response is 502 with code IMAGE_GENERATION_FAILED and a Retry-After header equal to the retryAfter value when present
        // Error: 502 Bad Gateway; code IMAGE_GENERATION_FAILED
        // Priority: High
    }

    [Fact(Skip = "TODO: Implement INVALID_RESPONSE Service-result to HTTP mapping wiring.")]
    public void PostImages_WhenServiceReturnsInvalidResponse_ReturnsBadGateway()
    {
        // ID: REQUEST-ENDPOINT-16
        // Source: docs/v1/api/controllers.md §6 (INVALID_RESPONSE -> 502 -> IMAGE_GENERATION_FAILED).
        // Given: a valid POST /images request and a controlled Service fake that returns an INVALID_RESPONSE failure
        // When: the client sends the request through WebApplicationFactory
        // Then: the response is 502 with code IMAGE_GENERATION_FAILED
        // Error: 502 Bad Gateway; code IMAGE_GENERATION_FAILED
        // Priority: Medium
    }

    [Fact(Skip = "TODO: Implement OUTPUT_SIZE_EXCEEDED Service-result to HTTP mapping wiring.")]
    public void PostImages_WhenServiceReturnsOutputSizeExceeded_ReturnsUnprocessableEntityWithoutFieldTarget()
    {
        // ID: REQUEST-ENDPOINT-17
        // Source: docs/v1/api/controllers.md §5 ("return a top-level error without assigning the failure to one request field") and §6 (OUTPUT_SIZE_EXCEEDED -> 422 -> IMAGE_SIZE_LIMIT_EXCEEDED); docs/v1/api/api.md §11 (Generated Image Size Error).
        // Given: a valid POST /images request and a controlled Service fake that returns an OUTPUT_SIZE_EXCEEDED failure
        // When: the client sends the request through WebApplicationFactory
        // Then: the response is 422 with top-level code IMAGE_SIZE_LIMIT_EXCEEDED and is not shaped as a field-targeted ApiValidationErrorResponse
        // Error: 422 Unprocessable Entity; code IMAGE_SIZE_LIMIT_EXCEEDED
        // Priority: High
    }

    [Fact(Skip = "TODO: Implement FAILED Service-result to HTTP mapping wiring.")]
    public void PostImages_WhenServiceReturnsFailed_ReturnsBadGateway()
    {
        // ID: REQUEST-ENDPOINT-18
        // Source: docs/v1/api/controllers.md §6 (FAILED -> 502 -> IMAGE_GENERATION_FAILED).
        // Given: a valid POST /images request and a controlled Service fake that returns a FAILED failure
        // When: the client sends the request through WebApplicationFactory
        // Then: the response is 502 with code IMAGE_GENERATION_FAILED
        // Error: 502 Bad Gateway; code IMAGE_GENERATION_FAILED
        // Priority: Medium
    }

    [Fact(Skip = "TODO: Implement the unexpected-exception boundary for POST /images.")]
    public void PostImages_WhenServiceThrowsUnexpectedException_ReturnsInternalServerErrorWithoutInternalDetails()
    {
        // ID: REQUEST-ENDPOINT-19
        // Source: docs/v1/api/controllers.md §9 (Unexpected Exceptions); docs/v1/api/api.md §11 (500 Internal Server Error).
        // Given: a valid POST /images request and a controlled Service fake that throws an unexpected exception (for example InvalidOperationException)
        // When: the client sends the request through WebApplicationFactory
        // Then: the response is 500 with code INTERNAL_SERVER_ERROR, and the response body does not contain the exception message or a stack trace
        // Error: 500 Internal Server Error; code INTERNAL_SERVER_ERROR
        // Priority: High
    }

    [Fact(Skip = "TODO: Implement local rate-limit policy attachment for POST /images.")]
    public void PostImages_WhenLocalRateLimitIsExceeded_ReturnsTooManyRequestsWithoutCallingGlyphForge()
    {
        // ID: REQUEST-ENDPOINT-20
        // Source: docs/v1/api/api.md §13 (Rate Limiting), §14 step 5; docs/v1/api/implementation-plan.md Branch 11 ("wire all production dependencies"); ImageGenerationRateLimiterPolicy.PolicyName (backend/Mojica.Api/Infrastructure/ImageGenerationRateLimiterPolicy.cs).
        // Given: a WebApplicationFactory configured with a small RateLimit:PermitLimit, a fake Glyph Forge HTTP handler that records call count, and enough valid POST /images requests to exceed the configured permit limit
        // When: the client sends requests until the local limit is exceeded
        // Then: the response beyond the limit is 429 with code RATE_LIMIT_EXCEEDED and a Retry-After header, and the fake Glyph Forge handler recorded no call for the rejected request
        // Error: 429 Too Many Requests; code RATE_LIMIT_EXCEEDED
        // Priority: High
    }

    [Fact(Skip = "TODO: Implement local rate-limit policy attachment for POST /images.")]
    public void PostImages_WhenWithinLocalRateLimit_ProceedsToService()
    {
        // ID: REQUEST-ENDPOINT-21
        // Source: docs/v1/api/api.md §13, §14; docs/v1/api/implementation-plan.md Branch 11.
        // Given: a WebApplicationFactory configured with a RateLimit:PermitLimit large enough for a single request, and a controlled Service fake
        // When: the client sends one valid POST /images request through WebApplicationFactory
        // Then: the request is not rejected by the rate limiter (no 429) and the Service fake is invoked
        // Priority: Medium
    }

    [Fact(Skip = "TODO: Implement Glyph Forge endpoint routing for type=standard through the full production wiring.")]
    public void PostImages_WhenTypeIsStandard_RoutesToGlyphForgeStandardEndpoint()
    {
        // ID: REQUEST-ENDPOINT-22
        // Source: docs/v1/api/api.md §7 (Endpoint Routing table, standard -> POST /images); docs/v1/api/implementation-plan.md Branch 11 ("wire all production dependencies").
        // Given: a valid POST /images request with type=standard, the real ImageGenerationService and GlyphForgeImageGenerationAdapter wired through DI, and a fake Glyph Forge HTTP handler that records the requested path and returns a minimal successful PNG response
        // When: the client sends the request through WebApplicationFactory
        // Then: the fake Glyph Forge handler recorded a request to POST /images (not /images/background or /images/x-icon)
        // Priority: Medium

        // Theory candidate: REQUEST-ENDPOINT-22..24 share the same shape (type -> expected Glyph Forge path) and can be consolidated into one [Theory] once fixtures exist.
    }

    [Fact(Skip = "TODO: Implement Glyph Forge endpoint routing for type=x-background through the full production wiring.")]
    public void PostImages_WhenTypeIsXBackground_RoutesToGlyphForgeBackgroundEndpoint()
    {
        // ID: REQUEST-ENDPOINT-23
        // Source: docs/v1/api/api.md §7 (Endpoint Routing table, x-background -> POST /images/background).
        // Given: a valid POST /images request with type=x-background, the real ImageGenerationService and GlyphForgeImageGenerationAdapter wired through DI, and a fake Glyph Forge HTTP handler that records the requested path and returns a minimal successful PNG response
        // When: the client sends the request through WebApplicationFactory
        // Then: the fake Glyph Forge handler recorded a request to POST /images/background
        // Priority: Medium
    }

    [Fact(Skip = "TODO: Implement Glyph Forge endpoint routing for type=x-icon through the full production wiring.")]
    public void PostImages_WhenTypeIsXIcon_RoutesToGlyphForgeXIconEndpoint()
    {
        // ID: REQUEST-ENDPOINT-24
        // Source: docs/v1/api/api.md §7 (Endpoint Routing table, x-icon -> POST /images/x-icon).
        // Given: a valid POST /images request with type=x-icon, the real ImageGenerationService and GlyphForgeImageGenerationAdapter wired through DI, and a fake Glyph Forge HTTP handler that records the requested path and returns a minimal successful PNG response
        // When: the client sends the request through WebApplicationFactory
        // Then: the fake Glyph Forge handler recorded a request to POST /images/x-icon
        // Priority: Medium
    }

    [Fact(Skip = "TODO: Implement HEX-to-RGB conversion visibility through the full production wiring.")]
    public void PostImages_WhenRequestIsValid_ConvertsHexColorsToRgbBeforeCallingGlyphForge()
    {
        // ID: REQUEST-ENDPOINT-25
        // Source: docs/v1/api/api.md §8 (Color Conversion), §14 step 6; docs/v1/api/controllers.md dependency direction (Controller -> Service -> Port <- Adapter).
        // Given: a valid POST /images request with known foregroundColor/backgroundColor HEX values, the real production chain wired through DI, and a fake Glyph Forge HTTP handler that captures the request body it received
        // When: the client sends the request through WebApplicationFactory
        // Then: the captured Glyph Forge request body contains RGB values matching the known HEX-to-RGB conversion, not the original HEX strings
        // Priority: Medium

        // Note: unit-level HEX-to-RGB conversion is already covered by GlyphForgeRequestMapperSmallTests; this case only proves the endpoint wires the real conversion path end to end, and should not re-assert every HEX/RGB pair.
    }

    [Fact(Skip = "TODO: Implement full production dependency wiring for POST /images.")]
    public void PostImages_WhenApplicationStarts_ResolvesFullDependencyChainWithoutThrowing()
    {
        // ID: REQUEST-ENDPOINT-26
        // Source: docs/v1/api/implementation-plan.md Branch 11 ("wire all production dependencies"); docs/v1/api/controllers.md §2 (Dependency Direction).
        // Given: a WebApplicationFactory configured with valid GlyphForge and RateLimit options and no test-only service overrides
        // When: the factory's Services are first accessed (or a request is sent) through WebApplicationFactory
        // Then: IImageGenerationService, ImageGenerationPort, and the POST /images endpoint all resolve without throwing
        // Priority: Medium
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

    private static WebApplicationFactory<Program> CreateFactory(ImageGenerationPort? port = null)
    {
        return new ImageGenerationEndpointFactory().WithWebHostBuilder(builder =>
        {
            if (port is not null)
            {
                builder.ConfigureTestServices(services => services.AddSingleton(port));
            }
        });
    }

    private sealed class RecordingImageGenerationPort(
        ImageGenerationPortResult result) : ImageGenerationPort
    {
        public int CallCount { get; private set; }

        public ImageGenerationRequest? ReceivedRequest { get; private set; }

        public Task<ImageGenerationPortResult> GenerateAsync(
            ImageGenerationRequest request,
            CancellationToken cancellationToken)
        {
            CallCount++;
            ReceivedRequest = request;
            return Task.FromResult(result);
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
