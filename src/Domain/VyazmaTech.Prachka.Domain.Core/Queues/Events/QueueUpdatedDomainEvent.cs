using Newtonsoft.Json;
using VyazmaTech.Prachka.Domain.Core.ValueObjects;
using VyazmaTech.Prachka.Domain.Kernel;

#pragma warning disable CS8618

namespace VyazmaTech.Prachka.Domain.Core.Queues.Events;

/// <summary>
/// Используется в realtime обновлениях на клиенте
/// </summary>
public sealed class QueueUpdatedDomainEvent : IDomainEvent
{
    private QueueUpdatedDomainEvent() { }

    private QueueUpdatedDomainEvent(
        Guid id,
        AssignmentDate assignmentDate,
        QueueActivityBoundaries activityBoundaries,
        QueueState state,
        Capacity capacity,
        int currentCapacity)
    {
        Id = id;
        AssignmentDate = assignmentDate;
        ActivityBoundaries = activityBoundaries;
        State = state;
        Capacity = capacity;
        CurrentCapacity = currentCapacity;
    }

    [JsonProperty]
    public Guid Id { get; private set; }

    [JsonProperty]
    public AssignmentDate AssignmentDate { get; private set; }

    [JsonProperty]
    public QueueActivityBoundaries ActivityBoundaries { get; private set; }

    [JsonProperty]
    public QueueState State { get; private set; }

    [JsonProperty]
    public Capacity Capacity { get; private set; }

    [JsonProperty]
    public int CurrentCapacity { get; private set; }

    public static QueueUpdatedDomainEvent From(Queue queue)
    {
        return new QueueUpdatedDomainEvent(
            queue.Id,
            queue.AssignmentDate,
            queue.ActivityBoundaries,
            queue.State,
            queue.Capacity,
            queue.Orders.Count);
    }
}