using FluentAssertions;
using Moq;
using VyazmaTech.Prachka.Domain.Common.Abstractions;
using VyazmaTech.Prachka.Domain.Common.Errors;
using VyazmaTech.Prachka.Domain.Common.Result;
using VyazmaTech.Prachka.Domain.Core.Order;
using VyazmaTech.Prachka.Domain.Core.Queue;
using VyazmaTech.Prachka.Domain.Core.Queue.Events;
using VyazmaTech.Prachka.Domain.Core.ValueObjects;
using VyazmaTech.Prachka.Domain.Kernel;
using VyazmaTech.Prachka.Infrastructure.Tools;
using Xunit;

namespace VyazmaTech.Prachka.Domain.Tests.Entities;

public class QueueTests
{
    private readonly Mock<IDateTimeProvider> _dateTimeProvider = new ();

    [Fact]
    public void CreateQueueCapacity_ShouldReturnFailureResult_WhenCapacityIsNegative()
    {
        Result<Capacity> creationResult = Capacity.Create(-1);

        creationResult.IsFaulted.Should().BeTrue();
        creationResult.Error.Should().Be(DomainErrors.Capacity.Negative);
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
        _dateTimeProvider.Setup(x => x.SpbDateOnlyNow).Returns(SpbDateTimeProvider.CurrentDate);

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
        creationResult.Error.Should().Be(DomainErrors.QueueDate.InThePast);
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
        creationResult.Error.Should().Be(DomainErrors.QueueDate.NotNextWeek);
    }

    [Fact]
    public void Add_ShouldReturnFailureResult_WhenUserOrderIsAlreadyInQueue()
    {
        var orderId = Guid.NewGuid();

        var order = new OrderEntity(
            orderId,
            queueId: Guid.Empty,
            status: OrderStatus.New,
            userId: Guid.Empty,
            creationDateTimeUtc: default);

        var queue = new QueueEntity(
            Guid.Empty,
            capacity: 1,
            assignmentDate: default,
            activeFrom: default,
            activeUntil: default,
            state: QueueState.Active,
            new HashSet<Guid> { orderId });

        Result<OrderEntity> entranceResult = queue.Add(order, default);

        entranceResult.IsFaulted.Should().BeTrue();
        entranceResult.Error.Should().Be(DomainErrors.Queue.ContainsOrderWithId(order.Id));
    }

    [Fact]
    public void Add_ShouldReturnFailureResult_WhenQueueIsFull()
    {
        var order = new OrderEntity(
            id: Guid.NewGuid(),
            queueId: Guid.Empty,
            userId: Guid.Empty,
            status: OrderStatus.New,
            creationDateTimeUtc: default);

        var queue = new QueueEntity(
            id: Guid.Empty,
            capacity: 1,
            assignmentDate: default,
            activeFrom: default,
            activeUntil: default,
            state: QueueState.Active,
            orderIds: new HashSet<Guid> { Guid.NewGuid() });

        Result<OrderEntity> incomingOrderResult = queue.Add(order, default);

        incomingOrderResult.IsFaulted.Should().BeTrue();
        incomingOrderResult.Error.Should().Be(DomainErrors.Queue.Overfull);
    }

    [Fact]
    public void Add_ShouldReturnFailureResult_WhenQueueIsExpired()
    {
        _dateTimeProvider.Setup(x => x.DateNow).Returns(DateOnly.FromDateTime(DateTime.UtcNow));
        _dateTimeProvider.Setup(x => x.UtcNow).Returns(DateTime.UtcNow);
        _dateTimeProvider.Setup(x => x.SpbDateTimeNow).Returns(new SpbDateTime(DateTime.UtcNow.AddMinutes(2)));

        var order = new OrderEntity(
            id: Guid.NewGuid(),
            queueId: Guid.Empty,
            userId: Guid.Empty,
            status: OrderStatus.New,
            creationDateTimeUtc: default);

        var queue = new QueueEntity(
            id: Guid.Empty,
            capacity: 2,
            assignmentDate: _dateTimeProvider.Object.DateNow,
            activeFrom: TimeOnly.FromDateTime(_dateTimeProvider.Object.UtcNow),
            activeUntil: TimeOnly.FromDateTime(_dateTimeProvider.Object.UtcNow.AddMinutes(1)),
            state: QueueState.Active,
            orderIds: new HashSet<Guid> { Guid.NewGuid() });

        Result<OrderEntity> incomingOrderResult = queue.Add(order, _dateTimeProvider.Object.SpbDateTimeNow);

        incomingOrderResult.IsFaulted.Should().BeTrue();
        incomingOrderResult.Error.Should().Be(DomainErrors.Queue.Expired);
    }

    [Fact]
    public void Remove_ShouldReturnFailureResult_WhenUserOrderIsNotInQueue()
    {
        var order = new OrderEntity(
            id: Guid.NewGuid(),
            queueId: Guid.Empty,
            userId: Guid.Empty,
            status: OrderStatus.New,
            creationDateTimeUtc: default);

        var queue = new QueueEntity(
            id: Guid.Empty,
            capacity: 1,
            assignmentDate: default,
            activeFrom: default,
            activeUntil: default,
            state: QueueState.Active,
            orderIds: Array.Empty<Guid>().ToHashSet());

        Result<OrderEntity> quitResult = queue.Remove(order);

        quitResult.IsFaulted.Should().BeTrue();
        quitResult.Error.Should().Be(DomainErrors.Queue.OrderIsNotInQueue(order.Id));
    }

    [Fact]
    public void IncreaseCapacity_ShouldReturnSuccessResult_WhenNewCapacityIsGreaterThenCurrent()
    {
        var queue = new QueueEntity(
            id: Guid.Empty,
            capacity: 1,
            assignmentDate: default,
            activeFrom: default,
            activeUntil: default,
            state: QueueState.Active,
            orderIds: Array.Empty<Guid>().ToHashSet());

        SpbDateTime modificationDate = SpbDateTimeProvider.CurrentDateTime;
        Result<QueueEntity> increasingResult = queue.IncreaseCapacity(Capacity.Create(2).Value, modificationDate);

        increasingResult.IsSuccess.Should().BeTrue();
        queue.ModifiedOn.Should().Be(modificationDate);
        queue.Capacity.Should().Be(2);
    }

    [Fact]
    public void TryExpire_ShouldRaiseDomainEvent_WhenQueueExpired()
    {
        _dateTimeProvider.Setup(x => x.DateNow).Returns(DateOnly.FromDateTime(DateTime.UtcNow));
        _dateTimeProvider.Setup(x => x.UtcNow).Returns(DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(1)));
        _dateTimeProvider.Setup(x => x.SpbDateOnlyNow).Returns(DateOnly.FromDateTime(DateTime.UtcNow));

        var queue = new QueueEntity(
            id: Guid.Empty,
            capacity: 10,
            assignmentDate: _dateTimeProvider.Object.DateNow,
            activeFrom: TimeOnly.FromDateTime(_dateTimeProvider.Object.UtcNow),
            activeUntil: TimeOnly.FromDateTime(_dateTimeProvider.Object.UtcNow.AddSeconds(1)),
            state: QueueState.Active,
            orderIds: Array.Empty<Guid>().ToHashSet());

        queue.TryExpire(new SpbDateTime(_dateTimeProvider.Object.UtcNow.AddMinutes(1)));

        queue.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<QueueExpiredDomainEvent>();
    }

    [Fact]
    public void TryNotifyAboutAvailablePosition_ShouldRaiseDomainEvent_WhenItExpiredAndNotFullAndMaxCapacityReached()
    {
        var queue = new QueueEntity(
            id: Guid.Empty,
            capacity: 10,
            assignmentDate: default,
            activeFrom: default,
            activeUntil: default,
            state: QueueState.Expired,
            orderIds: Array.Empty<Guid>().ToHashSet(),
            maxCapacityReached: true);

        queue.TryNotifyAboutAvailablePosition(default);

        queue.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<PositionAvailableDomainEvent>();
    }
}