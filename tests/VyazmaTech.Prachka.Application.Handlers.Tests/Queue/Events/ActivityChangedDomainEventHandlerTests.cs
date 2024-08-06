using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using VyazmaTech.Prachka.Application.Handlers.Core.Queue.Events;
using VyazmaTech.Prachka.Application.Handlers.Tests.Fixtures;
using VyazmaTech.Prachka.Domain.Core.Queues;
using VyazmaTech.Prachka.Domain.Core.Queues.Events;
using VyazmaTech.Prachka.Domain.Kernel;
using VyazmaTech.Prachka.Tests.Tools.FluentBuilders;

namespace VyazmaTech.Prachka.Application.Handlers.Tests.Queue.Events;

public sealed class ActivityChangedDomainEventHandlerTests : TestBase
{
    private readonly ActivityChangedDomainEventHandler _handler;
    private readonly Mock<IDateTimeProvider> _timeProvider;

    public ActivityChangedDomainEventHandlerTests(CoreDatabaseFixture fixture) : base(fixture)
    {
        _timeProvider = new Mock<IDateTimeProvider>();

        _handler = new ActivityChangedDomainEventHandler(
            fixture.Context,
            fixture.Scope.ServiceProvider,
            _timeProvider.Object);
    }

    [Fact]
    public async Task Handle_ShouldActivateQueue_WhenCurrentTimeWithinNewBoundaries()
    {
        // Arrange
        _timeProvider.Setup(x => x.DateNow).Returns(DateTime.UtcNow.AsDateOnly());
        _timeProvider.Setup(x => x.UtcNow).Returns(DateTime.UtcNow);

        var queueId = Guid.NewGuid();
        var queue = Create.Queue
            .WithId(queueId)
            .WithCapacity(1)
            .WithAssignmentDate(_timeProvider.Object.DateNow)
            .WithActivityBoundaries(
                _timeProvider.Object.UtcNow.AddHours(-1).AsTimeOnly(),
                _timeProvider.Object.UtcNow.AddHours(1).AsTimeOnly())
            .Build();

        Context.Queues.Add(queue);
        await Context.SaveChangesAsync();

        // Act
        var @event = new ActivityChangedDomainEvent(queueId, queue.ActivityBoundaries, queue.AssignmentDate);
        await _handler.Handle(@event, default);

        queue = await Context.Queues.FirstAsync(default);

        // Assert
        queue.State.Should().Be(QueueState.Active);
    }

    [Fact]
    public async Task Handle_ShouldPrepareQueue_WhenCurrentTimeLessThanNewActiveFrom()
    {
        // Arrange
        _timeProvider.Setup(x => x.DateNow).Returns(DateTime.UtcNow.AsDateOnly());
        _timeProvider.Setup(x => x.UtcNow).Returns(DateTime.UtcNow);

        var queueId = Guid.NewGuid();
        var queue = Create.Queue
            .WithId(queueId)
            .WithCapacity(1)
            .WithAssignmentDate(_timeProvider.Object.DateNow)
            .WithActivityBoundaries(
                _timeProvider.Object.UtcNow.AddHours(1).AsTimeOnly(),
                _timeProvider.Object.UtcNow.AddHours(2).AsTimeOnly())
            .Build();

        Context.Queues.Add(queue);
        await Context.SaveChangesAsync();

        // Act
        var @event = new ActivityChangedDomainEvent(queueId, queue.ActivityBoundaries, queue.AssignmentDate);
        await _handler.Handle(@event, default);

        queue = await Context.Queues.FirstAsync(default);

        // Assert
        queue.State.Should().Be(QueueState.Prepared);
    }

    [Fact]
    public async Task Handle_ShouldCloseQueue_WhenCurrentTimeGreaterThanNewActiveUntil()
    {
        // Arrange
        _timeProvider.Setup(x => x.DateNow).Returns(DateTime.UtcNow.AsDateOnly());
        _timeProvider.Setup(x => x.UtcNow).Returns(DateTime.UtcNow);

        var queueId = Guid.NewGuid();
        var queue = Create.Queue
            .WithId(queueId)
            .WithCapacity(1)
            .WithAssignmentDate(_timeProvider.Object.DateNow)
            .WithActivityBoundaries(
                _timeProvider.Object.UtcNow.AddHours(-2).AsTimeOnly(),
                _timeProvider.Object.UtcNow.AddHours(-1).AsTimeOnly())
            .Build();

        Context.Queues.Add(queue);
        await Context.SaveChangesAsync();

        // Act
        var @event = new ActivityChangedDomainEvent(queueId, queue.ActivityBoundaries, queue.AssignmentDate);
        await _handler.Handle(@event, default);

        queue = await Context.Queues.FirstAsync(default);

        // Assert
        queue.State.Should().Be(QueueState.Closed);
    }
}