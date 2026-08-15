namespace Mojica.Api.Models;

internal static class ImageFileValue
{
    public static void Validate(byte[] content, string mediaType, string fileName)
    {
        ArgumentNullException.ThrowIfNull(content);
        ArgumentNullException.ThrowIfNull(mediaType);
        ArgumentNullException.ThrowIfNull(fileName);
    }

    public static bool ContentEquals(
        byte[] content,
        byte[] otherContent,
        string mediaType,
        string otherMediaType,
        string fileName,
        string otherFileName)
    {
        return BinaryValueEquality.ContentEquals(content, otherContent)
            && mediaType == otherMediaType
            && fileName == otherFileName;
    }

}
