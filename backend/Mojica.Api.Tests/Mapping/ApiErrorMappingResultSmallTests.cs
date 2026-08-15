using Mojica.Api.Contracts;
using Mojica.Api.Mapping;

namespace Mojica.Api.Tests.Mapping;

public sealed class ApiErrorMappingResultSmallTests
{
    [Fact]
    public void Create_WhenResponseIsNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new ApiErrorMappingResult(400, null!));
    }

    [Fact]
    public void Create_WhenResponseIsProvided_ExposesConstructorArguments()
    {
        var response = new ApiErrorResponse("BAD_REQUEST", "The request format is invalid.");

        var result = new ApiErrorMappingResult(400, response, 7);

        Assert.Equal(400, result.StatusCode);
        Assert.Same(response, result.Response);
        Assert.Equal(7, result.RetryAfter);
    }
}
