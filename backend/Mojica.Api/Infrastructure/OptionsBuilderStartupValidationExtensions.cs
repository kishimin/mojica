using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Mojica.Api.Infrastructure;

internal static class OptionsBuilderStartupValidationExtensions
{
    // Development can serve local health checks without every external dependency configured;
    // deployed environments must fail fast when their required configuration is missing.
    public static void ValidateOnStartOutsideDevelopment<TOptions>(
        this OptionsBuilder<TOptions> optionsBuilder,
        IHostEnvironment environment)
        where TOptions : class
    {
        if (environment.IsDevelopment())
        {
            return;
        }

        optionsBuilder.ValidateOnStart();
    }
}
