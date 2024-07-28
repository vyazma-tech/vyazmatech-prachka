namespace VyazmaTech.Prachka.Presentation.WebAPI.Configuration.Secrets;

internal sealed class SecretManagerConfigurationProvider : ConfigurationProvider
{
    public SecretManagerConfigurationProvider(IEnumerable<SecretEntry> secrets)
    {
        var data = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);

        foreach (SecretEntry secret in secrets)
        {
            data[Normalize(secret.Key)] = secret.Value;
        }

        Data = data;
    }

    private static string Normalize(string key)
    {
        return key.Replace("__", ConfigurationPath.KeyDelimiter, StringComparison.Ordinal);
    }
}