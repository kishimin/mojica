using Mojica.Api.Contracts;

namespace Mojica.Api.Tests.Contracts;

public sealed class ApiValidationFieldErrorSmallTests
{
    [Fact]
    public void Create_WhenFieldIsNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new ApiValidationFieldError(null!, "The text field is required."));
    }

    [Fact]
    public void Create_WhenMessageIsNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new ApiValidationFieldError("text", null!));
    }
}
