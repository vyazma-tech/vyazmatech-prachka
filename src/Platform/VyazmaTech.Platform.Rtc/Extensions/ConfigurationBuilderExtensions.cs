using VaultSharp.Extensions.Configuration;
using VyazmaTech.Platform.Exceptions;
using VyazmaTech.Platform.Rtc.Tools;

namespace VyazmaTech.Platform.Rtc.Extensions;

internal static class ConfigurationBuilderExtensions
{
    internal static IConfigurationBuilder AddRtcProvider(
        this IConfigurationBuilder builder,
        IHostEnvironment environment,
        IConfiguration configuration)
    {
        string path = environment.GetBasePath();
        const string uri = "http://vyazmatech-rtc:8200";

        string token = configuration.GetValue<string>("RtcToken")
                       ?? throw new PlatformException(
                           $"RTC token should be configured in {environment.EnvironmentName} environment");

        builder.AddVaultConfiguration(
            () =>
                new VaultOptions(
                    vaultAddress: uri,
                    vaultToken: token,
                    reloadOnChange: true,
                    reloadCheckIntervalSeconds: 5,
                    additionalCharactersForConfigurationPath: ['_']),
            basePath: path,
            mountPoint: "vyazmatech");

        return builder;
    }
}