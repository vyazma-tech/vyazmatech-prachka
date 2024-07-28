using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VyazmaTech.Prachka.Application.BackgroundWorkers.Configuration;
using VyazmaTech.Prachka.Application.BackgroundWorkers.Integration;
using VyazmaTech.Prachka.Application.BackgroundWorkers.Queues;

namespace VyazmaTech.Prachka.Application.BackgroundWorkers.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOutbox(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OutboxConfiguration>(configuration.GetSection(OutboxConfiguration.SectionKey));

        services.AddWorkers();
        return services;
    }

    private static IServiceCollection AddWorkers(this IServiceCollection services)
    {
        services.AddHostedService<OutboxMessagesProcessingBackgroundWorker>();
        services.AddHostedService<OutboxMessagePublishingBackgroundWorker>();

        return services;
    }
}