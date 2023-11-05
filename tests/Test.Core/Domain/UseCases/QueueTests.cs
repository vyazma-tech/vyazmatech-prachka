using System.Diagnostics.CodeAnalysis;
using Domain.Common.Errors;
using Domain.Common.Result;
using Domain.Core.Order;
using Domain.Core.Queue;
using Domain.Core.Queue.Events;
using Domain.Core.User;
using Domain.Core.ValueObjects;
using FluentAssertions;
using Infrastructure.Tools;
using Xunit;

namespace Test.Core.Domain.UseCases;

[SuppressMessage("Usage", "xUnit1026:Theory methods should use all of their parameters")]
public class QueueTests
{
    [Theory]
    [MemberData(nameof(QueueWithOneOrderMemberData))]
    public void EnterQueue_ShouldReturnFailureResult_WhenQueueIsFull(
        QueueEntity queue,
        UserEntity user,
        OrderEntity order)
    {
        Result<OrderEntity> incomingOrderResult = OrderEntity.Create(
            user,
            queue,
            DateTime.UtcNow);

        incomingOrderResult.IsFaulted.Should().BeTrue();
        incomingOrderResult.Error.Message.Should().Be(DomainErrors.Queue.Overfull.Message);
    }

    [Theory]
    [MemberData(nameof(QueueWithOneOrderMemberData))]
    public async Task Queue_ShouldRaiseDomainEvent_WhenItExpiredAndNotFullAndMaxCapacityReached(
        QueueEntity queue,
        UserEntity user,
        OrderEntity order)
    {
        queue.Remove(order);
        await Task.Delay(1_000);
        queue.TryExpire();
        queue.ClearDomainEvents();
        queue.NotifyAboutAvailablePosition();

        queue.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<PositionAvailableDomainEvent>();
    }

    public static IEnumerable<object[]> QueueWithOneOrderMemberData()
    {
        var user = new UserEntity(
            TelegramId.Create("1").Value,
            DateTime.UtcNow);

        var queue = new QueueEntity(
            Capacity.Create(1).Value,
            QueueDate.Create(DateTime.UtcNow.AddSeconds(5), new DateTimeProvider()).Value,
            QueueActivityBoundaries.Create(
                TimeOnly.FromDateTime(DateTime.UtcNow),
                TimeOnly.FromDateTime(DateTime.UtcNow.AddSeconds(1))).Value);

        Result<OrderEntity> order = OrderEntity.Create(
            user,
            queue,
            DateTime.UtcNow);

        yield return new object[] { queue, user, order.Value };
    }
}