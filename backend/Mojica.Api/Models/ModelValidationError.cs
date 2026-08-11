using System.Collections.ObjectModel;

namespace Mojica.Api.Models;

public sealed record ModelValidationError
{
    public ModelValidationError(
        string target,
        ModelValidationReason reason,
        IReadOnlyDictionary<string, string>? details = null)
    {
        Target = target;
        Reason = reason;
        Details = details is null
            ? null
            : new ReadOnlyDictionary<string, string>(
                new Dictionary<string, string>(details));
    }

    public string Code => Reason.Value;

    public string Target { get; }

    public ModelValidationReason Reason { get; }

    public IReadOnlyDictionary<string, string>? Details { get; }
}
