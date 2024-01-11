using Application.Core.Common;
using Application.Handlers.Queue.Commands.IncreaseQueueCapacity;
using Application.Handlers.Queue.Queries;
using Domain.Core.Order;
using Domain.Core.Queue;
using Domain.Core.User;
using Domain.Core.ValueObjects;
using FluentAssertions;
using Infrastructure.Tools;
using Test.Core.Domain.Entities.ClassData;
using Test.Handlers.Fixtures;
using Xunit;

namespace Test.Handlers.Queue.Commands;

public class IncreaseCapacityTest : TestBase
{
    private readonly IncreaseQueueCapacityCommandHandler _handler;
    
    public IncreaseCapacityTest(CoreDatabaseFixture database) : base(database)
    {
        var dateTimeProvider = new DateTimeProvider();
        _handler = new IncreaseQueueCapacityCommandHandler(database.Context, dateTimeProvider);
    }

    [Fact]
    public async Task IncreaseCapacityExistingQueue()
    {
        var queue = new QueueEntity(
            Capacity.Create(10).Value,
            QueueDate.Create(DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)), new DateTimeProvider()).Value,
            QueueActivityBoundaries.Create(
                TimeOnly.FromDateTime(DateTime.UtcNow.AddDays(1)),
                TimeOnly.FromDateTime(DateTime.UtcNow.AddDays(1)).AddHours(5)).Value);
        
        int newCapacity = 12;
        
        await Database.ResetAsync();
        Database.Context.Add(queue);
        await Database.Context.SaveChangesAsync();

        Guid queueId = Database.Context.Queues.First()
            .Id;

        var incCommand = new IncreaseQueueCapacityCommand(queueId, newCapacity);
        ResultResponse<QueueResponse> response = await _handler.Handle(incCommand, CancellationToken.None);

        response.Should().NotBeNull();
        response.IsSuccess.Should().BeTrue();
        response.Value.Should().NotBeNull();
        response.Value.Queue.Should().NotBeNull();
        response.Value.Queue.Capacity.Should().Be(newCapacity);
    }
    
    [Fact]
    public async Task IncreaseCapacity_ShouldReturnFailure_WhenNewCapacityLessThanPrevious()
    {
        var queue = new QueueEntity(
            Capacity.Create(10).Value,
            QueueDate.Create(DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)), new DateTimeProvider()).Value,
            QueueActivityBoundaries.Create(
                TimeOnly.FromDateTime(DateTime.UtcNow.AddDays(1)),
                TimeOnly.FromDateTime(DateTime.UtcNow.AddDays(1)).AddHours(5)).Value);
        
        int newCapacity = 2;
        
        await Database.ResetAsync();
        Database.Context.Add(queue);
        await Database.Context.SaveChangesAsync();

        Guid queueId = Database.Context.Queues.First()
            .Id;

        var incCommand = new IncreaseQueueCapacityCommand(queueId, newCapacity);
        ResultResponse<QueueResponse> response = await _handler.Handle(incCommand, CancellationToken.None);

        response.Should().NotBeNull();
        response.IsFaulted.Should().BeTrue();
        response.Value.Should().Be(default!);
    }
}