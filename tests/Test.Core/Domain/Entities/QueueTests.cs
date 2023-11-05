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

namespace Test.Core.Domain.Entities;

public class QueueTests
{
    private readonly Mock<IDateTimeProvider> _dateTimeProvider = new Mock<IDateTimeProvider>();

    [Fact]
    public void CreateQueueCapacity_ShouldReturnFailureResult_WhenCapacityIsNegative()
    {
        Result<Capacity> creationResult = Capacity.Create(-1);

        creationResult.IsFaulted.Should().BeTrue();
        creationResult.Error.Message.Should().Be(DomainErrors.Capacity.Negative.Message);
    }

    [Fact]
    public void CreateQueueDate_ShouldReturnFailureResult_WhenCreationDateIsInThePast()
    {
        var dateTime = new DateTime(
            year: 2023,
            month: 1,
            day: 1,
            hour: 1,
            minute: 30,
            second: 0);
        _dateTimeProvider.Setup(x => x.UtcNow).Returns(dateTime);

        var registrationDate = new DateTime(
            year: 2023,
            month: 1,
            day: 1,
            hour: 1,
            minute: 29,
            second: 59);
        Result<QueueDate> creationResult = QueueDate.Create(registrationDate, _dateTimeProvider.Object);

        creationResult.IsFaulted.Should().BeTrue();
        creationResult.Error.Message.Should().Be(DomainErrors.QueueDate.InThePast.Message);
    }

    [Fact]
    public void CreateQueueDate_ShouldReturnFailureResult_WhenCreationDateIsNotNextWeek()
    {
        var dateTime = new DateTime(
            year: 2023,
            month: 1,
            day: 1,
            hour: 1,
            minute: 30,
            second: 0);
        _dateTimeProvider.Setup(x => x.UtcNow).Returns(dateTime);

        var registrationDate = new DateTime(
            year: 2023,
            month: 1,
            day: 9,
            hour: 1,
            minute: 30,
            second: 0);
        Result<QueueDate> creationResult = QueueDate.Create(registrationDate, _dateTimeProvider.Object);

        creationResult.IsFaulted.Should().BeTrue();
        creationResult.Error.Message.Should().Be(DomainErrors.QueueDate.NotNextWeek.Message);
    }

    [Fact]
    public void CreateQueue_ShouldReturnNotNullQueue()
    {
        _dateTimeProvider.Setup(x => x.UtcNow).Returns(DateTime.UtcNow);

        DateTime creationDate = DateTime.UtcNow;
        var queue = new QueueEntity(
            Capacity.Create(10).Value,
            QueueDate.Create(creationDate, _dateTimeProvider.Object).Value,
            QueueActivityBoundaries.Create(
                TimeOnly.FromDateTime(creationDate),
                TimeOnly.FromDateTime(creationDate).AddHours(5)).Value);

        queue.Should().NotBeNull();
        queue.Capacity.Value.Should().Be(10);
        queue.Items.Should().BeEmpty();
        queue.CreationDate.Should().Be(creationDate);
        queue.ModifiedOn.Should().BeNull();
    }

    [Theory]
    [MemberData(nameof(UserWithOrderInQueueMemberData))]
    public void EnterQueue_ShouldReturnFailureResult_WhenUserOrderIsAlreadyInQueue(QueueEntity queue, OrderEntity order)
    {
        Result<OrderEntity> entranceResult = queue.Add(order);

        entranceResult.IsFaulted.Should().BeTrue();
        entranceResult.Error.Message.Should().Be(DomainErrors.Queue.ContainsOrderWithId(order.Id).Message);
    }

    [Theory]
    [MemberData(nameof(UserWithOrderInQueueMemberData))]
    public void QuitQueue_ShouldReturnFailureResult_WhenUserOrderIsNotInQueue(QueueEntity queue, OrderEntity order)
    {
        _ = queue.Remove(order);

        Result<OrderEntity> quitResult = queue.Remove(order);

        quitResult.IsFaulted.Should().BeTrue();
        quitResult.Error.Message.Should().Be(DomainErrors.Queue.OrderIsNotInQueue(order.Id).Message);
    }

    [Fact]
    public void IncreaseQueueCapacity_ShouldReturnSuccessResult_WhenNewCapacityIsGreaterThenCurrent()
    {
        var queue = new QueueEntity(
            Capacity.Create(10).Value,
            QueueDate.Create(DateTime.UtcNow.AddDays(1), new DateTimeProvider()).Value,
            QueueActivityBoundaries.Create(
                TimeOnly.FromDateTime(DateTime.UtcNow.AddDays(1)),
                TimeOnly.FromDateTime(DateTime.UtcNow.AddDays(1)).AddHours(5)).Value);

        DateTime modificationDate = DateTime.UtcNow;
        Result<QueueEntity> increasingResult = queue.IncreaseCapacity(Capacity.Create(11).Value, modificationDate);

        increasingResult.IsSuccess.Should().BeTrue();
        queue.ModifiedOn.Should().Be(modificationDate);
        queue.Capacity.Value.Should().Be(11);
    }

    [Fact]
    public async Task Queue_ShouldRaiseDomainEvent_WhenItExpired()
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

        queue.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<QueueExpiredDomainEvent>();
    }

    public static IEnumerable<object[]> UserWithOrderInQueueMemberData()
    {
        var dateTimeProvider = new DateTimeProvider();
        var user = new UserEntity(
            TelegramId.Create("1").Value,
            DateTime.UtcNow);

        var queue = new QueueEntity(
            Capacity.Create(10).Value,
            QueueDate.Create(DateTime.UtcNow.AddDays(1), dateTimeProvider).Value,
            QueueActivityBoundaries.Create(
                TimeOnly.FromDateTime(DateTime.UtcNow.AddDays(1)),
                TimeOnly.FromDateTime(DateTime.UtcNow.AddDays(1)).AddHours(5)).Value);

        Result<OrderEntity> order = OrderEntity.Create(
            user,
            queue,
            DateTime.UtcNow);

        yield return new object[] { queue, order.Value };
    }
}