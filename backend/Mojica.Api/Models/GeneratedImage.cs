namespace Mojica.Api.Models;

public sealed record GeneratedImage(
    byte[] Content,
    string MediaType,
    string FileName)
{
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
