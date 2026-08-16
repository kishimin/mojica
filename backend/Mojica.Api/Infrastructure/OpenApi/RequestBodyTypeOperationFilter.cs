using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Mojica.Api.Infrastructure.OpenApi;

public sealed class RequestBodyTypeOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var attribute = context.MethodInfo.GetCustomAttributes(typeof(SwaggerRequestBodyTypeAttribute), inherit: false)
            .OfType<SwaggerRequestBodyTypeAttribute>()
            .FirstOrDefault();

        if (attribute is null)
        {
            return;
        }

        var schema = context.SchemaGenerator.GenerateSchema(attribute.Type, context.SchemaRepository);

        operation.RequestBody = new OpenApiRequestBody
        {
            Required = true,
            Content = new Dictionary<string, OpenApiMediaType>
            {
                ["application/json"] = new OpenApiMediaType { Schema = schema },
            },
        };
    }
}
