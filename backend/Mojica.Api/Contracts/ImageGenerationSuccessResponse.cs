using Mojica.Api.Models;

namespace Mojica.Api.Contracts;

public sealed record ImageGenerationSuccessResponse
{
    private readonly ImageFileValue value;

    public ImageGenerationSuccessResponse(
        byte[] content,
        string mediaType,
        string fileName)
    {
        value = new ImageFileValue(content, mediaType, fileName);
    }

    public byte[] Content => value.Binary.Content;

    public string MediaType => value.Binary.MediaType;

    public string FileName => value.FileName;

    public bool Equals(ImageGenerationSuccessResponse? other)
    {
        return other is not null && value.EqualsValue(other.value);
    }

    public override int GetHashCode()
    {
        // Mutable image content cannot safely participate in a stable hash code.
        return value.GetStableHashCode();
    }
}
