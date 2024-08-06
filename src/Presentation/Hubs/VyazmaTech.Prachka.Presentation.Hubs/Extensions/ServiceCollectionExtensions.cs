namespace VyazmaTech.Prachka.Presentation.Hubs.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHubs(this IServiceCollection services)
    {
        services.AddSignalR();
        return services;
    }
}