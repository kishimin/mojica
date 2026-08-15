using Mojica.Api.Models;

namespace Mojica.Api.Ports;

public sealed record GeneratedImageData
{
    private readonly ImageBinaryValue value;

    public GeneratedImageData(byte[] content, string mediaType)
    {
        value = new ImageBinaryValue(content, mediaType);
    }

    public byte[] Content => value.Content;

    public string MediaType => value.MediaType;

    public bool Equals(GeneratedImageData? other)
    {
        return other is not null && value.EqualsValue(other.value);
    }

    public override int GetHashCode()
    {
        // Mutable image content cannot safely participate in a stable hash code.
        return value.GetStableHashCode();
    }
}
