using System.Net.Http.Json;
using System.Text.Json;

namespace Mojica.Api.Tests.Controllers;

public sealed class ImageControllerSwaggerDocumentMediumTests : IClassFixture<ImageControllerFactory>
{
    private readonly HttpClient client;

    public ImageControllerSwaggerDocumentMediumTests(ImageControllerFactory factory)
    {
        client = factory.CreateClient();
    }

    [Fact]
    public async Task SwaggerDocument_ForPostImages_DeclaresARequestBody()
    {
        var operation = await GetPostImagesOperationAsync();

        Assert.True(operation.TryGetProperty("requestBody", out var requestBody));
        Assert.True(requestBody
            .GetProperty("content")
            .TryGetProperty("application/json", out var jsonContent));
        Assert.True(jsonContent.TryGetProperty("schema", out _));
    }

    public static TheoryData<string> DocumentedStatusCodes => new()
    {
        "200", "400", "422", "429", "500", "502", "504",
    };

    [Theory]
    [MemberData(nameof(DocumentedStatusCodes))]
    public async Task SwaggerDocument_ForPostImages_DeclaresStatusCode(string statusCode)
    {
        var operation = await GetPostImagesOperationAsync();

        var responses = operation.GetProperty("responses");

        Assert.True(
            responses.TryGetProperty(statusCode, out _),
            $"Expected the /images operation to document status code {statusCode}.");
    }

    [Fact]
    public async Task SwaggerDocument_ForPostImages_Declares422AsEitherValidationOrPlainError()
    {
        // ApiErrorMapper returns ApiValidationErrorResponse for field validation
        // failures but a plain ApiErrorResponse (no "errors" array) for the
        // IMAGE_SIZE_LIMIT_EXCEEDED case (docs/v1/api/api.md, Generated Image
        // Size Error). Both share status 422, so the schema must document both.
        var operation = await GetPostImagesOperationAsync();

        var schema = operation
            .GetProperty("responses")
            .GetProperty("422")
            .GetProperty("content")
            .GetProperty("application/json")
            .GetProperty("schema");

        Assert.True(schema.TryGetProperty("oneOf", out var oneOf));

        var referencedSchemas = oneOf.EnumerateArray()
            .Select(element => element.GetProperty("$ref").GetString())
            .ToList();

        Assert.Contains("#/components/schemas/ApiValidationErrorResponse", referencedSchemas);
        Assert.Contains("#/components/schemas/ApiErrorResponse", referencedSchemas);
    }

    private async Task<JsonElement> GetPostImagesOperationAsync()
    {
        using var document = await client.GetFromJsonAsync<JsonDocument>("/swagger/v1/swagger.json");

        return document!.RootElement
            .GetProperty("paths")
            .GetProperty("/images")
            .GetProperty("post")
            .Clone();
    }
}
