﻿using FluentAssertions;
using Moq;
using VyazmaTech.Prachka.Domain.Common.Errors;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
using VyazmaTech.Prachka.Domain.Core.Orders;
using VyazmaTech.Prachka.Domain.Core.Queues;
using VyazmaTech.Prachka.Domain.Core.Queues.Events;
using VyazmaTech.Prachka.Domain.Core.ValueObjects;
using VyazmaTech.Prachka.Domain.Kernel;
using VyazmaTech.Prachka.Tests.Tools.FluentBuilders;
using Xunit;

namespace VyazmaTech.Prachka.Domain.Core.Tests.Entities;

public class QueueTests
{
    private readonly Mock<IDateTimeProvider> _dateTimeProvider = new();

    [Fact]
    public void BulkInsert_ShouldThrow_WhenTransferCauseOverflow()
    {
        var orderId = Guid.NewGuid();
        Order order = Create.Order.WithId(orderId).Build();
        Queue queue = Create.Queue
            .WithCapacity(1)
            .WithOrders([order])
            .Build();

        Action action = () => queue.BulkInsert([order]);

        action.Should()
            .Throw<DomainInvalidOperationException>()
            .Which.Error.Should()
            .Be(DomainErrors.Queue.WillOverflow);
    }

    [Fact]
    public void BulkInsert_ShouldThrow_WhenQueueClosed()
    {
        var orderId = Guid.NewGuid();
        Order order = Create.Order.WithId(orderId).Build();
        Queue queue = Create.Queue
            .WithCapacity(2)
            .WithState(QueueState.Closed)
            .WithOrders([order])
            .Build();

        Action action = () => queue.BulkInsert([order]);

        action.Should()
            .Throw<DomainInvalidOperationException>()
            .Which.Error.Should()
            .Be(DomainErrors.Queue.Expired);
    }

    [Fact]
    public void BulkInsert_ShouldThrow_WhenUserOrderIsAlreadyInQueue()
    {
        var orderId = Guid.NewGuid();
        Order order = Create.Order.WithId(orderId).Build();
        Queue queue = Create.Queue
            .WithState(QueueState.Active)
            .WithCapacity(2)
            .WithOrders([order])
            .Build();

        Action action = () => queue.BulkInsert([order]);

        action.Should()
            .Throw<DomainInvalidOperationException>()
            .Which.Error.Should()
            .Be(DomainErrors.Queue.ContainsOrderWithId(order.Id));
    }

    [Fact]
    public void BulkInsert_ShouldNotThrow_WhenInsertSucceededAndMaxCapacityReached()
    {
        var orderId = Guid.NewGuid();
        Order order = Create.Order.WithId(orderId).Build();
        Order newOrder = Create.Order.Build();
        Queue queue = Create.Queue
            .WithState(QueueState.Active)
            .WithCapacity(2)
            .WithOrders([order])
            .Build();

        queue.BulkInsert([newOrder]);

        queue.Orders.Should().Contain(order);
    }

    [Fact]
    public void Remove_ShouldThrow_WhenUserOrderIsNotInQueue()
    {
        Order order = Create.Order.Build();
        Queue queue = Create.Queue.Build();

        Action action = () => queue.Remove(order);

        action.Should()
            .Throw<DomainInvalidOperationException>()
            .Which.Error.Should()
            .Be(DomainErrors.Queue.OrderIsNotInQueue(order.Id));
    }

    [Fact]
    public void IncreaseCapacity_ShouldNotThrow_WhenNewCapacityIsGreaterThenCurrent()
    {
        int currentCapacity = 1;
        var newCapacity = Capacity.Create(2);
        Queue queue = Create.Queue.WithCapacity(currentCapacity).Build();

        queue.IncreaseCapacity(newCapacity);

        queue.Capacity.Should().Be(newCapacity);
    }

    [Fact]
    public void ModifyState_ShouldRaiseDomainEvent_WhenQueueExpired()
    {
        _dateTimeProvider.Setup(x => x.DateNow).Returns(DateTime.UtcNow.AsDateOnly());
        _dateTimeProvider.Setup(x => x.UtcNow).Returns(DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(1)));

        Queue queue = Create.Queue
            .WithActivityBoundaries(
                startDate: _dateTimeProvider.Object.UtcNow.AsTimeOnly(),
                endDate: _dateTimeProvider.Object.UtcNow.AddSeconds(1).AsTimeOnly())
            .Build();

        queue.ModifyState(QueueState.Expired);

        queue.State.Should().Be(QueueState.Expired);
        queue.DomainEvents.Should()
            .ContainSingle()
            .Which.Should()
            .BeOfType<QueueExpiredDomainEvent>();
    }

    [Fact]
    public void Create_ShouldRaiseDomainEvent_WhenQueueParametersValid()
    {
        // Arrange
        _dateTimeProvider.Setup(x => x.DateNow).Returns(DateTime.UtcNow.AsDateOnly());
        _dateTimeProvider.Setup(x => x.UtcNow).Returns(DateTime.UtcNow);

        var assignmentDate = AssignmentDate.Create(
            _dateTimeProvider.Object.DateNow.AddDays(1),
            _dateTimeProvider.Object.DateNow);

        var capacity = Capacity.Create(1);

        var activity = QueueActivityBoundaries.Create(
            _dateTimeProvider.Object.UtcNow.AsTimeOnly(),
            _dateTimeProvider.Object.UtcNow.AsTimeOnly().AddHours(1));

        // Act
        var queue = Queue.Create(capacity, assignmentDate, activity);

        // Assert
        queue.DomainEvents.Should()
            .ContainSingle()
            .Which.Should()
            .BeOfType<QueueCreatedDomainEvent>();
    }
}