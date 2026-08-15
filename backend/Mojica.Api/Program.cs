using Microsoft.Extensions.Options;
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
builder.Services.AddScoped<ImageGenerationPort, GlyphForgeImageGenerationAdapter>();

var app = builder.Build();

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
