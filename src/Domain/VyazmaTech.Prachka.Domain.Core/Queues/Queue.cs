using System.Collections.ObjectModel;
using VyazmaTech.Prachka.Domain.Common.Abstractions;
using VyazmaTech.Prachka.Domain.Common.Errors;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
using VyazmaTech.Prachka.Domain.Core.Orders;
using VyazmaTech.Prachka.Domain.Core.Queues.Events;
using VyazmaTech.Prachka.Domain.Core.ValueObjects;
using VyazmaTech.Prachka.Domain.Kernel;

#pragma warning disable CS8618

namespace VyazmaTech.Prachka.Domain.Core.Queues;

public sealed class Queue : Entity, IAuditableEntity
{
    private readonly List<Order> _orders;

    private Queue() { }

    public Queue(
        Guid id,
        Capacity capacity,
        AssignmentDate assignmentDate,
        QueueActivityBoundaries activityBoundaries,
        QueueState state,
        Collection<Order> orders,
        DateTime? modifiedOn = null)
        : base(id)
    {
        Capacity = capacity;
        AssignmentDate = assignmentDate;
        ActivityBoundaries = activityBoundaries;
        State = state;
        ModifiedOnUtc = modifiedOn;
        _orders = [.. orders];
    }

    public Capacity Capacity { get; private set; }

    public AssignmentDate AssignmentDate { get; }

    public QueueActivityBoundaries ActivityBoundaries { get; private set; }

    public QueueState State { get; private set; }

    public IReadOnlyCollection<Order> Orders => _orders;

    public DateOnly CreationDate => AssignmentDate.Value;

    public DateTime? ModifiedOnUtc { get; set; }

    public void BulkInsert(IReadOnlyCollection<Order> orders)
    {
        if (orders.Count + _orders.Count > Capacity.Value)
            throw new DomainInvalidOperationException(DomainErrors.Queue.WillOverflow);

        if (IsExpired())
            throw new DomainInvalidOperationException(DomainErrors.Queue.Expired);

        Order? existingOrder = _orders.FirstOrDefault(orders.Contains);

        if (existingOrder is not null)
            throw new DomainInvalidOperationException(DomainErrors.Queue.ContainsOrderWithId(existingOrder.Id));

        _orders.AddRange(orders);

        if (_orders.Count.Equals(Capacity))
            Raise(new QueueMaxCapacityReachedDomainEvent(this));
    }

    public void RemoveFor(Guid userId, int count)
    {
        var userOrders = _orders.Where(x => x.User.Id.Equals(userId)).ToHashSet();

        if (userOrders.Count < count)
            throw new DomainInvalidOperationException(DomainErrors.Queue.NotEnoughOrders(count));

        _orders.RemoveAll(x => userOrders.Contains(x));
    }

    internal void Remove(Order order)
    {
        if (_orders.Contains(order) is false)
            throw new DomainInvalidOperationException(DomainErrors.Queue.OrderIsNotInQueue(order.Id));

        _orders.Remove(order);
    }

    public void IncreaseCapacity(Capacity newCapacity)
    {
        if (newCapacity <= Capacity)
            throw new DomainInvalidOperationException(DomainErrors.Queue.InvalidNewCapacity);

        Capacity = newCapacity;
    }

    public void ChangeActivityBoundaries(QueueActivityBoundaries activityBoundaries, DateTime currentTimeUtc)
    {
        if (ActivityBoundaries == activityBoundaries)
            throw new DomainInvalidOperationException(DomainErrors.Queue.InvalidNewActivityBoundaries);

        if (currentTimeUtc.AsTimeOnly() < activityBoundaries.ActiveFrom)
            ModifyState(QueueState.Prepared);

        if (currentTimeUtc.AsTimeOnly() > activityBoundaries.ActiveUntil)
            ModifyState(QueueState.Closed);

        State = QueueState.Active;
        ActivityBoundaries = activityBoundaries;
    }

    public bool IsExpired()
    {
        return State == QueueState.Expired;
    }

    public void ModifyState(QueueState state)
    {
        if (State == QueueState.Expired)
            Raise(new QueueExpiredDomainEvent(this));

        State = state;
    }
}