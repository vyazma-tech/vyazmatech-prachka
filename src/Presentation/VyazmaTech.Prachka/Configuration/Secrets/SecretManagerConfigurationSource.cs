namespace VyazmaTech.Prachka.Presentation.WebAPI.Configuration.Secrets;

internal sealed class SecretManagerConfigurationSource : IConfigurationSource
{
    private readonly IEnumerable<SecretEntry> _entries;

    public SecretManagerConfigurationSource(IEnumerable<SecretEntry> entries)
    {
        _entries = entries;
    }

    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        return new SecretManagerConfigurationProvider(_entries);
    }
}