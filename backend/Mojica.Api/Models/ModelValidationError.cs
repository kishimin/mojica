using System.Collections.ObjectModel;

namespace Mojica.Api.Models;

public sealed record ModelValidationError
{
    private static readonly IReadOnlyDictionary<string, string> EmptyDetails =
        new ReadOnlyDictionary<string, string>(new Dictionary<string, string>());

    public ModelValidationError(
        string target,
        ModelValidationReason reason,
        IReadOnlyDictionary<string, string>? details = null)
    {
        Target = target;
        Reason = reason;
        Details = details is null
            ? EmptyDetails
            : new ReadOnlyDictionary<string, string>(
                new Dictionary<string, string>(details));
    }

    public string Code => Reason.Value;

    public string Target { get; }

    public ModelValidationReason Reason { get; }

    public IReadOnlyDictionary<string, string> Details { get; }

    public bool Equals(ModelValidationError? other)
    {
        return ReferenceEquals(this, other)
            || other is not null
            && string.Equals(Target, other.Target, StringComparison.Ordinal)
            && Equals(Reason, other.Reason)
            && Details.Count == other.Details.Count
            && Details.All(detail =>
                other.Details.TryGetValue(detail.Key, out var value)
                && string.Equals(detail.Value, value, StringComparison.Ordinal));
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(Target, StringComparer.Ordinal);
        hash.Add(Reason);

        foreach (var detail in Details.OrderBy(
                     detail => detail.Key,
                     StringComparer.Ordinal))
        {
            hash.Add(detail.Key, StringComparer.Ordinal);
            hash.Add(detail.Value, StringComparer.Ordinal);
        }

        return hash.ToHashCode();
    }
}
