﻿using FluentAssertions;
using VyazmaTech.Prachka.Domain.Common.Errors;
using VyazmaTech.Prachka.Domain.Common.Result;
using VyazmaTech.Prachka.Domain.Core.Order;
using VyazmaTech.Prachka.Domain.Core.Subscription;
using Xunit;

namespace VyazmaTech.Prachka.Domain.Tests.Entities;

public class SubscriptionTests
{
    [Fact]
    public void Subscribe_ShouldReturnSuccessResult_WhenOrderIsNotInSubscription()
    {
        var order = new OrderEntity(
            Guid.Empty,
            Guid.Empty,
            null!,
            OrderStatus.New,
            default);

        var subscription = new OrderSubscriptionEntity(
            Guid.Empty,
            Guid.Empty,
            default,
            Array.Empty<Guid>().ToHashSet());

        Result<OrderEntity> entranceResult = subscription.Subscribe(order);

        entranceResult.IsSuccess.Should().BeTrue();
        subscription.SubscribedOrders.Should().Contain(order.Id);
    }

    [Fact]
    public void Unsubscribe_ShouldReturnFailureResult_WhenUserOrderIsNotInSubscription()
    {
        var order = new OrderEntity(
            Guid.Empty,
            Guid.Empty,
            null!,
            OrderStatus.New,
            default);

        var subscription = new OrderSubscriptionEntity(
            Guid.Empty,
            Guid.Empty,
            default,
            Array.Empty<Guid>().ToHashSet());

        Result<OrderEntity> quitResult = subscription.Unsubscribe(order);

        quitResult.IsFaulted.Should().BeTrue();
        quitResult.Error.Should().Be(DomainErrors.Subscription.OrderIsNotInSubscription(order.Id));
    }
}