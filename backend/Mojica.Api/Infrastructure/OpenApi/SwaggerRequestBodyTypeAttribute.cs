namespace Mojica.Api.Infrastructure.OpenApi;

// Swashbuckle only infers a request body schema from a bound [FromBody] parameter.
// ImageController reads the body manually to control malformed-JSON error mapping,
// so this attribute is the only signal RequestBodyTypeOperationFilter has to work with.
[AttributeUsage(AttributeTargets.Method)]
public sealed class SwaggerRequestBodyTypeAttribute(Type type) : Attribute
{
    public Type Type { get; } = type;
}
