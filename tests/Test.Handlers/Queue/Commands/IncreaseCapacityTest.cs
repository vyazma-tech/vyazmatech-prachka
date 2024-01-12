using Application.Handlers.Queue.Commands.IncreaseQueueCapacity;
using Domain.Common.Result;
using Domain.Core.Queue;
using Domain.Core.ValueObjects;
using FluentAssertions;
using Infrastructure.DataAccess.Contexts;
using Infrastructure.DataAccess.Repositories;
using Infrastructure.Tools;
using Test.Handlers.Fixtures;
using Xunit;

namespace Test.Handlers.Queue.Commands;

public class IncreaseCapacityTest : TestBase
{
    private readonly IncreaseQueueCapacityCommandHandler _handler;
    private readonly PersistenceContext _persistenceContext;

    public IncreaseCapacityTest(CoreDatabaseFixture database) : base(database)
    {
        var dateTimeProvider = new DateTimeProvider();
        var queues = new QueueRepository(database.Context);
        var users = new UserRepository(database.Context);
        var orders = new OrderRepository(database.Context);
        var orderSubscriptions = new OrderSubscriptionRepository(database.Context);
        var queueSubscriptions = new QueueSubscriptionRepository(database.Context);

        _persistenceContext =
            new PersistenceContext(
                queues,
                orders,
                users,
                orderSubscriptions,
                queueSubscriptions,
                database.Context);
        
        _handler = new IncreaseQueueCapacityCommandHandler(
            database.Context,
            dateTimeProvider,
            _persistenceContext);
    }

    [Fact]
    public async Task IncreaseCapacityExistingQueue()
    {
        var queueId = Guid.NewGuid();
        var queue = new QueueEntity(
            queueId,
            Capacity.Create(10).Value,
            QueueDate.Create(DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)), new DateTimeProvider()).Value,
            QueueActivityBoundaries.Create(
                TimeOnly.FromDateTime(DateTime.UtcNow.AddDays(1)),
                TimeOnly.FromDateTime(DateTime.UtcNow.AddDays(1)).AddHours(5)).Value);
        
        int newCapacity = 12;
        
        await Database.ResetAsync();
        _persistenceContext.Queues.InsertRange(new List<QueueEntity> {queue});
        await Database.Context.SaveChangesAsync();

        var incCommand = new IncreaseQueueCapacity.Command(queueId, newCapacity);
        Result<IncreaseQueueCapacity.Response> response = await _handler.Handle(incCommand, CancellationToken.None);

        response.Should().NotBeNull();
        response.IsSuccess.Should().BeTrue();
        response.Value.Should().NotBeNull();
        response.Value.Capacity.Should().Be(newCapacity);
    }
    
    [Fact]
    public async Task IncreaseCapacity_ShouldReturnFailure_WhenNewCapacityLessThanPrevious()
    {
        var queueId = Guid.NewGuid();

        var queue = new QueueEntity(
            queueId,
            Capacity.Create(10).Value,
            QueueDate.Create(DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)), new DateTimeProvider()).Value,
            QueueActivityBoundaries.Create(
                TimeOnly.FromDateTime(DateTime.UtcNow.AddDays(1)),
                TimeOnly.FromDateTime(DateTime.UtcNow.AddDays(1)).AddHours(5)).Value);
        
        await Database.ResetAsync();
        _persistenceContext.Queues.InsertRange(new List<QueueEntity> {queue});
        await Database.Context.SaveChangesAsync();
        
        int newCapacity = 2;

        var incCommand = new IncreaseQueueCapacity.Command(queueId, newCapacity);
        Result<IncreaseQueueCapacity.Response> response = await _handler.Handle(incCommand, CancellationToken.None);

        response.Should().NotBeNull();
        response.IsFaulted.Should().BeTrue();
        response.Error.Should().NotBeNull();
    }
}