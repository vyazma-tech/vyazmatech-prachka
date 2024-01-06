using FastEndpoints;
using Microsoft.AspNetCore.Http;

namespace Presentation.Endpoints.Extensions;

public static class EndpointExtensions
{
    public static Task SendPartialContentAsync<TResponse>(
        this IEndpoint endpoint,
        TResponse response,
        CancellationToken cancellationToken)
    {
        return endpoint.HttpContext.Response.SendAsync(
            response,
            StatusCodes.Status206PartialContent,
            cancellation: cancellationToken);
    }
}