using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Mojica.Api.Infrastructure.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Mojica.Api.Tests.Infrastructure.OpenApi;

public sealed class ResponseOneOfOperationFilterSmallTests
{
    [Fact]
    public void Apply_WhenMethodInfoIsNull_DoesNotThrow()
    {
        var context = new OperationFilterContext(
            new ApiDescription(),
            null!,
            new SchemaRepository(),
            null);

        var filter = new ResponseOneOfOperationFilter();
        var operation = new OpenApiOperation();

        var exception = Record.Exception(() => filter.Apply(operation, context));

        Assert.Null(exception);
    }

    [Fact]
    public void Apply_WhenMethodHasNoSwaggerResponseOneOfAttribute_LeavesResponsesUnchanged()
    {
        var methodInfo = typeof(UndecoratedTarget).GetMethod(nameof(UndecoratedTarget.Handle))!;
        var context = new OperationFilterContext(
            new ApiDescription(),
            null!,
            new SchemaRepository(),
            methodInfo);
        var operation = new OpenApiOperation
        {
            Responses = new OpenApiResponses
            {
                ["422"] = new OpenApiResponse(),
            },
        };

        new ResponseOneOfOperationFilter().Apply(operation, context);

        Assert.Empty(operation.Responses["422"].Content);
    }

    [Fact]
    public void Apply_WhenDeclaredStatusCodeIsNotInResponses_DoesNotThrowOrAddContent()
    {
        var methodInfo = typeof(DecoratedTarget).GetMethod(nameof(DecoratedTarget.Handle))!;
        var context = new OperationFilterContext(
            new ApiDescription(),
            null!,
            new SchemaRepository(),
            methodInfo);
        var operation = new OpenApiOperation
        {
            // The attribute declares status code 422, but only 400 is registered here -
            // this is the mismatch ResponseOneOfOperationFilter must tolerate.
            Responses = new OpenApiResponses
            {
                ["400"] = new OpenApiResponse(),
            },
        };

        var exception = Record.Exception(() => new ResponseOneOfOperationFilter().Apply(operation, context));

        Assert.Null(exception);
        Assert.Empty(operation.Responses["400"].Content);
    }

    [Fact]
    public void Apply_WhenDeclaredStatusCodeIsInResponses_SetsOneOfSchemaForApplicationJson()
    {
        var methodInfo = typeof(DecoratedTarget).GetMethod(nameof(DecoratedTarget.Handle))!;
        var schemaRepository = new SchemaRepository();
        var context = new OperationFilterContext(
            new ApiDescription(),
            new StubSchemaGenerator(),
            schemaRepository,
            methodInfo);
        var operation = new OpenApiOperation
        {
            Responses = new OpenApiResponses
            {
                ["422"] = new OpenApiResponse(),
            },
        };

        new ResponseOneOfOperationFilter().Apply(operation, context);

        var mediaType = Assert.Contains("application/json", operation.Responses["422"].Content);
        Assert.NotNull(mediaType.Schema.OneOf);
        Assert.Equal(2, mediaType.Schema.OneOf.Count);
    }

    private static class UndecoratedTarget
    {
        public static void Handle()
        {
        }
    }

    private static class DecoratedTarget
    {
        [SwaggerResponseOneOf(422, typeof(int), typeof(string))]
        public static void Handle()
        {
        }
    }

    private sealed class StubSchemaGenerator : ISchemaGenerator
    {
        public OpenApiSchema GenerateSchema(
            Type modelType,
            SchemaRepository schemaRepository,
            System.Reflection.MemberInfo? memberInfo = null,
            System.Reflection.ParameterInfo? parameterInfo = null,
            ApiParameterRouteInfo? routeInfo = null)
        {
            return new OpenApiSchema { Type = modelType.Name };
        }
    }
}
