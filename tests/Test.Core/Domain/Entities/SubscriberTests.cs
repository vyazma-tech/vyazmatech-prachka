using Domain.Common.Errors;
using Domain.Common.Result;
using Domain.Core.Order;
using Domain.Core.Queue;
using Domain.Core.Subscriber;
using Domain.Core.Subscriber.Events;
using Domain.Core.User;
using Domain.Core.ValueObjects;
using FluentAssertions;
using Infrastructure.Tools;
using Xunit;

namespace Test.Core.Domain.Entities;

public class SubscriberTests
{
    [Fact]
    public void CreateSubscriber_ShouldReturnNotNullSubscriber()
    {
        DateTime creationDate = DateTime.UtcNow;
        var user = new UserEntity(
            TelegramId.Create("1").Value,
            creationDate);

        var subscriber = new SubscriberEntity(
            user,
            creationDate);
        
        subscriber.Should().NotBeNull();
        subscriber.Orders.Should().BeEmpty();
        subscriber.CreationDate.Should().Be(creationDate);
        subscriber.ModifiedOn.Should().BeNull();
        subscriber.Queue.Should().BeNull();
    }
    
    [Theory]
    [MemberData(nameof(OrderAndSubscriptionWithUser))]
    public void SubscribeOrder_ShouldReturnSuccessResultAndRaiseDomainEvent_WhenOrderIsNotInSubscription(
        SubscriberEntity subscriber,
        OrderEntity order)
    {
        Result<OrderEntity> entranceResult = subscriber.Subscribe(order);

        entranceResult.IsSuccess.Should().BeTrue();
        subscriber.Orders.Should().Contain(order);
        subscriber.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<UserSubscribedDomainEvent>();
    }
    
    [Theory]
    [MemberData(nameof(OrderAndSubscriptionWithUser))]
    public void UnsubscribeOrder_ShouldReturnFailureResult_WhenUserOrderIsNotInSubscription(
        SubscriberEntity subscriber,
        OrderEntity order)
    {
        Result<OrderEntity> quitResult = subscriber.Unsubscribe(order);

        quitResult.IsFaulted.Should().BeTrue();
        quitResult.Error.Message.Should().Be(DomainErrors.Subscriber.OrderIsNotInSubscription(order.Id).Message);
    }
    
    public static IEnumerable<object[]> OrderAndSubscriptionWithUser()
    {
        var dateTimeProvider = new DateTimeProvider();
        var user = new UserEntity(
            TelegramId.Create("1").Value,
            DateTime.UtcNow);
        
        var queue = new QueueEntity(
            Capacity.Create(10).Value,
            QueueDate.Create(DateTime.UtcNow.AddDays(1), dateTimeProvider).Value);

        var order = new OrderEntity(
            user,
            queue,
            DateTime.UtcNow);

        var subscriber = new SubscriberEntity(
            user,
            DateTime.UtcNow);

        yield return new object[] { subscriber, order };
    }
}