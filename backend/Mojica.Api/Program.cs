using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using System.Threading.RateLimiting;
using Mojica.Api.Infrastructure;
using Mojica.Api.Infrastructure.OpenApi;
using Mojica.Api.Ports;
using Mojica.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => options.OperationFilter<RequestBodyTypeOperationFilter>());
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

app.MapControllers();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }))
    .WithName("GetHealth")
    .WithOpenApi();

app.Run();

public partial class Program { }
