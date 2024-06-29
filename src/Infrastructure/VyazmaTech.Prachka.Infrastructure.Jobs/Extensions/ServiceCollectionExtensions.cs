using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.Extensions.DependencyInjection;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Configuration;
using VyazmaTech.Prachka.Infrastructure.Jobs.Commands.Factories;
using VyazmaTech.Prachka.Infrastructure.Jobs.Jobs;

namespace VyazmaTech.Prachka.Infrastructure.Jobs.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddJobs(this IServiceCollection services)
    {
        services.AddJobCommands();
        services.AddQueueJobs();
        services.AddHangfireJobStorage();

        return services;
    }

    private static IServiceCollection AddJobCommands(this IServiceCollection services)
    {
        services.AddTransient<SchedulingCommandFactory, ActivationCommandFactory>();
        services.AddTransient<SchedulingCommandFactory, ExpirationCommandFactory>();

        return services;
    }

    private static IServiceCollection AddQueueJobs(this IServiceCollection services)
    {
        services.AddTransient<QueueActivationJob>();
        services.AddTransient<QueueExpirationJob>();
        services.AddTransient<QueueJobScheduler>();

        return services;
    }

    private static IServiceCollection AddHangfireJobStorage(this IServiceCollection services)
    {
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
}