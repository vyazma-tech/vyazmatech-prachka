using VyazmaTech.Prachka.Presentation.WebAPI.Exceptions;
using VyazmaTech.Prachka.Presentation.WebAPI.Extensions;

namespace VyazmaTech.Prachka.Presentation.WebAPI.Configuration.Secrets;

internal static class SecretConfigurationBuilder
{
    private const string DeploymentSecretSection = "Deployment:SecretId";

    internal static async Task AddConfiguration(
        IWebHostEnvironment environment,
        ConfigurationManager configuration)
    {
        if (environment.IsDevelopment())
            return;

        string secretId = configuration.GetValue<string>(DeploymentSecretSection)
                          ?? throw new StartupException(
                              $"SecretId must be defined for deployment in {environment.EnvironmentName} environment.");

        string token = await SecretTokenProvider.GetToken();
        var secretProvider = new SecretProvider(token);
        SecretEntry[] secrets = await secretProvider.GetEntries(secretId);
        configuration.AddConfigurationFromSecretProvider(secrets);
    }
}