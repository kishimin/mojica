using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Mojica.Api.Contracts;
using Mojica.Api.Localization;

namespace Mojica.Api.Infrastructure;

public sealed class UploadRequestSizeLimitAttribute : Attribute, IAsyncResourceFilter
{
    private const long MaximumRequestBytes = 2 * 1024 * 1024 + 64 * 1024;

    public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
    {
        if (context.HttpContext.Request.ContentLength is > MaximumRequestBytes)
        {
            SetTooLarge(context);
            return;
        }

        if (context.HttpContext.Request.ContentLength is null)
        {
            var request = context.HttpContext.Request;
            var original = request.Body;
            using var buffered = new MemoryStream();
            var chunk = new byte[81920];
            try
            {
                int count;
                while ((count = await original.ReadAsync(chunk, context.HttpContext.RequestAborted)) > 0)
                {
                    if (buffered.Length > MaximumRequestBytes - count)
                    {
                        SetTooLarge(context);
                        return;
                    }
                    await buffered.WriteAsync(chunk.AsMemory(0, count), context.HttpContext.RequestAborted);
                }
            }
            catch (BadHttpRequestException error) when (error.StatusCode == StatusCodes.Status413PayloadTooLarge)
            {
                SetTooLarge(context);
                return;
            }

            buffered.Position = 0;
            request.Body = buffered;
            try
            {
                await next();
            }
            finally
            {
                request.Body = original;
            }
            return;
        }

        await next();
    }

    private static void SetTooLarge(ResourceExecutingContext context)
    {
        var language = ApiLanguageSelector.Select(context.HttpContext.Request.Headers.AcceptLanguage.ToString());
        context.Result = new ObjectResult(new ApiErrorResponse(
            PublicApiErrorCode.RequestTooLarge,
            ApiErrorMessageProvider.GetPublicMessage(language, PublicApiErrorCode.RequestTooLarge)))
        {
            StatusCode = StatusCodes.Status413PayloadTooLarge,
        };
    }
}
