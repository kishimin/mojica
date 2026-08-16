using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Text.Json;
using System.Threading.RateLimiting;
using Mojica.Api.Contracts;
using Mojica.Api.Infrastructure;
using Mojica.Api.Localization;
using Mojica.Api.Mapping;
using Mojica.Api.Ports;
using Mojica.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var glyphForgeOptions = builder.Services
    .AddOptions<GlyphForgeClientOptions>()
    .BindConfiguration(GlyphForgeClientOptions.SectionName);
builder.Services.AddSingleton<IValidateOptions<GlyphForgeClientOptions>, GlyphForgeClientOptionsValidator>();
glyphForgeOptions.ValidateOnStartOutsideDevelopment(builder.Environment);
builder.Services.AddHttpClient("GlyphForge", (serviceProvider, client) =>
{
    var options = serviceProvider.GetRequiredService<IOptions<GlyphForgeClientOptions>>().Value;
    client.BaseAddress = options.BaseUrl;
    client.Timeout = options.Timeout;
});
builder.Services.AddSingleton<ImageGenerationPort, GlyphForgeImageGenerationAdapter>();
builder.Services.AddSingleton<IImageGenerationService, ImageGenerationService>();

var rateLimitOptions = builder.Services
    .AddOptions<RateLimitOptions>()
    .BindConfiguration(RateLimitOptions.SectionName);
builder.Services.AddSingleton<IValidateOptions<RateLimitOptions>, RateLimitOptionsValidator>();
rateLimitOptions.ValidateOnStartOutsideDevelopment(builder.Environment);
builder.Services.AddRateLimiter(limiterOptions =>
{
    limiterOptions.OnRejected = RateLimitRejectionHandler.WriteAsync;
    limiterOptions.AddPolicy(ImageGenerationRateLimiterPolicy.PolicyName, httpContext =>
    {
        var options = httpContext.RequestServices
            .GetRequiredService<IOptions<RateLimitOptions>>().Value;
        return RateLimitPartition.Get(
            ImageGenerationRateLimiterPolicy.PolicyName,
            _ => ImageGenerationRateLimiterPolicy.CreateLimiter(options));
    });
});

var app = builder.Build();

app.UseRateLimiter();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/health", () => Results.Ok(new { status = "ok" }))
    .WithName("GetHealth")
    .WithOpenApi();

app.MapPost("/images", async (HttpContext context, IImageGenerationService service, ILogger<Program> logger) =>
{
    var language = ApiLanguageSelector.Select(context.Request.Headers.AcceptLanguage.ToString());

    ImageGenerationRequestDto? dto;
    try
    {
        dto = await context.Request.ReadFromJsonAsync<ImageGenerationRequestDto>(context.RequestAborted);
    }
    catch (Exception exception) when (exception is JsonException or InvalidOperationException)
    {
        return ToErrorResult(context, ApiErrorMapper.MapMalformedRequest(language));
    }

    var mapping = ImageGenerationRequestMapper.Map(dto!);
    if (!mapping.IsSuccess)
    {
        return ToErrorResult(context, ApiErrorMapper.MapValidationFailure(mapping.Errors, language));
    }

    try
    {
        var serviceResult = await service.GenerateAsync(mapping.Request!, context.RequestAborted);
        if (!serviceResult.IsSuccess)
        {
            return ToErrorResult(context, ApiErrorMapper.MapPortFailure(serviceResult.Error!, language));
        }

        var image = serviceResult.Image!;
        return Results.File(image.Content, image.MediaType, image.FileName);
    }
    catch (Exception exception)
    {
        logger.LogError(exception, "Unexpected error while generating an image.");
        return ToErrorResult(context, ApiErrorMapper.MapUnexpectedFailure(language));
    }
})
    .WithName("PostImages")
    .WithOpenApi();

app.Run();

static IResult ToErrorResult(HttpContext context, ApiErrorMappingResult error)
{
    if (error.RetryAfter is { } retryAfterSeconds)
    {
        context.Response.Headers.RetryAfter = retryAfterSeconds.ToString(CultureInfo.InvariantCulture);
    }

    return Results.Json(error.Response, statusCode: error.StatusCode);
}

public partial class Program { }
