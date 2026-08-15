namespace Mojica.Api.Models;

internal static class ValueEquality
{
    public static bool ContentEquals<T>(T[] left, T[] right)
        where T : IEquatable<T>
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
