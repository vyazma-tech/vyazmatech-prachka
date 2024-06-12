using System.Net;
using System.Text.Json;
using FluentValidation;
using Npgsql;
using VyazmaTech.Prachka.Domain.Common.Errors;
using VyazmaTech.Prachka.Presentation.WebAPI.Models;

namespace VyazmaTech.Prachka.Presentation.WebAPI.Middlewares;

internal class GlobalExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    public GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Exception occured: {Message}", exception.Message);
            await HandleExceptionAsync(context, exception);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        ExceptionInformation exceptionInformation = GetExceptionInformation(exception);
        httpContext.Response.ContentType = "application/problem+json";
        httpContext.Response.StatusCode = (int)exceptionInformation.StatusCode;

        var serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        string response = JsonSerializer.Serialize(
            new { exceptionInformation.Errors },
            serializerOptions);

        await httpContext.Response.WriteAsync(response);
    }

    private static ExceptionInformation GetExceptionInformation(Exception exception)
    {
        return exception switch
        {
            ValidationException validationException => new ExceptionInformation
            {
                StatusCode = HttpStatusCode.BadRequest,
                Errors = validationException.Errors.Select(
                        x => Error.Validation(
                            x.ErrorCode,
                            $"{x.PropertyName} - {x.ErrorMessage}",
                            ErrorArea.Application))
                    .ToList(),
            },

            PostgresException postgresException => new ExceptionInformation
            {
                StatusCode = HttpStatusCode.ServiceUnavailable,
                Errors = new[]
                {
                    Error.Failure(
                        "Database Error",
                        postgresException.Detail ?? postgresException.Message,
                        ErrorArea.Infrastructure),
                },
            },

            _ => new ExceptionInformation
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Errors = new[]
                {
                    Error.Failure(
                        "Internal Server Error",
                        exception.Message,
                        ErrorArea.Application),
                },
            }
        };
    }
}