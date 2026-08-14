namespace Mojica.Api.Tests.Contracts;

public sealed class ApiValidationErrorResponseSmallTests
{
    [Fact(Skip = "TODO: Implement the validation-error response contract.")]
    public void Serialize_WhenValidationErrorsExist_WritesOverallErrorAndFieldErrors()
    {
        // ID: VALIDATION-RESPONSE-01
        // Source: docs/v1/api/controllers.md §5 422 Unprocessable Entity; docs/v1/api/api.md §9 and §11.
        // Given: VALIDATION_ERROR, a localized overall message, and multiple localized field errors
        // When: System.Text.Json serializes the validation response contract
        // Then: JSON contains code, message, and an errors array whose entries contain field and message
        // Blocked by: define ApiValidationErrorResponse and ApiValidationFieldError
        // Priority: High
    }

    [Fact(Skip = "TODO: Preserve every affected field in the public validation response.")]
    public void Serialize_WhenCombinationErrorAffectsTwoFields_WritesBothFieldEntries()
    {
        // ID: VALIDATION-RESPONSE-02
        // Source: docs/v1/api/api.md §11 Character Combination Error.
        // Given: localized VISIBLE_CHARACTER_REQUIRED errors for foregroundCharacter and backgroundCharacter
        // When: System.Text.Json serializes the validation response
        // Then: the errors array contains one entry for each affected field with the same localized message
        // Error: do not expose the Domain aggregate's comma-separated combined target as one public field
        // Blocked by: define ApiValidationErrorResponse and ApiValidationFieldError
        // Priority: High
    }

    [Fact(Skip = "TODO: Keep machine-readable fields independent from localization.")]
    public void Serialize_WhenMessageLanguageChanges_KeepsCodeAndFieldValuesStable()
    {
        // ID: VALIDATION-RESPONSE-03
        // Source: docs/v1/api/api.md §9 and §11.
        // Given: Japanese and English responses for the same validation reason and request field (Theory candidate: ja and en)
        // When: each response contract is serialized
        // Then: only message values differ while code and field values remain identical
        // Blocked by: define ApiValidationErrorResponse and ApiValidationFieldError
        // Priority: High
    }
}
