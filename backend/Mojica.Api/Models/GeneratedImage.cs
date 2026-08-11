namespace Mojica.Api.Models;

public sealed record GeneratedImage(
    byte[] Content,
    string MediaType,
    string FileName);
