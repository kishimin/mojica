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
        var hash = new HashCode();

        foreach (var value in Content)
        {
            hash.Add(value);
        }

        hash.Add(MediaType);
        hash.Add(FileName);
        return hash.ToHashCode();
    }
}
