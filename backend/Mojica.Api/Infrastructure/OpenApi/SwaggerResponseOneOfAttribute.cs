namespace Mojica.Api.Infrastructure.OpenApi;

// [ProducesResponseType] cannot express "this status code returns one of several
// shapes" - Swashbuckle silently keeps only the last-registered type for a given
// status code. This attribute is the signal ResponseOneOfOperationFilter needs
// to combine several types into a single oneOf schema for one status code.
[AttributeUsage(AttributeTargets.Method)]
public sealed class SwaggerResponseOneOfAttribute(int statusCode, params Type[] types) : Attribute
{
    public int StatusCode { get; } = statusCode;

    public IReadOnlyList<Type> Types { get; } = types;
}
