using System.Net;
using System.Text.Json;
using Domain.Common.Errors;
using Domain.Common.Exceptions;
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
                x =>
                    new Error(x.ErrorCode, $"{x.PropertyName} - {x.ErrorMessage}")).ToList().AsReadOnly()),
            DomainException domainException => (HttpStatusCode.BadRequest, new[] { domainException.Error }),
            _ => (HttpStatusCode.InternalServerError, new[] { new Error("InternalServerError", exception.Message) })
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