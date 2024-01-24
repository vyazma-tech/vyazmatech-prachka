using Domain.Common.Errors;
using Domain.Common.Result;
using Domain.Core.Order;
using Domain.Core.Subscription;
using FluentAssertions;
using Xunit;

namespace Test.Core.Domain.Entities;

public class SubscriptionTests
{
    [Fact]
    public void Subscribe_ShouldReturnSuccessResult_WhenOrderIsNotInSubscription()
    {
        var order = new OrderEntity(
            id: Guid.Empty,
            queueId: Guid.Empty,
            userId: Guid.Empty,
            status: OrderStatus.New,
            creationDateTimeUtc: default);

        var subscription = new OrderSubscriptionEntity(
            id: Guid.Empty,
            user: Guid.Empty,
            creationDateUtc: default,
            orderIds: Array.Empty<Guid>().ToHashSet());

        Result<OrderEntity> entranceResult = subscription.Subscribe(order);

        entranceResult.IsSuccess.Should().BeTrue();
        subscription.SubscribedOrders.Should().Contain(order.Id);
    }

    [Fact]
    public void Unsubscribe_ShouldReturnFailureResult_WhenUserOrderIsNotInSubscription()
    {
        var order = new OrderEntity(
            id: Guid.Empty,
            queueId: Guid.Empty,
            userId: Guid.Empty,
            status: OrderStatus.New,
            creationDateTimeUtc: default);

        var subscription = new OrderSubscriptionEntity(
            id: Guid.Empty,
            user: Guid.Empty,
            creationDateUtc: default,
            orderIds: Array.Empty<Guid>().ToHashSet());
        
        Result<OrderEntity> quitResult = subscription.Unsubscribe(order);

        quitResult.IsFaulted.Should().BeTrue();
        quitResult.Error.Should().Be(DomainErrors.Subscription.OrderIsNotInSubscription(order.Id));
    }
}