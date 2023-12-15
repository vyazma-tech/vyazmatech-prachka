using Application.BackgroundWorkers.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application.BackgroundWorkers.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWorkersConfiguration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<QueueWorkerConfiguration>(configuration.GetSection(QueueWorkerConfiguration.SectionKey));

        return services;
    }
}