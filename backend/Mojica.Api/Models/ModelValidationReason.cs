namespace Mojica.Api.Models;

public sealed record ModelValidationReason
{
    public static ModelValidationReason Required { get; } = new("REQUIRED");

    private ModelValidationReason(string value)
    {
        Value = value;
    }

    public string Value { get; }
}
