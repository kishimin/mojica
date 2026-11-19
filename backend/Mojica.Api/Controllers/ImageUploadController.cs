using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Mojica.Api.Contracts;
using Mojica.Api.Infrastructure;
using Mojica.Api.Localization;
using Mojica.Api.Mapping;
using Mojica.Api.Services;

namespace Mojica.Api.Controllers;

[ApiController]
public sealed class ImageUploadController(ImageUploadService service) : ControllerBase
{
    [HttpPost("/images/frame-file")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK, "image/png")]
    [EnableRateLimiting(ImageGenerationRateLimiterPolicy.PolicyName)]
    [RequestSizeLimit(2 * 1024 * 1024 + 64 * 1024)]
    [UploadRequestSizeLimit]
    public async Task<IActionResult> PostFrameFile(
        IFormFile? frameImage,
        [FromForm] string? foregroundCharacter,
        [FromForm] string? foregroundColor,
        [FromForm] string? backgroundCharacter,
        [FromForm] string? backgroundColor,
        CancellationToken cancellationToken)
    {
        var language = ApiLanguageSelector.Select(Request.Headers.AcceptLanguage.ToString());
        if (frameImage is null)
        {
            var error = ApiErrorMapper.MapMalformedRequest(language);
            return StatusCode(error.StatusCode, error.Response);
        }
        foregroundCharacter = Request.Form.TryGetValue("foregroundCharacter", out var foregroundValues)
            ? foregroundValues.ToString() : null;
        backgroundCharacter = Request.Form.TryGetValue("backgroundCharacter", out var backgroundValues)
            ? backgroundValues.ToString() : null;
        var mapping = ImageGenerationRequestMapper.Map(new ImageGenerationRequestDto(
            "standard", "x", foregroundCharacter, foregroundColor, backgroundCharacter, backgroundColor));
        if (!mapping.IsSuccess)
        {
            var validation = ApiErrorMapper.MapValidationFailure(mapping.Errors, language);
            return StatusCode(validation.StatusCode, validation.Response);
        }

        await using var stream = frameImage.OpenReadStream();
        var result = await service.GenerateAsync(stream, mapping.Request!, cancellationToken);
        if (!result.IsSuccess)
        {
            var error = ApiErrorMapper.MapPortFailure(result.Error!, language);
            if (error.RetryAfter is { } retryAfter)
            {
                Response.Headers.RetryAfter = retryAfter.ToString(System.Globalization.CultureInfo.InvariantCulture);
            }
            return StatusCode(error.StatusCode, error.Response);
        }

        return File(result.Image!.Content, result.Image.MediaType, result.Image.FileName);
    }
}
