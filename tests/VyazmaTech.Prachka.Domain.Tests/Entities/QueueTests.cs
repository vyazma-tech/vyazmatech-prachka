using FluentAssertions;
using Moq;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
using VyazmaTech.Prachka.Domain.Core.Order;
using VyazmaTech.Prachka.Domain.Core.Queue;
using VyazmaTech.Prachka.Domain.Core.Queue.Events;
using VyazmaTech.Prachka.Domain.Core.ValueObjects;
using VyazmaTech.Prachka.Domain.Kernel;
using Xunit;

namespace VyazmaTech.Prachka.Domain.Tests.Entities;

public class QueueTests
{
    private readonly Mock<IDateTimeProvider> _dateTimeProvider = new();

    [Fact]
    public void CreateQueueCapacity_ShouldThrow_WhenCapacityIsNegative()
    {
        Func<Capacity> action = () => Capacity.Create(-1);

        action.Should().Throw<UserInvalidInputException>();
    }

    [Fact]
    public void CreateQueueDate_ShouldThrow_WhenCreationDateIsInThePast()
    {
        // TODO: переписать
        _dateTimeProvider.Setup(x => x.DateNow).Returns(DateTime.UtcNow.AsDateOnly());

        var registrationDate = new DateTime(
            2023,
            1,
            1,
            1,
            30,
            30);

        Func<QueueDate> action = () => QueueDate.Create(registrationDate.AsDateOnly(), _dateTimeProvider.Object);

        action.Should().Throw<DomainInvalidOperationException>();
    }

    [Fact]
    public void CreateQueueDate_ShouldThrow_WhenCreationDateIsNotNextWeek()
    {
        // TODO: переписать
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

        Func<QueueDate> action = () => QueueDate.Create(registrationDate.AsDateOnly(), _dateTimeProvider.Object);

        action.Should().Throw<DomainInvalidOperationException>();
    }

    [Fact]
    public void Add_ShouldThrow_WhenUserOrderIsAlreadyInQueue()
    {
        var orderId = Guid.NewGuid();

        var order = new OrderEntity(
            orderId,
            Guid.Empty,
            status: OrderStatus.New,
            user: null!,
            creationDateTimeUtc: default);

        var queue = new QueueEntity(
            Guid.Empty,
            1,
            default,
            default!,
            QueueState.Active,
            new HashSet<OrderInfo>(new OrderByIdComparer()) { new(orderId, null!, default, default) });

        Action action = () => queue.Add(order, default);

        action.Should().Throw<DomainInvalidOperationException>();
    }

    [Fact]
    public void Add_ShouldThrow_WhenQueueIsFull()
    {
        var order = new OrderEntity(
            Guid.NewGuid(),
            Guid.Empty,
            null!,
            OrderStatus.New,
            default);

        var queue = new QueueEntity(
            Guid.Empty,
            1,
            default,
            default!,
            QueueState.Active,
            new HashSet<OrderInfo>(new OrderByIdComparer()) { new(Guid.NewGuid(), default!, default, default) });

        Action action = () => queue.Add(order, default);

        action.Should().Throw<DomainInvalidOperationException>();
    }

    [Fact]
    public void Add_ShouldThrow_WhenQueueIsExpired()
    {
        _dateTimeProvider.Setup(x => x.DateNow).Returns(DateTime.UtcNow.AsDateOnly());
        _dateTimeProvider.Setup(x => x.UtcNow).Returns(DateTime.UtcNow);

        var order = new OrderEntity(
            Guid.NewGuid(),
            Guid.Empty,
            null!,
            OrderStatus.New,
            default);

        var queue = new QueueEntity(
            Guid.Empty,
            2,
            _dateTimeProvider.Object.DateNow,
            QueueActivityBoundaries.Create(
                _dateTimeProvider.Object.UtcNow.Subtract(TimeSpan.FromMinutes(1)).AsTimeOnly(),
                _dateTimeProvider.Object.UtcNow.AsTimeOnly()),
            QueueState.Active,
            new HashSet<OrderInfo>(new OrderByIdComparer()) { new(Guid.NewGuid(), default!, default, default) });

        Action action = () => queue.Add(order, _dateTimeProvider.Object.UtcNow);
        action.Should().Throw<DomainInvalidOperationException>();
    }

    [Fact]
    public void Remove_ShouldThrow_WhenUserOrderIsNotInQueue()
    {
        var order = new OrderEntity(
            Guid.NewGuid(),
            Guid.Empty,
            null!,
            OrderStatus.New,
            default);

        var queue = new QueueEntity(
            Guid.Empty,
            1,
            default,
            default!,
            QueueState.Active,
            Array.Empty<OrderInfo>().ToHashSet(new OrderByIdComparer()));

        Action action = () => queue.Remove(order);
        action.Should().Throw<DomainInvalidOperationException>();
    }

    [Fact]
    public void IncreaseCapacity_ShouldNotThrow_WhenNewCapacityIsGreaterThenCurrent()
    {
        var queue = new QueueEntity(
            Guid.Empty,
            1,
            default,
            default!,
            QueueState.Active,
            Array.Empty<OrderInfo>().ToHashSet(new OrderByIdComparer()));

        DateTime modificationDate = DateTime.UtcNow;
        queue.IncreaseCapacity(Capacity.Create(2), modificationDate);

        queue.ModifiedOnUtc.Should().Be(modificationDate);
        queue.Capacity.Should().Be(2);
    }

    [Fact]
    public void TryExpire_ShouldRaiseDomainEvent_WhenQueueExpired()
    {
        _dateTimeProvider.Setup(x => x.DateNow).Returns(DateTime.UtcNow.AsDateOnly());
        _dateTimeProvider.Setup(x => x.UtcNow).Returns(DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(1)));

        var queue = new QueueEntity(
            Guid.Empty,
            10,
            _dateTimeProvider.Object.DateNow,
            QueueActivityBoundaries.Create(
                TimeOnly.FromDateTime(_dateTimeProvider.Object.UtcNow),
                TimeOnly.FromDateTime(_dateTimeProvider.Object.UtcNow.AddSeconds(1))),
            QueueState.Active,
            Array.Empty<OrderInfo>().ToHashSet(new OrderByIdComparer()));

        queue.TryExpire(_dateTimeProvider.Object.UtcNow.AddMinutes(1));

        queue.DomainEvents.Should()
            .ContainSingle()
            .Which.Should()
            .BeOfType<QueueExpiredDomainEvent>();
    }

    [Fact]
    public void TryNotifyAboutAvailablePosition_ShouldRaiseDomainEvent_WhenItExpiredAndNotFullAndMaxCapacityReached()
    {
        var queue = new QueueEntity(
            Guid.Empty,
            10,
            default,
            default!,
            QueueState.Expired,
            Array.Empty<OrderInfo>().ToHashSet(new OrderByIdComparer()),
            true);

        queue.TryNotifyAboutAvailablePosition(default);

        queue.DomainEvents.Should()
            .ContainSingle()
            .Which.Should()
            .BeOfType<PositionAvailableDomainEvent>();
    }
}