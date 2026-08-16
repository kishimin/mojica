using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
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

app.MapPost("/images", async (HttpContext context, IImageGenerationService service) =>
{
    var language = ApiLanguageSelector.Select(context.Request.Headers.AcceptLanguage.ToString());

    ImageGenerationRequestDto? dto;
    try
    {
        dto = await context.Request.ReadFromJsonAsync<ImageGenerationRequestDto>(context.RequestAborted);
    }
    catch (Exception exception) when (exception is JsonException or InvalidOperationException)
    {
        var malformed = ApiErrorMapper.MapMalformedRequest(language);
        return Results.Json(malformed.Response, statusCode: malformed.StatusCode);
    }

    var mapping = ImageGenerationRequestMapper.Map(dto!);
    if (!mapping.IsSuccess)
    {
        var validationFailure = ApiErrorMapper.MapValidationFailure(mapping.Errors, language);
        return Results.Json(validationFailure.Response, statusCode: validationFailure.StatusCode);
    }

    var serviceResult = await service.GenerateAsync(mapping.Request!, context.RequestAborted);
    if (!serviceResult.IsSuccess)
    {
        throw new NotImplementedException(
            "POST /images Service-failure response mapping is not yet implemented.");
    }

    var image = serviceResult.Image!;
    return Results.File(image.Content, image.MediaType, image.FileName);
})
    .WithName("PostImages")
    .WithOpenApi();

app.Run();

public partial class Program { }
