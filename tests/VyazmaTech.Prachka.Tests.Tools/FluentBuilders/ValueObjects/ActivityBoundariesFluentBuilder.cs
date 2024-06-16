using VyazmaTech.Prachka.Domain.Core.ValueObjects;

namespace VyazmaTech.Prachka.Tests.Tools.FluentBuilders.ValueObjects;

internal sealed class ActivityBoundariesFluentBuilder : AbstractFluentBuilder<QueueActivityBoundaries>
{
    public QueueActivityBoundaries WithRange(TimeOnly from, TimeOnly to)
    {
        WithProperty(x => x.ActiveFrom, from);
        WithProperty(x => x.ActiveUntil, to);
        return Build();
    }
}