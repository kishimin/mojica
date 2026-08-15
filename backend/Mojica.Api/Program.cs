using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using System.Threading.RateLimiting;
using Mojica.Api.Infrastructure;
using Mojica.Api.Ports;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var glyphForgeOptions = builder.Services
    .AddOptions<GlyphForgeClientOptions>()
    .BindConfiguration(GlyphForgeClientOptions.SectionName);
builder.Services.AddSingleton<IValidateOptions<GlyphForgeClientOptions>, GlyphForgeClientOptionsValidator>();
if (!builder.Environment.IsDevelopment())
{
    // Development can serve local health checks without a running Glyph Forge instance;
    // deployed environments must fail fast when their required external configuration is missing.
    glyphForgeOptions.ValidateOnStart();
}
builder.Services.AddHttpClient("GlyphForge", (serviceProvider, client) =>
{
    var options = serviceProvider.GetRequiredService<IOptions<GlyphForgeClientOptions>>().Value;
    client.BaseAddress = options.BaseUrl;
    client.Timeout = options.Timeout;
});
builder.Services.AddSingleton<ImageGenerationPort, GlyphForgeImageGenerationAdapter>();

var rateLimitOptions = builder.Services
    .AddOptions<RateLimitOptions>()
    .BindConfiguration(RateLimitOptions.SectionName);
builder.Services.AddSingleton<IValidateOptions<RateLimitOptions>, RateLimitOptionsValidator>();
if (!builder.Environment.IsDevelopment())
{
    // Development can serve local health checks without a rate limit configured;
    // deployed environments must fail fast when their required configuration is missing.
    rateLimitOptions.ValidateOnStart();
}
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

app.Run();

public partial class Program { }
