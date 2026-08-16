using System.Globalization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Mojica.Api.Infrastructure.OpenApi;

public sealed class ResponseOneOfOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.MethodInfo is null)
        {
            return;
        }

        var attribute = context.MethodInfo.GetCustomAttributes(typeof(SwaggerResponseOneOfAttribute), inherit: false)
            .OfType<SwaggerResponseOneOfAttribute>()
            .FirstOrDefault();

        if (attribute is null)
        {
            return;
        }

        var statusCodeKey = attribute.StatusCode.ToString(CultureInfo.InvariantCulture);
        if (!operation.Responses.TryGetValue(statusCodeKey, out var response))
        {
            return;
        }

        var schemas = attribute.Types
            .Select(type => context.SchemaGenerator.GenerateSchema(type, context.SchemaRepository))
            .ToList();

        response.Content["application/json"] = new OpenApiMediaType
        {
            Schema = new OpenApiSchema { OneOf = schemas },
        };
    }
}
