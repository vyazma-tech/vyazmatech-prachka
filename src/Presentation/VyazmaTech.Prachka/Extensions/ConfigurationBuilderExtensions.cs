using VyazmaTech.Prachka.Presentation.WebAPI.Configuration.Secrets;

namespace VyazmaTech.Prachka.Presentation.WebAPI.Extensions;

internal static class ConfigurationBuilderExtensions
{
    internal static IConfigurationBuilder AddConfigurationFromSecretProvider(
        this IConfigurationBuilder builder,
        SecretEntry[] secrets)
    {
        SecretEntry[] entries = secrets ?? [];
        return builder.Add(new SecretManagerConfigurationSource(entries));
    }
}