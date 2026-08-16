using System.Globalization;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Mojica.Api.Contracts;
using Mojica.Api.Infrastructure;
using Mojica.Api.Localization;
using Mojica.Api.Mapping;
using Mojica.Api.Services;

namespace Mojica.Api.Controllers;

[ApiController]
public sealed class ImageController(
    IImageGenerationService service,
    ILogger<ImageController> logger) : ControllerBase
{
    [HttpPost("/images")]
    [EnableRateLimiting(ImageGenerationRateLimiterPolicy.PolicyName)]
    public async Task<IActionResult> PostImages(CancellationToken cancellationToken)
    {
        var language = ApiLanguageSelector.Select(Request.Headers.AcceptLanguage.ToString());

        ImageGenerationRequestDto? dto;
        try
        {
            dto = await Request.ReadFromJsonAsync<ImageGenerationRequestDto>(cancellationToken);
        }
        catch (Exception exception) when (exception is JsonException or InvalidOperationException)
        {
            return ToErrorResult(ApiErrorMapper.MapMalformedRequest(language));
        }

        var mapping = ImageGenerationRequestMapper.Map(dto!);
        if (!mapping.IsSuccess)
        {
            return ToErrorResult(ApiErrorMapper.MapValidationFailure(mapping.Errors, language));
        }

        try
        {
            var serviceResult = await service.GenerateAsync(mapping.Request!, cancellationToken);
            if (!serviceResult.IsSuccess)
            {
                return ToErrorResult(ApiErrorMapper.MapPortFailure(serviceResult.Error!, language));
            }

            var image = serviceResult.Image!;
            return File(image.Content, image.MediaType, image.FileName);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Unexpected error while generating an image.");
            return ToErrorResult(ApiErrorMapper.MapUnexpectedFailure(language));
        }
    }

    private ObjectResult ToErrorResult(ApiErrorMappingResult error)
    {
        if (error.RetryAfter is { } retryAfterSeconds)
        {
            Response.Headers.RetryAfter = retryAfterSeconds.ToString(CultureInfo.InvariantCulture);
        }

        return StatusCode(error.StatusCode, error.Response);
    }
}
