using Application.Handlers.Queue.Commands.IncreaseQueueCapacity;
using Domain.Common.Result;
using Domain.Core.Order;
using Domain.Core.Queue;
using Domain.Core.User;
using FluentAssertions;
using Infrastructure.DataAccess.Contexts;
using Infrastructure.DataAccess.Repositories;
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
        var queues = new QueueRepository(database.Context);
        var users = new UserRepository(database.Context);
        var orders = new OrderRepository(database.Context);
        var orderSubscriptions = new OrderSubscriptionRepository(database.Context);
        var queueSubscriptions = new QueueSubscriptionRepository(database.Context);

        _handler = new IncreaseQueueCapacityCommandHandler(
            database.Context,
            dateTimeProvider,
            new PersistenceContext(
                queues,
                orders,
                users,
                orderSubscriptions,
                queueSubscriptions,
                database.Context));
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

        var incCommand = new IncreaseQueueCapacity.Command(queueId, newCapacity);
        Result<IncreaseQueueCapacity.Response> response = await _handler.Handle(incCommand, CancellationToken.None);

        response.Should().NotBeNull();
        response.IsSuccess.Should().BeTrue();
        response.Value.Should().NotBeNull();
        response.Value.Should().NotBeNull();
        response.Value.Capacity.Should().Be(newCapacity);
    }
}