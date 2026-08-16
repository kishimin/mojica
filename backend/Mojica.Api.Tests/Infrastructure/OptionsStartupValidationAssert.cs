using Microsoft.Extensions.Options;
using Xunit;

namespace Mojica.Api.Tests.Infrastructure;

internal static class OptionsStartupValidationAssert
{
    public static void ContainsValidationFailure(Exception exception, string expectedMessage)
    {
        IEnumerable<Exception> failures = exception is AggregateException aggregate
            ? aggregate.InnerExceptions
            : new[] { exception };

        Assert.Contains(failures, failure =>
            failure is OptionsValidationException && failure.Message.Contains(expectedMessage));
    }
}
