using Mojica.Api.Mapping;
using Mojica.Api.Models;

namespace Mojica.Api.Tests.Mapping;

public sealed class ImageGenerationRequestMappingResultSmallTests
{
    [Fact]
    public void Success_WhenRequestIsNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            ImageGenerationRequestMappingResult.Success(null!));
    }

    [Fact]
    public void Failure_WhenErrorsAreNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            ImageGenerationRequestMappingResult.Failure(null!));
    }

    [Fact]
    public void Failure_WhenErrorsAreEmpty_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
            ImageGenerationRequestMappingResult.Failure(
                Array.Empty<ModelValidationError>()));
    }
}
