using System.Collections.ObjectModel;
using Mojica.Api.Models;

namespace Mojica.Api.Mapping;

public sealed record ImageGenerationRequestMappingResult
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

    public bool Equals(ImageGenerationRequestMappingResult? other)
    {
        return other is not null
            && Equals(Request, other.Request)
            && Errors.SequenceEqual(other.Errors);
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(Request);

        foreach (var error in Errors)
        {
            hash.Add(error);
        }

        return hash.ToHashCode();
    }

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
