using System.Text.Json;
using Mojica.Api.Contracts;

namespace Mojica.Api.Tests.Contracts;

public sealed class ApiErrorResponseSmallTests
{
    [Fact]
    public void Serialize_WhenErrorResponseIsCreated_WritesCodeAndMessage()
    {
        var response = new ApiErrorResponse(
            "BAD_REQUEST",
            "The request format is invalid.");

        using var document = JsonDocument.Parse(JsonSerializer.Serialize(response));
        var root = document.RootElement;

        Assert.Equal("BAD_REQUEST", root.GetProperty("code").GetString());
        Assert.Equal("The request format is invalid.", root.GetProperty("message").GetString());
    }

    [Fact]
    public void Serialize_WhenErrorResponseIsCreated_DoesNotExposeInternalDetails()
    {
        var response = new ApiErrorResponse(
            "INTERNAL_SERVER_ERROR",
            "An unexpected error occurred during image generation.");

        using var document = JsonDocument.Parse(JsonSerializer.Serialize(response));

        Assert.Equal(
            ["code", "message"],
            document.RootElement.EnumerateObject().Select(property => property.Name));
    }
}
