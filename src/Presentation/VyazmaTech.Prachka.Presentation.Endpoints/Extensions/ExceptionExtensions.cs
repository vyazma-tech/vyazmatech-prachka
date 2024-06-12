using Microsoft.AspNetCore.Http;
using VyazmaTech.Prachka.Domain.Common.Errors;
using VyazmaTech.Prachka.Domain.Common.Exceptions;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Extensions;

internal static class ExceptionExtensions
{
    public static IResult ToProblemDetails(this DomainException e)
    {
        return Results.Problem(
            statusCode: GetStatusCode(e.Error.Type),
            title: GetTitle(e.Error.Type),
            type: GetType(e.Error.Type),
            extensions: new Dictionary<string, object?>
            {
                { "errors", new[] { e.Error } },
            });

        static int GetStatusCode(ErrorType type)
        {
            return type switch
            {
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Unprocessable => StatusCodes.Status422UnprocessableEntity,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.BadRequest => StatusCodes.Status400BadRequest,
                ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
                ErrorType.Forbidden => StatusCodes.Status403Forbidden,
                ErrorType.Failure => StatusCodes.Status500InternalServerError,

                _ => StatusCodes.Status500InternalServerError
            };
        }

        static string GetTitle(ErrorType type)
        {
            return type switch
            {
                ErrorType.Validation => "Bad Request",
                ErrorType.Conflict => "Conflict",
                ErrorType.Unprocessable => "Unprocessable",
                ErrorType.NotFound => "Not Found",
                ErrorType.BadRequest => "Bad Request",
                ErrorType.Unauthorized => "Unauthorized",
                ErrorType.Forbidden => "No access",
                ErrorType.Failure => "Internal Server Error",

                _ => "Internal Server Error"
            };
        }

        static string GetType(ErrorType type)
        {
            return type switch
            {
                ErrorType.Validation => "https://www.rfc-editor.org/rfc/rfc9110.html#section-15.5.1",
                ErrorType.Conflict => "https://www.rfc-editor.org/rfc/rfc9110.html#section-15.5.10",
                ErrorType.Unprocessable => "https://www.rfc-editor.org/rfc/rfc9110.html#section-15.5.21",
                ErrorType.NotFound => "https://www.rfc-editor.org/rfc/rfc9110.html#section-15.5.5",
                ErrorType.BadRequest => "https://www.rfc-editor.org/rfc/rfc9110.html#section-15.5.1",
                ErrorType.Unauthorized => "https://www.rfc-editor.org/rfc/rfc9110.html#section-15.5.2",
                ErrorType.Forbidden => "https://www.rfc-editor.org/rfc/rfc9110.html#section-15.5.4",
                ErrorType.Failure => "Internal Server Error",

                _ => "Internal Server Error"
            };
        }
    }
}