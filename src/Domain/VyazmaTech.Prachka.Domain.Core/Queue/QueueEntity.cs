﻿using VyazmaTech.Prachka.Domain.Common.Abstractions;
using VyazmaTech.Prachka.Domain.Common.Errors;
using VyazmaTech.Prachka.Domain.Common.Result;
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
        TimeOnly activeFrom,
        TimeOnly activeUntil,
        QueueState state,
        HashSet<OrderInfo> orderInfos,
        bool maxCapacityReached = false,
        SpbDateTime? modifiedOn = null)
        : base(id)
    {
        Capacity = capacity;
        AssignmentDate = assignmentDate;
        ActiveFrom = activeFrom;
        ActiveUntil = activeUntil;
        State = state;
        MaxCapacityReached = maxCapacityReached;
        ModifiedOn = modifiedOn;
        _orderInfos = orderInfos;
    }

    public int Capacity { get; private set; }

    public DateOnly AssignmentDate { get; }

    public TimeOnly ActiveFrom { get; private set; }

    public TimeOnly ActiveUntil { get; private set; }

    public QueueState State { get; private set; }

    public bool MaxCapacityReached { get; private set; }

    public QueueInfo Info => new(Id, Capacity, AssignmentDate, ActiveFrom, ActiveUntil, State);

    public IReadOnlyCollection<OrderInfo> Orders => _orderInfos;

    public DateOnly CreationDate => AssignmentDate;

    public SpbDateTime? ModifiedOn { get; private set; }

    public Result<OrderEntity> Add(OrderEntity order, SpbDateTime currentTimeUtc)
    {
        if (_orderInfos.Contains(order.Info))
        {
            return new Result<OrderEntity>(DomainErrors.Queue.ContainsOrderWithId(order.Id));
        }

        if (_orderInfos.Count.Equals(Capacity))
        {
            return new Result<OrderEntity>(DomainErrors.Queue.Overfull);
        }

        if (Expired(currentTimeUtc))
        {
            return new Result<OrderEntity>(DomainErrors.Queue.Expired);
        }

        _orderInfos.Add(order.Info);
        if (_orderInfos.Count.Equals(Capacity) is true)
        {
            MaxCapacityReached = true;
        }

        return order;
    }

    public Result<OrderEntity> Remove(OrderEntity order)
    {
        if (_orderInfos.Contains(order.Info) is false)
        {
            return new Result<OrderEntity>(DomainErrors.Queue.OrderIsNotInQueue(order.Id));
        }

        _orderInfos.Remove(order.Info);

        return order;
    }

    public Result<QueueEntity> IncreaseCapacity(Capacity newCapacity, SpbDateTime modifiedOnUtc)
    {
        if (newCapacity.Value <= Capacity)
        {
            return new Result<QueueEntity>(DomainErrors.Queue.InvalidNewCapacity);
        }

        Capacity = newCapacity.Value;
        ModifiedOn = modifiedOnUtc;

        return this;
    }

    public Result<QueueEntity> ChangeActivityBoundaries(
        QueueActivityBoundaries activityBoundaries,
        SpbDateTime modifiedOnUtc)
    {
        if (activityBoundaries.ActiveFrom == ActiveFrom && activityBoundaries.ActiveUntil == ActiveUntil)
        {
            return new Result<QueueEntity>(DomainErrors.Queue.InvalidNewActivityBoundaries);
        }

        ActiveFrom = activityBoundaries.ActiveFrom;
        ActiveUntil = activityBoundaries.ActiveUntil;
        ModifiedOn = modifiedOnUtc;

        return this;
    }

    public bool TryExpire(SpbDateTime currentTimeUtc)
    {
        if (Expired(currentTimeUtc) && State == QueueState.Active)
        {
            Raise(new QueueExpiredDomainEvent(this));
            State = QueueState.Expired;
            ModifiedOn = currentTimeUtc;
            return true;
        }

        return false;
    }

    public bool TryNotifyAboutAvailablePosition(SpbDateTime currentDateTimeUtc)
    {
        if (State == QueueState.Expired && MaxCapacityReached)
        {
            Raise(new PositionAvailableDomainEvent(this));
            ModifiedOn = currentDateTimeUtc;
            MaxCapacityReached = false;
            return true;
        }

        return false;
    }

    public bool TryActivate(SpbDateTime currentDateTimeUtc)
    {
        if (currentDateTimeUtc.AsTimeOnly() >= ActiveFrom && State == QueueState.Prepared)
        {
            State = QueueState.Active;
            ModifiedOn = currentDateTimeUtc;
            return true;
        }

        return false;
    }

    private bool Expired(SpbDateTime currentDateTimeUtc)
    {
        DateOnly dateNow = currentDateTimeUtc.AsDateOnly();

        return
            (currentDateTimeUtc.AsTimeOnly() >= ActiveUntil
             && dateNow == CreationDate)
            || dateNow > CreationDate;
    }
}