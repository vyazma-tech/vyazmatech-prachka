using VyazmaTech.Prachka.Domain.Core.ValueObjects;
using VyazmaTech.Prachka.Domain.Kernel;

namespace VyazmaTech.Prachka.Domain.Core.Queues.Events;

public sealed class QueueCreatedDomainEvent : IDomainEvent
{
    public QueueCreatedDomainEvent(
        Guid queueId,
        QueueActivityBoundaries activityBoundaries,
        AssignmentDate assignmentDate)
    {
        QueueId = queueId;
        ActivityBoundaries = activityBoundaries;
        AssignmentDate = assignmentDate;
    }

    public Guid QueueId { get; }

    public QueueActivityBoundaries ActivityBoundaries { get; }

    public AssignmentDate AssignmentDate { get; }
}