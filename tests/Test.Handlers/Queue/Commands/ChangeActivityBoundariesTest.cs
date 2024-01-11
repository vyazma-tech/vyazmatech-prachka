using Application.Core.Common;
using Application.Handlers.Queue.Commands.ChangeQueueActivityBoundaries;
using Application.Handlers.Queue.Queries;
using Domain.Common.Result;
using Domain.Core.Queue;
using Domain.Core.ValueObjects;
using FluentAssertions;
using Infrastructure.Tools;
using Test.Handlers.Fixtures;
using Xunit;

namespace Test.Handlers.Queue.Commands;

public class ChangeActivityBoundariesTest : TestBase
{
    private readonly ChangeQueueActivityBoundariesCommandHandler _handler;
    
    public ChangeActivityBoundariesTest(CoreDatabaseFixture database) : base(database)
    {
        var dateTimeProvider = new DateTimeProvider();
        _handler = new ChangeQueueActivityBoundariesCommandHandler(database.Context, dateTimeProvider);
    }

    [Fact]
    public async Task ChangeBoundaries_ShouldReturnSuccess_WhenPreviousNotTheSame()
    {
        var queue = new QueueEntity(
            Capacity.Create(10).Value,
            QueueDate.Create(DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)), new DateTimeProvider()).Value,
            QueueActivityBoundaries.Create(
                TimeOnly.FromDateTime(DateTime.UtcNow.AddDays(1)),
                TimeOnly.FromDateTime(DateTime.UtcNow.AddDays(1)).AddHours(5)).Value);

        var activeFrom = TimeOnly.FromDateTime(DateTime.UtcNow.AddDays(2));
        var activeUntil = TimeOnly.FromDateTime(DateTime.UtcNow.AddDays(3));

        await Database.ResetAsync();
        Database.Context.Add(queue);
        await Database.Context.SaveChangesAsync();
        
        Guid queueId = Database.Context.Queues.First()
            .Id;

        var cmd = new ChangeQueueActivityBoundariesCommand(queueId, activeFrom, activeUntil);

        Result<QueueResponse> response = await _handler.Handle(cmd, CancellationToken.None);

        response.Should().NotBeNull();
        response.IsSuccess.Should().BeTrue();
        response.Value.Should().NotBeNull();
        response.Value.Queue.Should().NotBeNull();
        response.Value.Queue.ActiveFrom.Should().Be(activeFrom);
        response.Value.Queue.ActiveFrom.Should().Be(activeUntil);
    }
}