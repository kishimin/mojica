namespace Mojica.Api.Models;

public sealed record ModelValidationError(
    string Code,
    string Target,
    ModelValidationReason Reason,
    IReadOnlyDictionary<string, string>? Details = null);
