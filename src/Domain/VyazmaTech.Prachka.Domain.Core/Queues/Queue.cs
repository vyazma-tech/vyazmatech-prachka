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

    private Queue(
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
        CreationDate = assignmentDate.Value;
        ActivityBoundaries = activityBoundaries;
        State = state;
        ModifiedOnUtc = modifiedOn;
        _orders = [.. orders];
    }

    public static Queue Create(
        Capacity capacity,
        AssignmentDate assignmentDate,
        QueueActivityBoundaries activityBoundaries)
    {
        var queue = new Queue(
            Guid.NewGuid(),
            capacity,
            assignmentDate,
            activityBoundaries,
            QueueState.Prepared,
            []);

        queue.Raise(new QueueCreatedDomainEvent(queue.Id, queue.ActivityBoundaries, queue.AssignmentDate));

        return queue;
    }

    public Capacity Capacity { get; private set; }

    public AssignmentDate AssignmentDate { get; }

    public QueueActivityBoundaries ActivityBoundaries { get; private set; }

    public QueueState State { get; private set; }

    public IReadOnlyCollection<Order> Orders => _orders;

    public DateOnly CreationDate { get; }

    public DateTime? ModifiedOnUtc { get; }

    public void BulkInsert(IReadOnlyCollection<Order> orders)
    {
        if (orders.Count + _orders.Count > Capacity.Value)
            throw new DomainInvalidOperationException(DomainErrors.Queue.WillOverflow);

        if (IsClosed())
            throw new DomainInvalidOperationException(DomainErrors.Queue.Expired);

        Order? existingOrder = _orders.FirstOrDefault(orders.Contains);

        if (existingOrder is not null)
            throw new DomainInvalidOperationException(DomainErrors.Queue.ContainsOrderWithId(existingOrder.Id));

        _orders.AddRange(orders);
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

    public void ChangeActivityBoundaries(QueueActivityBoundaries activityBoundaries)
    {
        if (ActivityBoundaries == activityBoundaries)
            throw new DomainInvalidOperationException(DomainErrors.Queue.InvalidNewActivityBoundaries);

        Raise(new ActivityChangedDomainEvent(Id, AssignmentDate, current: activityBoundaries));
        ActivityBoundaries = activityBoundaries;
    }

    public bool IsClosed()
    {
        return State == QueueState.Closed;
    }

    public void ModifyState(QueueState state)
    {
        if (state == QueueState.Expired)
            Raise(new QueueExpiredDomainEvent(Id));

        State = state;
    }
}