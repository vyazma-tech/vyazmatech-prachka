using VyazmaTech.Prachka.Domain.Core.ValueObjects;

namespace VyazmaTech.Prachka.Tests.Tools.FluentBuilders.ValueObjects;

internal sealed class CapacityFluentBuilder : AbstractFluentBuilder<Capacity>
{
    public Capacity WithValue(int value)
    {
        WithProperty(x => x.Value, value);
        return Build();
    }
}