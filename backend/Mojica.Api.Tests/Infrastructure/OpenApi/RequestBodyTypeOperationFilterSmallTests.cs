using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Mojica.Api.Infrastructure.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Mojica.Api.Tests.Infrastructure.OpenApi;

public sealed class RequestBodyTypeOperationFilterSmallTests
{
    [Fact]
    public void Apply_WhenMethodInfoIsNull_DoesNotThrow()
    {
        var context = new OperationFilterContext(
            new ApiDescription(),
            null!,
            new SchemaRepository(),
            null);

        var filter = new RequestBodyTypeOperationFilter();
        var operation = new OpenApiOperation();

        var exception = Record.Exception(() => filter.Apply(operation, context));

        Assert.Null(exception);
    }
}
