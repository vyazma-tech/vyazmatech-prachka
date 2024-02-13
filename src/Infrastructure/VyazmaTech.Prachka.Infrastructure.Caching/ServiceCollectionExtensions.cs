using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.DependencyInjection.Extensions;
using StackExchange.Redis;

namespace VyazmaTech.Prachka.Infrastructure.Caching;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCaching(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddRedisConnectionMultiplexer(
            options =>
            {
                options.AddBasePolicy(
                    policyBuilder =>
                        policyBuilder.AddPolicy<QueueCachePolicy>(), excludeDefaultPolicy: true);
            }, configuration);
    }

    private static IServiceCollection AddRedisConnectionMultiplexer(
        this IServiceCollection services,
        Action<OutputCacheOptions> configureOptions,
        IConfiguration configuration)
    {
        RedisCacheConfiguration? cacheConfiguration = services.AddRedisCache(configuration);

        if (cacheConfiguration is null)
            return services;

        services.AddSingleton<IConnectionMultiplexer>(_ =>
            ConnectionMultiplexer.Connect(cacheConfiguration.ConnectionString));

        return services.AddRedisOutputCacheStore(configureOptions);
    }

    private static IServiceCollection AddRedisOutputCacheStore(
        this IServiceCollection services,
        Action<OutputCacheOptions> configureOptions)
    {
        services.AddOutputCache(configureOptions);

        services.RemoveAll<IOutputCacheStore>();

        services.AddSingleton<IOutputCacheStore, RedisOutputCacheStore>();

        return services;
    }

    private static RedisCacheConfiguration? AddRedisCache(this IServiceCollection services,
        IConfiguration configuration)
    {
        RedisCacheConfiguration? redisCacheConfiguration = configuration
            .GetSection(RedisCacheConfiguration.SectionKey)
            .Get<RedisCacheConfiguration>();

        if (redisCacheConfiguration is null)
            return redisCacheConfiguration;

        services.AddSingleton(redisCacheConfiguration);
        services
            .AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisCacheConfiguration.ConnectionString;
            });

        return redisCacheConfiguration;
    }
}