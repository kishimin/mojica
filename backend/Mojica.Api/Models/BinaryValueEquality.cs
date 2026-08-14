namespace Mojica.Api.Models;

internal static class BinaryValueEquality
{
    public static bool ContentEquals(byte[] left, byte[] right)
    {
        return left.AsSpan().SequenceEqual(right);
    }

    public static int GetStableHashCode(params string[] metadata)
    {
        var hash = new HashCode();
        foreach (var value in metadata)
        {
            hash.Add(value);
        }

        return hash.ToHashCode();
    }
}
