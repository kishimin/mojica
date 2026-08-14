namespace Mojica.Api.Ports;

public sealed record GeneratedImageData(
    byte[] Content,
    string MediaType)
{
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
