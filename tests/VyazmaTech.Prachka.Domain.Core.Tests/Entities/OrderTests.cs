using FluentAssertions;
using VyazmaTech.Prachka.Domain.Common.Errors;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
using VyazmaTech.Prachka.Domain.Core.Orders;
using VyazmaTech.Prachka.Domain.Core.Orders.Events;
using VyazmaTech.Prachka.Domain.Core.Queues;
using VyazmaTech.Prachka.Tests.Tools.FluentBuilders;
using Xunit;

namespace VyazmaTech.Prachka.Domain.Core.Tests.Entities;

public class OrderTests
{
    [Fact]
    public void MakeReady_ShouldRaiseDomainEvent_WhenOrderIsNotAlreadyReady()
    {
        Order order = Create.Order.WithStatus(OrderStatus.Paid).Build();

        order.MakeReady();

        order.Status.Should().Be(OrderStatus.Ready);
        order.DomainEvents.Should()
            .ContainSingle()
            .Which.Should()
            .BeOfType<OrderReadyDomainEvent>();
    }

    [Fact]
    public void MakePayment_ShouldRaiseDomainEvent_WhenOrderIsNotAlreadyPaid()
    {
        Order order = Create.Order.WithStatus(OrderStatus.New).Build();

        order.MakePayment();

        order.Status.Should().Be(OrderStatus.Paid);
        order.DomainEvents.Should()
            .ContainSingle()
            .Which.Should()
            .BeOfType<OrderPaidDomainEvent>();
    }

    [Fact]
    public void ProlongInto_ShouldThrow_WhenOrderDetachedFromPreviousQueue()
    {
        Queue orderQueue = Create.Queue.Build();
        Queue targetQueue = Create.Queue.Build();

        Order order = Create.Order.WithQueue(orderQueue).Build();

        Action action = () => order.ProlongInto(targetQueue);

        action.Should()
            .Throw<DomainInvalidOperationException>()
            .Which.Error.Should()
            .Be(DomainErrors.Queue.OrderIsNotInQueue(order.Id));
    }

    [Fact]
    public void PrologInto_ShouldThrow_WhenOrderTransferCauseQueueOverflow()
    {
        Queue orderQueue = Create.Queue.WithCapacity(1).Build();
        Queue targetQueue = Create.Queue.WithCapacity(0).Build();

        Order order = Create.Order.WithQueue(orderQueue).Build();
        orderQueue.BulkInsert([order]);

        Action action = () => order.ProlongInto(targetQueue);

        action.Should()
            .Throw<DomainInvalidOperationException>()
            .Which.Error.Should()
            .Be(DomainErrors.Queue.WillOverflow);
    }

    [Fact]
    public void PrologInto_ShouldThrow_WhenOrderTransferIntoClosedQueue()
    {
        Queue orderQueue = Create.Queue.WithCapacity(1).Build();
        Queue targetQueue = Create.Queue
            .WithCapacity(1)
            .WithState(QueueState.Closed)
            .Build();

        Order order = Create.Order.WithQueue(orderQueue).Build();
        orderQueue.BulkInsert([order]);

        Action action = () => order.ProlongInto(targetQueue);

        action.Should()
            .Throw<DomainInvalidOperationException>()
            .Which.Error.Should()
            .Be(DomainErrors.Queue.Expired);
    }

    [Fact]
    public void ProlongInto_ShouldNotThrow_WhenTransferIntoSameQueue()
    {
        Queue orderQueue = Create.Queue.WithCapacity(1).Build();
        Order order = Create.Order.WithQueue(orderQueue).Build();
        orderQueue.BulkInsert([order]);

        Action action = () => order.ProlongInto(orderQueue);

        action.Should()
            .NotThrow<DomainInvalidOperationException>();
    }

    [Fact]
    public void ProlongInto_ShouldProlong_WhenTransferSucceeded()
    {
        Queue orderQueue = Create.Queue.WithCapacity(1).Build();
        Queue targetQueue = Create.Queue.WithCapacity(1).Build();
        Order order = Create.Order.WithQueue(orderQueue).Build();
        orderQueue.BulkInsert([order]);

        order.ProlongInto(targetQueue);

        order.Status.Should().Be(OrderStatus.Prolonged);
        orderQueue.Orders.Should().NotContain(order);
        targetQueue.Orders.Should().Contain(order);
    }
}