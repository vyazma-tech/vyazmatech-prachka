using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VyazmaTech.Prachka.Application.BackgroundWorkers.Configuration;
using VyazmaTech.Prachka.Application.BackgroundWorkers.Integration;
using VyazmaTech.Prachka.Application.BackgroundWorkers.Queues;
using VyazmaTech.Prachka.Application.BackgroundWorkers.Queues.Jobs;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Configuration;

namespace VyazmaTech.Prachka.Application.BackgroundWorkers.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddQueueScheduling(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<QueueSchedulingConfiguration>(
            configuration.GetSection(QueueSchedulingConfiguration.SectionKey));

        services.AddWorkers();
        services.AddJobs();
        return services;
    }

    public static IApplicationBuilder UseSchedulingDashboard(this WebApplication app)
    {
        app.UseHangfireDashboard(
            options: new DashboardOptions
            {
                Authorization = [],
                DarkModeEnabled = true
            });

        return app;
    }

    private static IServiceCollection AddWorkers(this IServiceCollection services)
    {
        services.AddHostedService<OutboxMessagesProcessingBackgroundWorker>();
        services.AddHostedService<QueueSchedulingBackgroundWorker>();

        PostgresConfiguration postgresConfiguration = services.BuildServiceProvider()
            .GetRequiredService<PostgresConfiguration>();

        services.AddHangfire(
            config =>
                config
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UsePostgreSqlStorage(
                        options => options.UseNpgsqlConnection(postgresConfiguration.ToConnectionString())));

        services.AddHangfireServer(options => options.SchedulePollingInterval = TimeSpan.FromSeconds(1));

        return services;
    }

    private static IServiceCollection AddJobs(this IServiceCollection services)
    {
        services.AddTransient<QueueActivationJob>();
        services.AddTransient<QueueExpirationJob>();
        services.AddTransient<QueueJobScheduler>();

        return services;
    }
}