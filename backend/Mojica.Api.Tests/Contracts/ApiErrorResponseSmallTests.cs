namespace Mojica.Api.Tests.Contracts;

public sealed class ApiErrorResponseSmallTests
{
    [Fact(Skip = "TODO: Implement the stable top-level API error contract.")]
    public void Serialize_WhenErrorResponseIsCreated_WritesCodeAndMessage()
    {
        // ID: ERROR-RESPONSE-01
        // Source: docs/v1/api/controllers.md §5-6 and §9; docs/v1/api/api.md §11.
        // Given: a public API error code and a localized safe message
        // When: System.Text.Json serializes the error response contract
        // Then: the JSON object contains the documented code and message properties with their supplied values
        // Blocked by: define ApiErrorResponse
        // Priority: High
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
