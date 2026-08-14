using Mojica.Api.Contracts;
using Mojica.Api.Mapping;

namespace Mojica.Api.Tests.Mapping;

public sealed class ImageGenerationRequestMapperSmallTests
{
    [Fact]
    public void Map_WhenAllValuesAreValid_ReturnsImageGenerationRequest()
    {
        var dto = new ImageGenerationRequestDto(
            "x-icon",
            "KA",
            "🌻",
            "#FFD400",
            "☀",
            "#FF69B4");

        var result = ImageGenerationRequestMapper.Map(dto);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Request);
        Assert.Empty(result.Errors);
        Assert.Equal("x-icon", result.Request.Type.Value);
        Assert.Equal("KA", result.Request.Text.Value);
        Assert.Equal("🌻", result.Request.ForegroundCharacter.Value);
        Assert.Equal("#FFD400", result.Request.ForegroundColor.Value);
        Assert.Equal("☀", result.Request.BackgroundCharacter.Value);
        Assert.Equal("#FF69B4", result.Request.BackgroundColor.Value);
    }

    [Fact(Skip = "TODO: Implement field-aware Value Object validation mapping.")]
    public void Map_WhenOneValueObjectCannotBeCreated_ReturnsReasonForItsRequestField()
    {
        // ID: REQUEST-MAPPING-02
        // Source: docs/v1/api/controllers.md §3-5; ADR-0022.
        // Given: each invalid request attribute in turn, including context-free PatternCharacter and HexColor failures (Theory candidate: all six attributes and their documented reasons)
        // When: the input Mapper converts the HTTP DTO values into an ImageGenerationRequest
        // Then: mapping fails, returns no aggregate, preserves the ModelValidationReason, and assigns the failing request attribute as Target
        // Error: Target is type, text, foregroundCharacter, foregroundColor, backgroundCharacter, or backgroundColor
        // Blocked by: define ImageGenerationRequestDto and ImageGenerationRequestMapper
        // Priority: High
    }

    [Fact(Skip = "TODO: Implement accumulation of independent field errors.")]
    public void Map_WhenMultipleValuesAreInvalid_ReturnsAllIndependentFieldErrors()
    {
        // ID: REQUEST-MAPPING-03
        // Source: docs/v1/api/controllers.md §4; docs/v1/api/api.md §11 422 Unprocessable Entity.
        // Given: a request DTO with independently invalid values in multiple attributes
        // When: the input Mapper maps the DTO
        // Then: mapping fails, returns no aggregate, and returns one classifiable error for every invalid attribute that can be evaluated
        // Error: do not stop after the first independent Value Object failure
        // Blocked by: define the Mapper result contract for multiple ModelValidationError values
        // Priority: High
    }

    [Fact(Skip = "TODO: Implement request-level visible-character validation mapping.")]
    public void Map_WhenBothPatternValuesContainNoVisibleCharacter_ReturnsErrorsForBothFields()
    {
        // ID: REQUEST-MAPPING-04
        // Source: docs/v1/api/api.md §6 Character Combination and §11 Character Combination Error; ADR-0022.
        // Given: individually valid foreground and background pattern values that contain no visible character
        // When: the input Mapper maps the DTO and creates the ImageGenerationRequest aggregate
        // Then: mapping fails and returns VISIBLE_CHARACTER_REQUIRED once for foregroundCharacter and once for backgroundCharacter
        // Error: expand the aggregate's combined target into the two public request fields without losing the reason
        // Blocked by: define the Mapper result contract for aggregate validation failures
        // Priority: High
    }

    [Fact(Skip = "TODO: Preserve validation detail values through the Mapper boundary.")]
    public void Map_WhenValidationReasonContainsDetails_PreservesThoseDetails()
    {
        // ID: REQUEST-MAPPING-05
        // Source: docs/v1/api/models.md validation contracts; docs/v1/api/implementation-plan.md §4 branch 7B.
        // Given: a request value whose validation failure includes machine-readable details such as a maximum length
        // When: the input Mapper returns the field error
        // Then: the error keeps the original reason, request-field target, and every detail value
        // Error: do not replace a detailed ModelValidationError with an unclassified string message
        // Blocked by: define ImageGenerationRequestMapper and its failure result
        // Priority: Medium
    }
}
