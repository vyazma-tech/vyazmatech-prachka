using Application.Core.Common;
using Application.Handlers.Queue.Commands.IncreaseQueueCapacity;
using Application.Handlers.Queue.Queries;
using Domain.Core.Order;
using Domain.Core.Queue;
using Domain.Core.User;
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

    [Theory]
    [ClassData(typeof(QueueClassData))]
    public async Task IncreaseCapacityExistingQueue(QueueEntity queue, UserEntity user, OrderEntity order)
    {
        int newCapacity = 2;
        
        await Database.ResetAsync();
        Database.Context.Add(queue);
        Database.Context.Add(user);
        Database.Context.Add(order);
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
}