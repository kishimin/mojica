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

    [Fact(Skip = "TODO: Keep internal diagnostic details outside the public contract.")]
    public void Serialize_WhenErrorResponseIsCreated_DoesNotExposeInternalDetails()
    {
        // ID: ERROR-RESPONSE-02
        // Source: docs/v1/api/controllers.md §5 and §8-9; docs/v1/api/implementation-plan.md §6.
        // Given: a public API error response created from a safe code and localized message
        // When: System.Text.Json serializes the response
        // Then: the public JSON has no exception, stack trace, upstream body, internal URL, credential, or infrastructure-detail property
        // Error: internal diagnostics must not become fields of the public response DTO
        // Blocked by: define ApiErrorResponse
        // Priority: High
    }
}
