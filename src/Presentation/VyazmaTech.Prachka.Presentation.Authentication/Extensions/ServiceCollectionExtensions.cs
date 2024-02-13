using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace VyazmaTech.Prachka.Presentation.Authentication.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTelegramAuthentication(this IServiceCollection services)
    {
        services
            .AddAuthorization()
            .AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = "Bearer";
                o.DefaultChallengeScheme = "Bearer";
            })
            .AddScheme<TelegramAuthenticationOptions, TelegramAuthenticationHandler>("Bearer", options =>
            {
                options.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.HandleResponse();

                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = _ => Task.CompletedTask
                };
            });

        return services;
    }
}