using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using VyazmaTech.Prachka.Application.Handlers.Core.Queue.Events;
using VyazmaTech.Prachka.Application.Handlers.Tests.Fixtures;
using VyazmaTech.Prachka.Domain.Core.Queues.Events;
using VyazmaTech.Prachka.Domain.Kernel;
using VyazmaTech.Prachka.Tests.Tools.FluentBuilders;

namespace VyazmaTech.Prachka.Application.Handlers.Tests.Queue.Events;

public sealed class QueueExpiredDomainEventHandlerTests : TestBase
{
    private readonly QueueExpiredDomainEventHandler _handler;
    private readonly Mock<IDateTimeProvider> _timeProvider = new();

    public QueueExpiredDomainEventHandlerTests(CoreDatabaseFixture fixture) : base(fixture)
    {
        var logger = new Mock<ILogger<QueueExpiredDomainEventHandler>>();
        _handler = new QueueExpiredDomainEventHandler(fixture.PersistenceContext, logger.Object);
    }

    [Fact]
    public async Task Handle_ShouldRemoveUnpaidOrder_WhenAny()
    {
        // Arrange
        _timeProvider.Setup(x => x.DateNow).Returns(DateTime.UtcNow.AsDateOnly());
        _timeProvider.Setup(x => x.UtcNow).Returns(DateTime.UtcNow);

        var queueId = Guid.NewGuid();
        var queue = Create.Queue
            .WithId(queueId)
            .WithCapacity(10)
            .WithAssignmentDate(_timeProvider.Object.DateNow.AddDays(1))
            .WithActivityBoundaries(
                _timeProvider.Object.UtcNow.AsTimeOnly(),
                _timeProvider.Object.UtcNow.AddHours(1).AsTimeOnly())
            .Build();

        var user = Create.User
            .WithFullname("bobby shmurda")
            .WithTelegramUsername("@bobster")
            .Build();

        var orders = Enumerable.Range(1, 3)
            .Select(_ => Create.Order
                .WithId(Guid.NewGuid())
                .WithUser(user)
                .Build())
            .ToList();

        queue.BulkInsert(orders);
        PersistenceContext.Queues.InsertRange([queue]);
        await PersistenceContext.SaveChangesAsync(default);

        // Act
        var @event = new QueueExpiredDomainEvent(queueId);
        await _handler.Handle(@event, default);

        queue = await PersistenceContext.Queues.GetByIdAsync(queueId, default);

        // Assert
        queue.Orders.Should().BeEmpty();
    }
}