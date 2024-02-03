using Domain.Common.Errors;
using Domain.Common.Result;
using Microsoft.AspNetCore.Http;

namespace Presentation.Endpoints.Extensions;

internal static class ResultExtensions
{
    public static IResult ToProblemDetails<TResponse>(this Result<TResponse> result)
    {
        return Results.Problem(
            statusCode: GetStatusCode(result.Error.Type),
            title: GetTitle(result.Error.Type),
            type: GetType(result.Error.Type),
            extensions: new Dictionary<string, object?>
            {
                { "errors", new[] { result.Error } }
            });

        static int GetStatusCode(ErrorType type)
            => type switch
            {
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Unprocessable => StatusCodes.Status422UnprocessableEntity,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.BadRequest => StatusCodes.Status400BadRequest,
                ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,

                _ => StatusCodes.Status500InternalServerError
            };

        static string GetTitle(ErrorType type)
            => type switch
            {
                ErrorType.Validation => "Bad Request",
                ErrorType.Conflict => "Conflict",
                ErrorType.Unprocessable => "Unprocessable",
                ErrorType.NotFound => "Not Found",
                ErrorType.BadRequest => "Bad Request",
                ErrorType.Unauthorized => "Unauthorized",

                _ => "Internal Server Error"
            };

        static string GetType(ErrorType type)
            => type switch
            {
                ErrorType.Validation => "https://www.rfc-editor.org/rfc/rfc9110.html#section-15.5.1",
                ErrorType.Conflict => "https://www.rfc-editor.org/rfc/rfc9110.html#section-15.5.10",
                ErrorType.Unprocessable => "https://www.rfc-editor.org/rfc/rfc9110.html#section-15.5.21",
                ErrorType.NotFound => "https://www.rfc-editor.org/rfc/rfc9110.html#section-15.5.5",
                ErrorType.BadRequest => "https://www.rfc-editor.org/rfc/rfc9110.html#section-15.5.1",
                ErrorType.Unauthorized => "https://www.rfc-editor.org/rfc/rfc9110.html#section-15.5.2",

                _ => "Internal Server Error"
            };
    }
}