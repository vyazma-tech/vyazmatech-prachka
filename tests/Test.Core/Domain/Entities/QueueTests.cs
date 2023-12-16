using System.Diagnostics.CodeAnalysis;
using Domain.Common.Abstractions;
using Domain.Common.Errors;
using Domain.Common.Result;
using Domain.Core.Order;
using Domain.Core.Queue;
using Domain.Core.Queue.Events;
using Domain.Core.User;
using Domain.Core.ValueObjects;
using Domain.Kernel;
using FluentAssertions;
using Infrastructure.Tools;
using Moq;
using Test.Core.Domain.Entities.ClassData;
using Xunit;

namespace Test.Core.Domain.Entities;

[SuppressMessage("Usage", "xUnit1026:Theory methods should use all of their parameters")]
public class QueueTests
{
    private readonly Mock<IDateTimeProvider> _dateTimeProvider = new ();

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
            2023,
            1,
            2,
            1,
            30,
            0);
        _dateTimeProvider.Setup(x => x.DateNow).Returns(DateOnly.FromDateTime(dateTime));

        var registrationDate = new DateTime(
            2023,
            1,
            1,
            1,
            30,
            30);
        Result<QueueDate> creationResult =
            QueueDate.Create(DateOnly.FromDateTime(registrationDate), _dateTimeProvider.Object);

        creationResult.IsFaulted.Should().BeTrue();
        creationResult.Error.Message.Should().Be(DomainErrors.QueueDate.InThePast.Message);
    }

    [Fact]
    public void CreateQueueDate_ShouldReturnFailureResult_WhenCreationDateIsNotNextWeek()
    {
        var dateTime = new DateTime(
            2023,
            1,
            1,
            1,
            30,
            0);
        _dateTimeProvider.Setup(x => x.DateNow).Returns(DateOnly.FromDateTime(dateTime));

        var registrationDate = new DateTime(
            2023,
            1,
            9,
            1,
            30,
            0);
        Result<QueueDate> creationResult =
            QueueDate.Create(DateOnly.FromDateTime(registrationDate), _dateTimeProvider.Object);

        creationResult.IsFaulted.Should().BeTrue();
        creationResult.Error.Message.Should().Be(DomainErrors.QueueDate.NotNextWeek.Message);
    }

    [Fact]
    public void CreateQueue_Should_ReturnNotNullQueue()
    {
        _dateTimeProvider.Setup(x => x.DateNow).Returns(DateOnly.FromDateTime(DateTime.UtcNow));

        DateTime creationDate = DateTime.UtcNow;
        var queue = new QueueEntity(
            Capacity.Create(10).Value,
            QueueDate.Create(DateOnly.FromDateTime(creationDate), _dateTimeProvider.Object).Value,
            QueueActivityBoundaries.Create(
                TimeOnly.FromDateTime(creationDate),
                TimeOnly.FromDateTime(creationDate).AddHours(5)).Value);

        queue.Should().NotBeNull();
        queue.Capacity.Value.Should().Be(10);
        queue.Items.Should().BeEmpty();
        queue.CreationDate.Should().Be(DateOnly.FromDateTime(creationDate));
        queue.ModifiedOn.Should().BeNull();
    }

    [Theory]
    [ClassData(typeof(QueueClassData))]
    public void EnterQueue_ShouldReturnFailureResult_WhenUserOrderIsAlreadyInQueue(
        QueueEntity queue,
        UserEntity user,
        OrderEntity order)
    {
        Result<OrderEntity> entranceResult = queue.Add(order);

        entranceResult.IsFaulted.Should().BeTrue();
        entranceResult.Error.Message.Should().Be(DomainErrors.Queue.ContainsOrderWithId(order.Id).Message);
    }

    [Theory]
    [ClassData(typeof(QueueClassData))]
    public void QuitQueue_ShouldReturnFailureResult_WhenUserOrderIsNotInQueue(
        QueueEntity queue,
        UserEntity user,
        OrderEntity order)
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
            QueueDate.Create(DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)), new DateTimeProvider()).Value,
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
        _dateTimeProvider.Setup(x => x.DateNow).Returns(DateOnly.FromDateTime(DateTime.UtcNow));
        var queue = new QueueEntity(
            Capacity.Create(10).Value,
            QueueDate.Create(_dateTimeProvider.Object.DateNow, _dateTimeProvider.Object).Value,
            QueueActivityBoundaries.Create(
                TimeOnly.FromDateTime(_dateTimeProvider.Object.UtcNow),
                TimeOnly.FromDateTime(_dateTimeProvider.Object.UtcNow.AddSeconds(1))).Value);

        await Task.Delay(1_000);
        queue.TryExpire();

        queue.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<QueueExpiredDomainEvent>();
    }

    [Theory]
    [ClassData(typeof(QueueClassData))]
    public void EnterQueue_ShouldReturnFailureResult_WhenQueueIsFull(
        QueueEntity queue,
        UserEntity user,
        OrderEntity order)
    {
        Result<OrderEntity> incomingOrderResult = OrderEntity.Create(
            user,
            queue,
            DateOnly.FromDateTime(DateTime.UtcNow));

        incomingOrderResult.IsFaulted.Should().BeTrue();
        incomingOrderResult.Error.Message.Should().Be(DomainErrors.Queue.Overfull.Message);
    }

    [Theory]
    [ClassData(typeof(QueueClassData))]
    public async Task Queue_ShouldRaiseDomainEvent_WhenItExpiredAndNotFullAndMaxCapacityReached(
        QueueEntity queue,
        UserEntity user,
        OrderEntity order)
    {
        queue.Remove(order);
        await Task.Delay(1_000);
        queue.TryExpire();
        queue.ClearDomainEvents();
        queue.TryNotifyAboutAvailablePosition();

        queue.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<PositionAvailableDomainEvent>();
    }
}