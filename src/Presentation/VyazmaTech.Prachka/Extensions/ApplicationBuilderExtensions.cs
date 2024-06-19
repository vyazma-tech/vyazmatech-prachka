﻿using FastEndpoints.Swagger;
using Serilog;
using VyazmaTech.Prachka.Presentation.Endpoints.Extensions;
using VyazmaTech.Prachka.Presentation.WebAPI.Middlewares;

namespace VyazmaTech.Prachka.Presentation.WebAPI.Extensions;

public static class ApplicationBuilderExtensions
{
    public static WebApplication ConfigureApp(this IApplicationBuilder builder)
    {
        builder
            .UseSerilogRequestLogging()
            .UseAuthentication()
            .UseMiddleware<RequestLogContextMiddleware>()
            .UseMiddleware<GlobalExceptionHandlingMiddleware>()
            .UseRouting()
            .UseAuthorization()
            .UseEndpoints()
            .UseSwaggerGen();

        return (WebApplication)builder;
    }
}