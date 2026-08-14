namespace Mojica.Api.Ports;

public sealed record ImageGenerationPortResult
{
    private ImageGenerationPortResult(
        GeneratedImageData? data,
        ImageGenerationPortError? error)
    {
        Data = data;
        Error = error;
    }

    public bool IsSuccess => Error is null;

    public GeneratedImageData? Data { get; }

    public ImageGenerationPortError? Error { get; }

    public static ImageGenerationPortResult Success(GeneratedImageData data)
    {
        return new ImageGenerationPortResult(data, null);
    }

    public static ImageGenerationPortResult Failure(ImageGenerationPortError error)
    {
        return new ImageGenerationPortResult(null, error);
    }
}
