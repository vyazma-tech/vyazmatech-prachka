using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace VyazmaTech.Prachka.Infrastructure.Tools;

public sealed class IgnoreEventsCollectionResolver : DefaultContractResolver
{
    private readonly HashSet<string> _ignoredProperties;

    public IgnoreEventsCollectionResolver(IReadOnlyCollection<string> ignoredProperties)
    {
        _ignoredProperties = ignoredProperties.ToHashSet();
    }

    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        JsonProperty property = base.CreateProperty(member, memberSerialization);

        if (_ignoredProperties.Contains(property.PropertyName ?? string.Empty))
        {
            property.ShouldSerialize = _ => false;
        }

        return property;
    }
}