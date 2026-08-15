namespace Mojica.Api.Contracts;

public interface IApiErrorResponse
{
    string Code { get; }

    string Message { get; }
}
