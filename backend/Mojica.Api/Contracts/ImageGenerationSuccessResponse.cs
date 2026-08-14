namespace Mojica.Api.Contracts;

public sealed record ImageGenerationSuccessResponse
{
    public ImageGenerationSuccessResponse(
        byte[] content,
        string mediaType,
        string fileName)
    {
        ArgumentNullException.ThrowIfNull(content);
        ArgumentNullException.ThrowIfNull(mediaType);
        ArgumentNullException.ThrowIfNull(fileName);

        Content = content;
        MediaType = mediaType;
        FileName = fileName;
    }

    public byte[] Content { get; }

    public string MediaType { get; }

    public string FileName { get; }
}
