using System.Reflection;
using VaultSharp.Extensions.Configuration;
using VyazmaTech.Platform.Rtc.Attributes;

namespace VyazmaTech.Platform.Rtc.Extensions;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddRtcServices(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        if (environment.IsDevelopment() is false)
            services.AddHostedService<VaultChangeWatcher>();

        services.AddRealTimeOptions(configuration);

        return services;
    }

    private static IServiceCollection AddRealTimeOptions(this IServiceCollection services, IConfiguration configuration)
    {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

        IEnumerable<Type> rtcConfigurations = assemblies
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.GetCustomAttribute<RealtimeOptionAttribute>() != null);

        foreach (Type rtcConfiguration in rtcConfigurations)
        {
            RealtimeOptionAttribute? attribute = rtcConfiguration.GetCustomAttribute<RealtimeOptionAttribute>();

            if (attribute is null)
                continue;

            IConfigurationSection section = configuration.GetSection(attribute.Section);
            services.Configure(rtcConfiguration, section);
        }

        return services;
    }

    private static void Configure(this IServiceCollection services, Type configType, IConfigurationSection section)
    {
        MethodInfo? method = typeof(OptionsConfigurationServiceCollectionExtensions)
            .GetMethods(BindingFlags.Static | BindingFlags.Public)
            .FirstOrDefault(m => m.Name == nameof(Configure) && m.IsGenericMethod && m.GetParameters().Length == 2);

        if (method is not null)
        {
            MethodInfo genericMethod = method.MakeGenericMethod(configType);
            genericMethod.Invoke(null, [services, section]);
        }
    }
}