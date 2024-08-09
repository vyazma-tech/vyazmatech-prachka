using Serilog;
using VyazmaTech.Prachka.Infrastructure.Jobs.Extensions;
using VyazmaTech.Prachka.Presentation.Endpoints.Extensions;
using VyazmaTech.Prachka.Presentation.Hubs.Extensions;
using VyazmaTech.Prachka.Presentation.WebAPI.Middlewares;

namespace VyazmaTech.Prachka.Presentation.WebAPI.Extensions;

public static class ApplicationBuilderExtensions
{
    public static WebApplication ConfigureApp(this WebApplication builder)
    {
        builder
            .UseSerilogRequestLogging()
            .UseAuthentication()
            .UseMiddleware<RequestLogContextMiddleware>()
            .UseMiddleware<GlobalExceptionHandlingMiddleware>()
            .UseRouting()
            .UseCors()
            .UseAuthorization()
            .UseEndpoints(builder.Environment)
            .UseSchedulingDashboard();

        builder.MapHubs();

        return builder;
    }
}