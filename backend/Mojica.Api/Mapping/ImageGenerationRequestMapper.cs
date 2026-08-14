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

        var foregroundCharacter = CreatePatternCharacter(
            dto.ForegroundCharacter,
            "foregroundCharacter",
            errors);
        var foregroundColor = CreateHexColor(
            dto.ForegroundColor,
            "foregroundColor",
            errors);
        var backgroundCharacter = CreatePatternCharacter(
            dto.BackgroundCharacter,
            "backgroundCharacter",
            errors);
        var backgroundColor = CreateHexColor(
            dto.BackgroundColor,
            "backgroundColor",
            errors);

        if (errors.Count > 0)
        {
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

        var fieldErrors = requestError.Targets
            .Select(target => new ModelValidationError(
                target,
                requestError.Reason,
                requestError.Details));

        return ImageGenerationRequestMappingResult.Failure(fieldErrors);
    }

    private static PatternCharacter? CreatePatternCharacter(
        string? value,
        string target,
        ICollection<ModelValidationError> errors)
    {
        if (PatternCharacter.TryCreate(value, out var patternCharacter, out var reason))
        {
            return patternCharacter;
        }

        errors.Add(new ModelValidationError(target, reason));
        return null;
    }

    private static HexColor? CreateHexColor(
        string? value,
        string target,
        ICollection<ModelValidationError> errors)
    {
        if (HexColor.TryCreate(value, out var color, out var reason))
        {
            return color;
        }

        errors.Add(new ModelValidationError(target, reason));
        return null;
    }
}
