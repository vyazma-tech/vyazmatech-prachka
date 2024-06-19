using VyazmaTech.Prachka.Domain.Core.ValueObjects;

namespace VyazmaTech.Prachka.Tests.Tools.FluentBuilders.ValueObjects;

internal sealed class AssignmentDateFluentBuilder : AbstractFluentBuilder<AssignmentDate>
{
    public AssignmentDate WithValue(DateOnly date)
    {
        WithProperty(x => x.Value, date);
        return Build();
    }
}