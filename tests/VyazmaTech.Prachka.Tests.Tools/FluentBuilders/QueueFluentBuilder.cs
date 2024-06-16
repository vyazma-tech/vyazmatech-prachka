using VyazmaTech.Prachka.Domain.Core.Orders;
using VyazmaTech.Prachka.Domain.Core.Queues;
using VyazmaTech.Prachka.Domain.Kernel;
using VyazmaTech.Prachka.Tests.Tools.FluentBuilders.ValueObjects;

namespace VyazmaTech.Prachka.Tests.Tools.FluentBuilders;

internal sealed class QueueFluentBuilder : AbstractFluentBuilder<Queue>
{
    private readonly CapacityFluentBuilder _capacity = new();
    private readonly AssignmentDateFluentBuilder _assignmentDate = new();
    private readonly ActivityBoundariesFluentBuilder _activityBoundaries = new();

    public QueueFluentBuilder WithId(Guid Id)
    {
        WithProperty(x => x.Id, Id);
        return this;
    }

    public QueueFluentBuilder WithCapacity(int capacity)
    {
        WithProperty(x => x.Capacity, _capacity.WithValue(capacity));
        return this;
    }

    public QueueFluentBuilder WithAssignmentDate(DateOnly date)
    {
        WithProperty(x => x.AssignmentDate, _assignmentDate.WithValue(date));
        return this;
    }

    public QueueFluentBuilder WithActivityBoundaries(TimeOnly startDate, TimeOnly endDate)
    {
        WithProperty(x => x.ActivityBoundaries, _activityBoundaries.WithRange(startDate, endDate));
        return this;
    }

    public QueueFluentBuilder WithState(QueueState state)
    {
        WithProperty(x => x.State, state);
        return this;
    }

    public QueueFluentBuilder WithOrders(IReadOnlyCollection<Order> orders)
    {
        WithCollection(x => x.Orders, orders.ToList());
        return this;
    }

    public override Queue Build()
    {
        if (Entity.AssignmentDate is null)
            WithProperty(x => x.AssignmentDate, _assignmentDate.WithValue(DateTime.UtcNow.AsDateOnly()));

        if (Entity.Orders is null)
            WithCollection(x => x.Orders, Array.Empty<Order>().ToList());

        return base.Build();
    }
}