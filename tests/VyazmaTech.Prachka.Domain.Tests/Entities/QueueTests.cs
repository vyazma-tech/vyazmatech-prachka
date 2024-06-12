using FluentAssertions;
using Moq;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
using VyazmaTech.Prachka.Domain.Core.Orders;
using VyazmaTech.Prachka.Domain.Core.Queues;
using VyazmaTech.Prachka.Domain.Core.Queues.Events;
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
    public void CreateAssignmentDate_ShouldThrow_WhenAssignmentDateIsInThePast()
    {
        _dateTimeProvider.Setup(x => x.DateNow).Returns(DateTime.UtcNow.AsDateOnly());

        Func<AssignmentDate> action = () => AssignmentDate.Create(
            _dateTimeProvider.Object.DateNow.AddDays(-1),
            _dateTimeProvider.Object.DateNow);

        action.Should().Throw<DomainInvalidOperationException>();
    }

    [Fact]
    public void CreateAssignmentDate_ShouldThrow_WhenAssignmentDateIsNotNextWeek()
    {
        _dateTimeProvider.Setup(x => x.DateNow).Returns(DateTime.UtcNow.AsDateOnly());

        Func<AssignmentDate> action = () => AssignmentDate.Create(
            _dateTimeProvider.Object.DateNow,
            _dateTimeProvider.Object.DateNow.AddDays(AssignmentDate.Week + 1));

        action.Should().Throw<DomainInvalidOperationException>();
    }

    [Fact]
    public void Add_ShouldThrow_WhenUserOrderIsAlreadyInQueue()
    {
        var orderId = Guid.NewGuid();

        var order = new Order(
            orderId,
            default!,
            status: default,
            user: null!,
            creationDateTimeUtc: default);

        var queue = new Queue(
            default,
            default!,
            default!,
            default!,
            default,
            [new(orderId, default!, default!, default, default)]);

        Action action = () => queue.Add(order, default);

        action.Should().Throw<DomainInvalidOperationException>();
    }

    [Fact]
    public void Add_ShouldThrow_WhenQueueIsFull()
    {
        var order = new Order(
            Guid.NewGuid(),
            default!,
            null!,
            default,
            default);

        var queue = new Queue(
            default!,
            Capacity.Create(1),
            default!,
            default!,
            default,
            [new(Guid.NewGuid(), default!, default!, default, default)]);

        Action action = () => queue.Add(order, default);

        action.Should().Throw<DomainInvalidOperationException>();
    }

    [Fact]
    public void Add_ShouldThrow_WhenQueueIsExpired()
    {
        var order = new Order(
            Guid.NewGuid(),
            default!,
            null!,
            default,
            default);

        var queue = new Queue(
            default,
            Capacity.Create(1),
            default!,
            default!,
            QueueState.Expired,
            [new(Guid.NewGuid(), default!, default!, default, default)]);

        Action action = () => queue.Add(order, _dateTimeProvider.Object.UtcNow);
        action.Should().Throw<DomainInvalidOperationException>();
    }

    [Fact]
    public void Remove_ShouldThrow_WhenUserOrderIsNotInQueue()
    {
        var order = new Order(
            Guid.NewGuid(),
            default!,
            null!,
            default,
            default);

        var queue = new Queue(
            default,
            default!,
            default!,
            default!,
            default,
            []);

        Action action = () => queue.Remove(order);
        action.Should().Throw<DomainInvalidOperationException>();
    }

    [Fact]
    public void IncreaseCapacity_ShouldNotThrow_WhenNewCapacityIsGreaterThenCurrent()
    {
        int currentCapacity = 1;
        int newCapacity = 2;

        var queue = new Queue(
            default,
            Capacity.Create(currentCapacity),
            default!,
            default!,
            default,
            []);

        queue.IncreaseCapacity(Capacity.Create(newCapacity));

        queue.Capacity.Should().Be(newCapacity);
    }

    [Fact]
    public void ModifyState_ShouldRaiseDomainEvent_WhenQueueExpired()
    {
        _dateTimeProvider.Setup(x => x.DateNow).Returns(DateTime.UtcNow.AsDateOnly());
        _dateTimeProvider.Setup(x => x.UtcNow).Returns(DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(1)));

        var queue = new Queue(
            default,
            default!,
            default!,
            QueueActivityBoundaries.Create(
                TimeOnly.FromDateTime(_dateTimeProvider.Object.UtcNow),
                TimeOnly.FromDateTime(_dateTimeProvider.Object.UtcNow.AddSeconds(1))),
            default!,
            []);

        queue.ModifyState(QueueState.Expired);

        queue.DomainEvents.Should()
            .ContainSingle()
            .Which.Should()
            .BeOfType<QueueExpiredDomainEvent>();
    }
}