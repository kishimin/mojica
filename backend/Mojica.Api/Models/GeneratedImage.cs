namespace Mojica.Api.Models;

public sealed record GeneratedImage
{
    public GeneratedImage(byte[] content, string mediaType, string fileName)
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

    public bool Equals(GeneratedImage? other)
    {
        return other is not null
            && Content.AsSpan().SequenceEqual(other.Content)
            && MediaType == other.MediaType
            && FileName == other.FileName;
    }

    public override int GetHashCode()
    {
        // Mutable image content cannot safely participate in a stable hash code.
        return HashCode.Combine(MediaType, FileName);
    }
}
