namespace Mojica.Api.Ports;

public sealed record ImageGenerationPortResult
{
    private ImageGenerationPortResult(GeneratedImageData data)
    {
        Data = data;
    }

    public bool IsSuccess => true;

    public GeneratedImageData Data { get; }

    public ImageGenerationPortError? Error => null;

    public static ImageGenerationPortResult Success(GeneratedImageData data)
    {
        return new ImageGenerationPortResult(data);
    }
}
