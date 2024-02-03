namespace Presentation.Authentication.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTelegramAuthentication(this IServiceCollection services)
    {
        services
            .AddAuthorization()
            .AddAuthentication(o => o.DefaultAuthenticateScheme = "Bearer")
            .AddScheme<TelegramAuthenticationOptions, TelegramAuthenticationHandler>("Bearer", _ => { });
        
        return services;
    }
}