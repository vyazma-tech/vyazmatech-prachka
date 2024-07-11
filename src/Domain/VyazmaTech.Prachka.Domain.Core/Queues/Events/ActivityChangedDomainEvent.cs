using VyazmaTech.Prachka.Domain.Core.ValueObjects;
using VyazmaTech.Prachka.Domain.Kernel;

namespace VyazmaTech.Prachka.Domain.Core.Queues.Events;

public sealed class ActivityChangedDomainEvent : IDomainEvent
{
    public ActivityChangedDomainEvent(Guid queueId, AssignmentDate assignmentDate, QueueActivityBoundaries current)
    {
        QueueId = queueId;
        AssignmentDate = assignmentDate;
        Current = current;
    }

    public Guid QueueId { get; }

    public AssignmentDate AssignmentDate { get; }

    public QueueActivityBoundaries Current { get; }
}