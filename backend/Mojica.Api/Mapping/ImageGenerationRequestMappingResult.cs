using System.Collections.ObjectModel;
using Mojica.Api.Models;

namespace Mojica.Api.Mapping;

public sealed class ImageGenerationRequestMappingResult
{
    private ImageGenerationRequestMappingResult(
        ImageGenerationRequest? request,
        IReadOnlyList<ModelValidationError> errors)
    {
        Request = request;
        Errors = errors;
    }

    public bool IsSuccess => Request is not null;

    public ImageGenerationRequest? Request { get; }

    public IReadOnlyList<ModelValidationError> Errors { get; }

    public static ImageGenerationRequestMappingResult Success(
        ImageGenerationRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        return new(request, Array.Empty<ModelValidationError>());
    }

    public static ImageGenerationRequestMappingResult Failure(
        IEnumerable<ModelValidationError> errors)
    {
        ArgumentNullException.ThrowIfNull(errors);
        var errorList = errors.ToList();
        if (errorList.Count == 0)
        {
            throw new ArgumentException("A failed mapping requires at least one error.", nameof(errors));
        }

        return new(
            null,
            new ReadOnlyCollection<ModelValidationError>(errorList));
    }
}
