using Mojica.Api.Models;

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

    public bool Equals(ImageGenerationSuccessResponse? other)
    {
        return other is not null
            && BinaryValueEquality.ContentEquals(Content, other.Content)
            && MediaType == other.MediaType
            && FileName == other.FileName;
    }

    public override int GetHashCode()
    {
        // Mutable image content cannot safely participate in a stable hash code.
        return BinaryValueEquality.GetStableHashCode(MediaType, FileName);
    }
}
