using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using VyazmaTech.Prachka.Application.Handlers.Core.Queue.Events;
using VyazmaTech.Prachka.Application.Handlers.Tests.Fixtures;
using VyazmaTech.Prachka.Domain.Core.Queues.Events;
using VyazmaTech.Prachka.Domain.Kernel;
using VyazmaTech.Prachka.Infrastructure.Jobs.Jobs;
using VyazmaTech.Prachka.Tests.Tools.FluentBuilders;

namespace VyazmaTech.Prachka.Application.Handlers.Tests.Queue.Events;

public sealed class QueueCreatedDomainEventHandlerTests : TestBase
{
    private readonly QueueCreatedDomainEventHandler _handler;
    private readonly Mock<IDateTimeProvider> _timeProvider;

    public QueueCreatedDomainEventHandlerTests(CoreDatabaseFixture fixture) : base(fixture)
    {
        var scheduler = fixture.Scope.ServiceProvider.GetRequiredService<QueueJobScheduler>();
        var provider = fixture.Scope.ServiceProvider;

        _timeProvider = new Mock<IDateTimeProvider>();

        _handler = new QueueCreatedDomainEventHandler(scheduler, fixture.Context, provider);
    }

    [Fact]
#pragma warning disable CA1506
    public async Task Handle_Should_StoreJobMessages()
#pragma warning restore CA1506
    {
        // Arrange
        _timeProvider.Setup(x => x.DateNow).Returns(DateTime.UtcNow.AsDateOnly());
        _timeProvider.Setup(x => x.UtcNow).Returns(DateTime.UtcNow);

        var queueId = Guid.NewGuid();
        var queue = Create.Queue
            .WithId(queueId)
            .WithCapacity(1)
            .WithAssignmentDate(_timeProvider.Object.DateNow.AddDays(1))
            .WithActivityBoundaries(
                _timeProvider.Object.UtcNow.AsTimeOnly(),
                _timeProvider.Object.UtcNow.AddHours(1).AsTimeOnly())
            .Build();

        FormattableString jobSql = $"""
                                    select id from hangfire.job
                                    """;

        // Act
        var @event = new QueueCreatedDomainEvent(queueId, queue.ActivityBoundaries, queue.AssignmentDate);
        await _handler.Handle(@event, default);

        var messages = await Context.QueueJobMessages.ToListAsync();
        var jobs = await Context.Database.SqlQuery<long>(jobSql).ToListAsync();

        // Assert
        messages.Count.Should().Be(2);
        messages.Zip(jobs)
            .Should()
            .AllSatisfy(x =>
            {
                x.First.QueueId.Should().Be(queueId);
                x.First.JobId.Should().Be(x.Second.ToString());
            });
    }
}