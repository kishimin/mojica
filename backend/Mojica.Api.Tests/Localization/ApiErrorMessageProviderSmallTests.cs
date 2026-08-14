namespace Mojica.Api.Tests.Localization;

public sealed class ApiErrorMessageProviderSmallTests
{
    [Fact(Skip = "TODO: Implement when the API error message provider exists.")]
    public void ApiErrorMessageProvider_GetPublicMessage_WhenLanguageIsJapanese_ReturnsDocumentedMessage()
    {
        // ID: LOCALIZATION-PUBLIC-01
        // Source: docs/v1/api/api.md §11 Error Responses.
        // Given: Japanese and each documented public API error code (Theory candidate: BAD_REQUEST, VALIDATION_ERROR, IMAGE_SIZE_LIMIT_EXCEEDED, RATE_LIMIT_EXCEEDED, INTERNAL_SERVER_ERROR, IMAGE_GENERATION_FAILED, IMAGE_GENERATION_TIMEOUT)
        // When: the public error message is resolved
        // Then: the exact documented Japanese message for that code is returned
        // Error: messages must not contain exception text, stack traces, upstream bodies, internal URLs, credentials, SQL, or infrastructure details
        // Blocked by: feature/add-api-error-localization must define the public error message lookup boundary
        // Priority: High
    }

    [Fact(Skip = "TODO: Implement when the API error message provider exists.")]
    public void ApiErrorMessageProvider_GetPublicMessage_WhenLanguageIsEnglish_ReturnsDocumentedMessage()
    {
        // ID: LOCALIZATION-PUBLIC-02
        // Source: docs/v1/api/api.md §11 Error Responses.
        // Given: English and each documented public API error code (Theory candidate: BAD_REQUEST, VALIDATION_ERROR, IMAGE_SIZE_LIMIT_EXCEEDED, RATE_LIMIT_EXCEEDED, INTERNAL_SERVER_ERROR, IMAGE_GENERATION_FAILED, IMAGE_GENERATION_TIMEOUT)
        // When: the public error message is resolved
        // Then: the exact documented English message for that code is returned
        // Error: messages must not contain exception text, stack traces, upstream bodies, internal URLs, credentials, SQL, or infrastructure details
        // Blocked by: feature/add-api-error-localization must define the public error message lookup boundary
        // Priority: High
    }

    [Fact(Skip = "TODO: Implement when the API error message provider exists.")]
    public void ApiErrorMessageProvider_GetValidationMessage_WhenLanguageIsJapanese_ReturnsMessageForReasonAndTarget()
    {
        // ID: LOCALIZATION-VALIDATION-01
        // Source: docs/v1/api/api.md §9 and §11 422 Unprocessable Entity; ADR-0022.
        // Given: Japanese, a documented ModelValidationReason, and the request target that owns the field context (Theory candidate for every supported reason-target pair)
        // When: the validation detail message is resolved
        // Then: the exact documented Japanese message for that validation condition and request attribute is returned
        // Error: the reason remains language-independent while the target supplies context such as text, foregroundCharacter, or backgroundColor
        // Blocked by: feature/add-api-error-localization must define the validation message lookup boundary
        // Priority: High
    }

    [Fact(Skip = "TODO: Implement when the API error message provider exists.")]
    public void ApiErrorMessageProvider_GetValidationMessage_WhenLanguageIsEnglish_ReturnsMessageForReasonAndTarget()
    {
        // ID: LOCALIZATION-VALIDATION-02
        // Source: docs/v1/api/api.md §9 and §11 422 Unprocessable Entity; ADR-0022.
        // Given: English, a documented ModelValidationReason, and the request target that owns the field context (Theory candidate for every supported reason-target pair)
        // When: the validation detail message is resolved
        // Then: the exact documented English message for that validation condition and request attribute is returned
        // Error: the reason remains language-independent while the target supplies context such as text, foregroundCharacter, or backgroundColor
        // Blocked by: feature/add-api-error-localization must define the validation message lookup boundary
        // Priority: High
    }
}
