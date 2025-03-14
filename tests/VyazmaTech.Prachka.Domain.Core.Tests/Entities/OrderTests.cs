﻿using FluentAssertions;
using VyazmaTech.Prachka.Domain.Common.Errors;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
using VyazmaTech.Prachka.Domain.Core.Orders;
using VyazmaTech.Prachka.Domain.Core.Orders.Events;
using VyazmaTech.Prachka.Domain.Core.Queues;
using VyazmaTech.Prachka.Domain.Core.Users;
using VyazmaTech.Prachka.Tests.Tools.FluentBuilders;
using Xunit;

namespace VyazmaTech.Prachka.Domain.Core.Tests.Entities;

public class OrderTests
{
    [Fact]
    public void MakeReady_ShouldRaiseDomainEvent_WhenOrderIsNotAlreadyReady()
    {
        User user = Create.User.WithFullname("Bobby Shmurda").WithTelegramUsername("@bobster").Build();
        Order order = Create.Order.WithStatus(OrderStatus.Paid).WithUser(user).Build();

        order.MakeReady();

        order.Status.Should().Be(OrderStatus.Ready);
        order.DomainEvents.Should()
            .ContainSingle()
            .Which.Should()
            .BeOfType<OrderReadyDomainEvent>();
    }

    [Fact]
    public void MakeReady_ShouldSetComment_WhenOrderIsNotAlreadyReady()
    {
        User user = Create.User.WithFullname("Bobby Shmurda").WithTelegramUsername("@bobster").Build();
        Order order = Create.Order.WithStatus(OrderStatus.Paid).WithUser(user).Build();

        order.MakeReady();

        order.Status.Should().Be(OrderStatus.Ready);
        order.Comment.Should().Be($"Ваш заказ переведен в статус Готов");
        order.DomainEvents.Should()
            .ContainSingle()
            .Which.Should()
            .BeOfType<OrderReadyDomainEvent>();
    }

    [Fact]
    public void MakePayment_ShouldRaiseDomainEvent_WhenOrderIsNotAlreadyPaid()
    {
        User user = Create.User.WithFullname("Bobby Shmurda").WithTelegramUsername("@bobster").Build();
        Order order = Create.Order.WithStatus(OrderStatus.New).WithUser(user).Build();

        order.MakePayment(0, string.Empty);

        order.Status.Should().Be(OrderStatus.Paid);
        order.DomainEvents.Should()
            .ContainSingle()
            .Which.Should()
            .BeOfType<OrderPaidDomainEvent>();
    }

    [Fact]
    public void MakePayment_ShouldSetPrice_WhenPriceIsValid()
    {
        User user = Create.User.WithFullname("Bobby Shmurda").WithTelegramUsername("@bobster").Build();
        Order order = Create.Order.WithStatus(OrderStatus.New).WithUser(user).Build();

        order.MakePayment(180, string.Empty);

        order.Price.Value.Should().Be(180);
    }

    [Fact]
    public void MakePayment_ShouldThrow_WhenPriceIsInvalid()
    {
        Order order = Create.Order.WithStatus(OrderStatus.New).Build();

        Action action = () => order.MakePayment(-1, string.Empty);

        action.Should()
            .Throw<UserInvalidInputException>()
            .Which.Error.Should()
            .Be(DomainErrors.Price.NegativePrice);
    }

    [Fact]
    public void MakePayment_ShouldSetComment_WhenPriceIsValid()
    {
        User user = Create.User.WithFullname("Bobby Shmurda").WithTelegramUsername("@bobster").Build();
        Order order = Create.Order.WithStatus(OrderStatus.New).WithUser(user).Build();

        order.MakePayment(180, "Hey there! Im using Vyazmatech.Prachka");

        order.Comment.Should().Be("Hey there! Im using Vyazmatech.Prachka");
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

    [Fact]
    public void ProlongInto_ShouldSetComment_WhenTransferSucceeded()
    {
        Queue orderQueue = Create.Queue.WithCapacity(1).Build();
        Queue targetQueue = Create.Queue.WithCapacity(1).Build();
        Order order = Create.Order.WithQueue(orderQueue).Build();
        orderQueue.BulkInsert([order]);

        order.ProlongInto(targetQueue);

        order.Status.Should().Be(OrderStatus.Prolonged);
        orderQueue.Orders.Should().NotContain(order);
        targetQueue.Orders.Should().Contain(order);
        order.Comment.Should()
            .Be(
                "Заказ переведен в другую очередь, потому что у сотрудников прачечной не хватило времени выполнить заказ. Следите за статусом заказа");
    }
}