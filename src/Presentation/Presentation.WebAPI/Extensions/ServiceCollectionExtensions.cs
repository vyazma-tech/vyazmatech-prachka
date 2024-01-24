using Application.BackgroundWorkers.Extensions;
using Application.BackgroundWorkers.Queue;
using Application.Core.Configuration;
using Application.Core.Extensions;
using Application.DataAccess.Contracts.Extensions;
using Application.Handlers.Extensions;
using Infrastructure.DataAccess.Extensions;
using Infrastructure.DataAccess.Interceptors;
using Microsoft.EntityFrameworkCore;
using Presentation.WebAPI.Configuration;
using Presentation.WebAPI.Exceptions;
using Presentation.WebAPI.Middlewares;

namespace Presentation.WebAPI.Extensions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWorkers(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddWorkersConfiguration(configuration);

        services
            .AddHostedService<QueueActivatorBackgroundWorker>()
            .AddHostedService<QueueActivityBackgroundWorker>()
            .AddHostedService<QueueAvailablePositionBackgroundWorker>();

        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        PostgresConfiguration? postgresConfiguration = configuration
            .GetSection(PostgresConfiguration.SectionKey)
            .Get<PostgresConfiguration>() ?? throw new StartupException(nameof(PostgresConfiguration));

        DbNameConfiguration? dbNameConfiguration = configuration
            .GetSection(DbNameConfiguration.SectionKey)
            .Get<DbNameConfiguration>() ?? throw new StartupException(nameof(DbNameConfiguration));

        services.AddSingleton(postgresConfiguration);
        services.AddDatabase((sp, o) =>
        {
            o.UseNpgsql(postgresConfiguration.ToConnectionString(dbNameConfiguration.DatabaseName))
                .UseLazyLoadingProxies()
                .AddInterceptors(sp.GetRequiredService<PublishDomainEventsInterceptor>());
        });

        return services;
    }

    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<PaginationConfiguration>(
            configuration.GetSection(PaginationConfiguration.SectionKey));

        services.AddQueryBuilders();
        services.AddQuerying();
        return services.AddHandlers();
    }

    public static IServiceCollection AddGlobalExceptionHandler(this IServiceCollection services)
    {
        return services
            .AddTransient<GlobalExceptionHandlingMiddleware>()
            .AddTransient<RequestLogContextMiddleware>();
    }
}