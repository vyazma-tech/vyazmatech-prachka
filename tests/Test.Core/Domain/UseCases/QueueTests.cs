using Domain.Common.Abstractions;
using Domain.Common.Errors;
using Domain.Common.Result;
using Domain.Core.Order;
using Domain.Core.Queue;
using Domain.Core.Queue.Events;
using Domain.Core.User;
using Domain.Core.ValueObjects;
using FluentAssertions;
using Infrastructure.Tools;
using Moq;
using Xunit;

namespace Test.Core.Domain.UseCases;

public class QueueTests
{
    private readonly Mock<IDateTimeProvider> _dateTimeProvider = new Mock<IDateTimeProvider>();

    [Theory]
    [MemberData(nameof(QueueWithOneOrderMemberData))]
    public void EnterQueue_ShouldReturnFailureResult_WhenQueueIsFull(QueueEntity queue, UserEntity user)
    {
        Result<OrderEntity> incomingOrderResult = OrderEntity.Create(
            user,
            queue,
            DateTime.UtcNow);

        incomingOrderResult.IsFaulted.Should().BeTrue();
        incomingOrderResult.Error.Message.Should().Be(DomainErrors.Queue.Overfull.Message);
    }

    [Fact]
    public async Task Queue_ShouldRaiseDomainEvent_WhenItExpiredAndNotFull()
    {
        _dateTimeProvider.Setup(x => x.UtcNow).Returns(DateTime.UtcNow);
        var queue = new QueueEntity(
            Capacity.Create(10).Value,
            QueueDate.Create(_dateTimeProvider.Object.UtcNow, _dateTimeProvider.Object).Value,
            QueueActivityBoundaries.Create(
                TimeOnly.FromDateTime(_dateTimeProvider.Object.UtcNow),
                TimeOnly.FromDateTime(_dateTimeProvider.Object.UtcNow.AddSeconds(1))).Value);

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
            QueueDate.Create(DateTime.UtcNow.AddDays(1), new DateTimeProvider()).Value,
            QueueActivityBoundaries.Create(
                TimeOnly.FromDateTime(DateTime.UtcNow),
                TimeOnly.FromDateTime(DateTime.UtcNow).AddHours(5)).Value);

        _ = OrderEntity.Create(
            user,
            queue,
            DateTime.UtcNow);

        yield return new object[] { queue, user };
    }
}