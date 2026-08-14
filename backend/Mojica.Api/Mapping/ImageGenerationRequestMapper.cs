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

        if (!PatternCharacter.TryCreate(
                dto.ForegroundCharacter,
                out var foregroundCharacter,
                out var foregroundCharacterReason))
        {
            errors.Add(new ModelValidationError(
                "foregroundCharacter",
                foregroundCharacterReason));
        }

        if (!HexColor.TryCreate(
                dto.ForegroundColor,
                out var foregroundColor,
                out var foregroundColorReason))
        {
            errors.Add(new ModelValidationError(
                "foregroundColor",
                foregroundColorReason));
        }

        if (!PatternCharacter.TryCreate(
                dto.BackgroundCharacter,
                out var backgroundCharacter,
                out var backgroundCharacterReason))
        {
            errors.Add(new ModelValidationError(
                "backgroundCharacter",
                backgroundCharacterReason));
        }

        if (!HexColor.TryCreate(
                dto.BackgroundColor,
                out var backgroundColor,
                out var backgroundColorReason))
        {
            errors.Add(new ModelValidationError(
                "backgroundColor",
                backgroundColorReason));
        }

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
}
