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
        : this([target], reason, details)
    {
    }

    public ModelValidationError(
        IReadOnlyList<string> targets,
        ModelValidationReason reason,
        IReadOnlyDictionary<string, string>? details = null)
    {
        ArgumentNullException.ThrowIfNull(targets);

        if (targets.Count == 0)
        {
            throw new ArgumentException("At least one validation target is required.", nameof(targets));
        }

        Targets = new ReadOnlyCollection<string>(targets.ToList());
        Target = string.Join(',', Targets);
        Reason = reason;
        Details = details is null
            ? EmptyDetails
            : new ReadOnlyDictionary<string, string>(
                new Dictionary<string, string>(details));
    }

    public string Code => Reason.Value;

    public string Target { get; }

    public IReadOnlyList<string> Targets { get; }

    public ModelValidationReason Reason { get; }

    public IReadOnlyDictionary<string, string> Details { get; }

    public bool Equals(ModelValidationError? other)
    {
        return ReferenceEquals(this, other)
            || other is not null
            && Targets.SequenceEqual(other.Targets, StringComparer.Ordinal)
            && Equals(Reason, other.Reason)
            && Details.Count == other.Details.Count
            && Details.All(detail =>
                other.Details.TryGetValue(detail.Key, out var value)
                && string.Equals(detail.Value, value, StringComparison.Ordinal));
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();
        foreach (var target in Targets)
        {
            hash.Add(target, StringComparer.Ordinal);
        }

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
