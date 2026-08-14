namespace Mojica.Api.Ports;

public sealed record GeneratedImageData
{
    public GeneratedImageData(byte[] content, string mediaType)
    {
        ArgumentNullException.ThrowIfNull(content);
        ArgumentNullException.ThrowIfNull(mediaType);

        Content = content;
        MediaType = mediaType;
    }

    public byte[] Content { get; }

    public string MediaType { get; }

    public bool Equals(GeneratedImageData? other)
    {
        return other is not null
            && Content.AsSpan().SequenceEqual(other.Content)
            && MediaType == other.MediaType;
    }

    public override int GetHashCode()
    {
        // Mutable image content cannot safely participate in a stable hash code.
        return MediaType.GetHashCode();
    }
}
