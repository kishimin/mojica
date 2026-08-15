using Mojica.Api.Contracts;
using Mojica.Api.Models;

namespace Mojica.Api.Mapping;

public static class ImageGenerationRequestMapper
{
    public static ImageGenerationRequestMappingResult Map(
        ImageGenerationRequestDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var errors = new List<ModelValidationError>();

        if (!ImageType.TryCreate(dto.Type, out var type, out var typeError))
        {
            errors.Add(typeError);
        }

        if (!RenderText.TryCreate(dto.Text, out var text, out var textError))
        {
            errors.Add(textError);
        }

        var foregroundCharacter = CreateValue<PatternCharacter>(
            dto.ForegroundCharacter,
            "foregroundCharacter",
            errors,
            PatternCharacter.TryCreate);
        var foregroundColor = CreateValue<HexColor>(
            dto.ForegroundColor,
            "foregroundColor",
            errors,
            HexColor.TryCreate);
        var backgroundCharacter = CreateValue<PatternCharacter>(
            dto.BackgroundCharacter,
            "backgroundCharacter",
            errors,
            PatternCharacter.TryCreate);
        var backgroundColor = CreateValue<HexColor>(
            dto.BackgroundColor,
            "backgroundColor",
            errors,
            HexColor.TryCreate);

        if (errors.Count > 0)
        {
            if (foregroundCharacter is not null && backgroundCharacter is not null)
            {
                var patternError = ImageGenerationRequest.GetPatternCombinationFailure(
                    foregroundCharacter,
                    backgroundCharacter);
                if (patternError is not null)
                {
                    AddFieldErrors(errors, patternError);
                }
            }
            return ImageGenerationRequestMappingResult.Failure(errors);
        }

        if (ImageGenerationRequest.TryCreate(
            type,
            text,
            foregroundCharacter,
            foregroundColor,
            backgroundCharacter,
            backgroundColor,
            out var request,
            out var requestError))
        {
            return ImageGenerationRequestMappingResult.Success(request);
        }

        var fieldErrors = new List<ModelValidationError>();
        AddFieldErrors(fieldErrors, requestError);

        return ImageGenerationRequestMappingResult.Failure(fieldErrors);
    }

    private static void AddFieldErrors(
        ICollection<ModelValidationError> errors,
        ModelValidationError error)
    {
        foreach (var target in error.Targets)
        {
            errors.Add(new ModelValidationError(
                target,
                error.Reason,
                error.Details));
        }
    }

    private delegate bool TryCreateValue<T>(
        string? value,
        out T? result,
        out ModelValidationReason? reason);

    private static T? CreateValue<T>(
        string? value,
        string target,
        ICollection<ModelValidationError> errors,
        TryCreateValue<T> tryCreate)
        where T : class
    {
        if (tryCreate(value, out var result, out var reason))
        {
            return result;
        }

        errors.Add(new ModelValidationError(target, reason!));
        return null;
    }
}
