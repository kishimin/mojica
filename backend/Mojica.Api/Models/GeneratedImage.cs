namespace Mojica.Api.Models;

public sealed record GeneratedImage
{
    private readonly ImageFileValue value;

    public GeneratedImage(byte[] content, string mediaType, string fileName)
    {
        value = new ImageFileValue(content, mediaType, fileName);
    }

    public byte[] Content => value.Binary.Content;

    public string MediaType => value.Binary.MediaType;

    public string FileName => value.FileName;

    public bool Equals(GeneratedImage? other)
    {
        return other is not null && value.EqualsValue(other.value);
    }

    public override int GetHashCode()
    {
        // Mutable image content cannot safely participate in a stable hash code.
        return value.GetStableHashCode();
    }
}
