﻿using Ardalis.GuardClauses;
using Domain.Common.Abstractions;
using Domain.Common.Errors;
using Domain.Common.Exceptions;
using Domain.Common.Result;
using Domain.Core.Order.Events;
using Domain.Core.Queue;
using Domain.Core.User;

namespace Domain.Core.Order;

public sealed class OrderEntity : Entity, IAuditableEntity
{
    public OrderEntity(UserEntity user, QueueEntity queueEntity, DateTime creationDateUtc)
        : base(Guid.NewGuid())
    {
        Guard.Against.Null(user, nameof(user), "User should not be null in order.");
        Guard.Against.Null(queueEntity, nameof(queueEntity), "Queue should not be null in order.");
        Guard.Against.Null(creationDateUtc, nameof(creationDateUtc), "Creation date should not be null in order.");

        User = user;
        Queue = queueEntity;
        queueEntity.Add(this);
        CreationDate = creationDateUtc;
        Raise(new OrderCreatedDomainEvent(this));
    }

#pragma warning disable CS8618
    private OrderEntity() { }
#pragma warning restore CS8618

    public UserEntity User { get; }
    public QueueEntity Queue { get; }
    public bool Paid { get; private set; }
    public bool Ready { get; private set; }
    public DateTime CreationDate { get; }
    public DateTime? ModifiedOn { get; private set; }

    public Result<OrderEntity> MakePayment(DateTime dateTimeUtc)
    {
        if (Paid)
        {
            var exception = new DomainException(DomainErrors.Order.AlreadyPaid);
            return new Result<OrderEntity>(exception);
        }

        Paid = true;
        ModifiedOn = dateTimeUtc;
        Raise(new OrderPaidDomainEvent(this));

        return this;
    }

    public Result<OrderEntity> MakeReady(DateTime dateTimeUtc)
    {
        if (Ready)
        {
            var exception = new DomainException(DomainErrors.Order.IsReady);
            return new Result<OrderEntity>(exception);
        }

        Ready = true;
        ModifiedOn = dateTimeUtc;
        Raise(new OrderReadyDomainEvent(this));

        return this;
    }

    public OrderEntity Prolong(DateTime dateTimeUtc)
    {
        ModifiedOn = dateTimeUtc;
        Raise(new OrderProlongedDomainEvent(this));

        return this;
    }
}