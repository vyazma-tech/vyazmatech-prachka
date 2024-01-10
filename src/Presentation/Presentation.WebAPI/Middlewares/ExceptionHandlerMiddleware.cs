using System.Net;
using System.Text.Json;
using Domain.Common.Errors;
using FluentValidation;

namespace Presentation.WebAPI.Middlewares;

internal class ExceptionHandlerMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            await HandleExceptionAsync(context, e);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        (var httpStatusCode, var errors) = GetHttpStatusCodeAndErrors(exception);

        httpContext.Response.ContentType = "application/json";

        httpContext.Response.StatusCode = (int)httpStatusCode;

        var serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var response = JsonSerializer.Serialize(new { errors }, serializerOptions);

        await httpContext.Response.WriteAsync(response);
    }

    private static (HttpStatusCode httpStatusCode, IReadOnlyCollection<Error>) GetHttpStatusCodeAndErrors(
        Exception exception)
    {
        return exception switch
        {
            ValidationException validationException => (HttpStatusCode.BadRequest, validationException.Errors.Select(
                    x => Error.Validation(
                        x.ErrorCode,
                        $"{x.PropertyName} - {x.ErrorMessage}",
                        ErrorArea.Application))
                .ToList()),

            _ => (HttpStatusCode.InternalServerError, new[]
            {
                Error.Failure(
                    "InternalServerError",
                    exception.Message,
                    ErrorArea.Application)
            })
        };
    }
}

internal static class ExceptionHandlerMiddlewareExtensions
{
    internal static IServiceCollection AddCustomExceptionHandler(this IServiceCollection services)
    {
        return services.AddTransient<ExceptionHandlerMiddleware>();
    }

    internal static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlerMiddleware>();
    }
}