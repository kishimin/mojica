using System.Net.Http.Json;
using System.Text.Json;

namespace Mojica.Api.Tests.Controllers;

public sealed class ImageUploadSwaggerDocumentMediumTests
{
    [Fact]
    public async Task SwaggerDocument_ForPostFrameFile_DeclaresPngBinarySuccessResponse()
    {
        using var factory = new ImageControllerFactory();
        using var client = factory.CreateClient();
        using var document = await client.GetFromJsonAsync<JsonDocument>("/swagger/v1/swagger.json");

        var schema = document!.RootElement.GetProperty("paths")
            .GetProperty("/images/frame-file").GetProperty("post")
            .GetProperty("responses").GetProperty("200")
            .GetProperty("content").GetProperty("image/png").GetProperty("schema");

        Assert.Equal("string", schema.GetProperty("type").GetString());
        Assert.Equal("binary", schema.GetProperty("format").GetString());
    }
}
