using VyazmaTech.Prachka.Domain.Common.Abstractions;
using VyazmaTech.Prachka.Domain.Common.Errors;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
using VyazmaTech.Prachka.Domain.Core.Order;
using VyazmaTech.Prachka.Domain.Core.Queue.Events;
using VyazmaTech.Prachka.Domain.Core.ValueObjects;
using VyazmaTech.Prachka.Domain.Kernel;

namespace VyazmaTech.Prachka.Domain.Core.Queue;

public sealed class QueueEntity : Entity, IAuditableEntity
{
    private readonly HashSet<OrderInfo> _orderInfos;

    public QueueEntity(
        Guid id,
        int capacity,
        DateOnly assignmentDate,
        QueueActivityBoundaries activityBoundaries,
        QueueState state,
        HashSet<OrderInfo> orderInfos,
        bool maxCapacityReached = false,
        DateTime? modifiedOn = null)
        : base(id)
    {
        Capacity = capacity;
        AssignmentDate = assignmentDate;
        ActivityBoundaries = activityBoundaries;
        State = state;
        MaxCapacityReached = maxCapacityReached;
        ModifiedOnUtc = modifiedOn;
        _orderInfos = orderInfos;
    }

    public int Capacity { get; private set; }

    public DateOnly AssignmentDate { get; }

    public QueueActivityBoundaries ActivityBoundaries { get; private set; }

    public QueueState State { get; private set; }

    public bool MaxCapacityReached { get; private set; }

    public QueueInfo Info => new(Id, Capacity, AssignmentDate, default, default, State);

    public IReadOnlyCollection<OrderInfo> Orders => _orderInfos;

    public DateOnly CreationDate => AssignmentDate;

    public DateTime? ModifiedOnUtc { get; private set; }

    public void Add(OrderEntity order, DateTime currentTimeUtc)
    {
        if (_orderInfos.Contains(order.Info))
            throw new DomainInvalidOperationException(DomainErrors.Queue.ContainsOrderWithId(order.Id));

        if (_orderInfos.Count.Equals(Capacity))
            throw new DomainInvalidOperationException(DomainErrors.Queue.Overfull);

        if (Expired(currentTimeUtc))
            throw new DomainInvalidOperationException(DomainErrors.Queue.Expired);

        _orderInfos.Add(order.Info);

        // TODO: domain event
        if (_orderInfos.Count.Equals(Capacity) is true)
        {
            MaxCapacityReached = true;
        }
    }

    public void Remove(OrderEntity order)
    {
        if (_orderInfos.Contains(order.Info) is false)
            throw new DomainInvalidOperationException(DomainErrors.Queue.OrderIsNotInQueue(order.Id));

        _orderInfos.Remove(order.Info);
    }

    public void IncreaseCapacity(Capacity newCapacity, DateTime modifiedOnUtc)
    {
        if (newCapacity.Value <= Capacity)
            throw new DomainInvalidOperationException(DomainErrors.Queue.InvalidNewCapacity);

        Capacity = newCapacity.Value;
        ModifiedOnUtc = modifiedOnUtc;
    }

    public void ChangeActivityBoundaries(
        QueueActivityBoundaries activityBoundaries,
        DateTime modifiedOnUtc)
    {
        if (ActivityBoundaries == activityBoundaries)
            throw new DomainInvalidOperationException(DomainErrors.Queue.InvalidNewActivityBoundaries);

        ActivityBoundaries = activityBoundaries;
        ModifiedOnUtc = modifiedOnUtc;
    }

    public bool TryExpire(DateTime currentTimeUtc)
    {
        if (Expired(currentTimeUtc) && State == QueueState.Active)
        {
            Raise(new QueueExpiredDomainEvent(this));
            State = QueueState.Expired;
            ModifiedOnUtc = currentTimeUtc;
            return true;
        }

        return false;
    }

    public bool TryNotifyAboutAvailablePosition(DateTime currentDateTimeUtc)
    {
        if (State == QueueState.Expired && MaxCapacityReached)
        {
            Raise(new PositionAvailableDomainEvent(this));
            ModifiedOnUtc = currentDateTimeUtc;
            MaxCapacityReached = false;
            return true;
        }

        return false;
    }

    public bool TryActivate(DateTime currentDateTimeUtc)
    {
        if (currentDateTimeUtc.AsTimeOnly() >= ActivityBoundaries.ActiveFrom && State == QueueState.Prepared)
        {
            State = QueueState.Active;
            ModifiedOnUtc = currentDateTimeUtc;
            return true;
        }

        return false;
    }

    private bool Expired(DateTime currentDateTimeUtc)
    {
        DateOnly dateNow = currentDateTimeUtc.AsDateOnly();

        return
            (currentDateTimeUtc.AsTimeOnly() >= ActivityBoundaries.ActiveUntil
             && dateNow == CreationDate)
            || dateNow > CreationDate;
    }
}