using Application.Core.Common;
using Application.Handlers.Queue.Commands.ChangeQueueActivityBoundaries;
using Application.Handlers.Queue.Queries;
using Domain.Common.Result;
using Domain.Core.Queue;
using Domain.Core.ValueObjects;
using FluentAssertions;
using Infrastructure.DataAccess.Contexts;
using Infrastructure.DataAccess.Contracts;
using Infrastructure.DataAccess.Repositories;
using Infrastructure.Tools;
using Test.Handlers.Fixtures;
using Xunit;

namespace Test.Handlers.Queue.Commands;

public class ChangeActivityBoundariesTest : TestBase
{
    private readonly ChangeQueueActivityBoundariesCommandHandler _handler;
    private readonly PersistenceContext _persistenceContext;
    
    public ChangeActivityBoundariesTest(CoreDatabaseFixture database) : base(database)
    {
        var dateTimeProvider = new DateTimeProvider();
        IUserRepository users = new UserRepository(database.Context);
        IOrderRepository orders = new OrderRepository(database.Context);
        IQueueRepository queues = new QueueRepository(database.Context);
        IQueueSubscriptionRepository queueSubscriptions = new QueueSubscriptionRepository(database.Context);
        IOrderSubscriptionRepository orderSubscriptions = new OrderSubscriptionRepository(database.Context);

        _persistenceContext = new PersistenceContext(
            queues,
            orders,
            users,
            orderSubscriptions,
            queueSubscriptions,
            database.Context);

        _handler = new ChangeQueueActivityBoundariesCommandHandler(
            database.Context,
            dateTimeProvider,
            _persistenceContext
            );
    }

    [Fact]
    public async Task ChangeBoundaries_ShouldReturnSuccess_WhenPreviousNotTheSame()
    {
        var queueId = Guid.NewGuid();
        var queue = new QueueEntity(
            queueId,
            Capacity.Create(10).Value,
            QueueDate.Create(DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)), new DateTimeProvider()).Value,
            QueueActivityBoundaries.Create(
                TimeOnly.FromDateTime(DateTime.UtcNow.AddDays(1)),
                TimeOnly.FromDateTime(DateTime.UtcNow.AddDays(1)).AddHours(5)).Value);

        var activeFrom = TimeOnly.FromDateTime(DateTime.UtcNow.AddDays(2));
        var activeUntil = TimeOnly.FromDateTime(DateTime.UtcNow.AddDays(3));

        await Database.ResetAsync();
        _persistenceContext.Queues.InsertRange(new List<QueueEntity>{queue});
        await Database.Context.SaveChangesAsync();

        var cmd = new ChangeQueueActivityBoundaries.Command(queueId, activeFrom, activeUntil);

        Result<ChangeQueueActivityBoundaries.Response> response = await _handler.Handle(cmd, CancellationToken.None);

        response.Should().NotBeNull();
        response.IsSuccess.Should().BeTrue();
        response.Value.Should().NotBeNull();
        response.Value.ActiveFrom.Should().Be(activeFrom);
        response.Value.ActiveUntil.Should().Be(activeUntil);
    }
}