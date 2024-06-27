using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VyazmaTech.Prachka.Application.BackgroundWorkers.Configuration;
using VyazmaTech.Prachka.Application.BackgroundWorkers.Integration;
using VyazmaTech.Prachka.Application.BackgroundWorkers.Queues;

namespace VyazmaTech.Prachka.Application.BackgroundWorkers.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddQueueScheduling(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<QueueSeedingConfiguration>(configuration.GetSection(QueueSeedingConfiguration.SectionKey));
        services.Configure<QueueJobOutboxConfiguration>(
            configuration.GetSection(QueueJobOutboxConfiguration.SectionKey));

        services.AddWorkers();
        return services;
    }

    private static IServiceCollection AddWorkers(this IServiceCollection services)
    {
        services.AddHostedService<OutboxMessagesProcessingBackgroundWorker>();
        services.AddHostedService<QueueSeedingBackgroundWorker>();

        // services.AddHostedService<QueueSchedulingBackgroundWorker>();
        return services;
    }
}