using Microsoft.EntityFrameworkCore;
using VyazmaTech.Prachka.Application.Abstractions.Configuration;
using VyazmaTech.Prachka.Application.Core.Extensions;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Configuration;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Extensions;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Interceptors;
using VyazmaTech.Prachka.Presentation.WebAPI.Exceptions;
using VyazmaTech.Prachka.Presentation.WebAPI.Middlewares;

namespace VyazmaTech.Prachka.Presentation.WebAPI.Extensions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPostgresConfiguration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        PostgresConfiguration? postgresConfiguration = configuration
            .GetSection(PostgresConfiguration.SectionKey)
            .Get<PostgresConfiguration>() ?? throw new StartupException(nameof(PostgresConfiguration));

        services.AddSingleton(postgresConfiguration);

        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddDatabase(
            (sp, o) =>
            {
                o.UseNpgsql(sp.GetRequiredService<PostgresConfiguration>().ToConnectionString())
                    .UseLazyLoadingProxies()
                    .AddInterceptors(sp.GetRequiredService<PublishDomainEventsInterceptor>());
            });

        return services;
    }

    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<PaginationConfiguration>(configuration.GetSection(PaginationConfiguration.SectionKey));

        services.AddQueryBuilders();
        services.AddQuerying();
        services.AddCurrentUsers();

        return services;
    }

    public static IServiceCollection AddMiddlewares(this IServiceCollection services)
    {
        return services
            .AddTransient<GlobalExceptionHandlingMiddleware>()
            .AddTransient<RequestLogContextMiddleware>();
    }
}