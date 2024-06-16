using VyazmaTech.Prachka.Domain.Core.ValueObjects;

namespace VyazmaTech.Prachka.Tests.Tools.FluentBuilders.ValueObjects;

internal sealed class FullnameFluentBuilder : AbstractFluentBuilder<Fullname>
{
    public Fullname WithValue(string fullname)
    {
        WithProperty(x => x.Value, fullname);
        return Build();
    }
}