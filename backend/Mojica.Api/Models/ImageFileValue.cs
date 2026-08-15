namespace Mojica.Api.Models;

internal sealed class ImageBinaryValue
{
    public ImageBinaryValue(byte[] content, string mediaType)
    {
        ArgumentNullException.ThrowIfNull(content);
        ArgumentNullException.ThrowIfNull(mediaType);

        Content = content;
        MediaType = mediaType;
    }

    public byte[] Content { get; }

    public string MediaType { get; }

    public bool EqualsValue(ImageBinaryValue? other)
    {
        return other is not null
            && BinaryValueEquality.ContentEquals(Content, other.Content)
            && MediaType == other.MediaType;
    }

    public int GetStableHashCode()
    {
        return BinaryValueEquality.GetStableHashCode(MediaType);
    }
}

internal sealed class ImageFileValue
{
    public ImageFileValue(byte[] content, string mediaType, string fileName)
    {
        ArgumentNullException.ThrowIfNull(fileName);

        Binary = new ImageBinaryValue(content, mediaType);
        FileName = fileName;
    }

    public ImageBinaryValue Binary { get; }

    public string FileName { get; }

    public bool EqualsValue(ImageFileValue? other)
    {
        return other is not null
            && Binary.EqualsValue(other.Binary)
            && FileName == other.FileName;
    }

    public int GetStableHashCode()
    {
        return BinaryValueEquality.GetStableHashCode(Binary.MediaType, FileName);
    }
}
