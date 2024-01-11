using Application.Core.Configuration;
using Application.Handlers.User.Queries.UserByQuery;
using Infrastructure.DataAccess.Contexts;
using Infrastructure.DataAccess.Contracts;
using Infrastructure.DataAccess.Repositories;
using Microsoft.Extensions.Options;
using Moq;
using Test.Handlers.Fixtures;
using Xunit;

namespace Test.Handlers.User.Queries;

public class FindUsersTest : TestBase
{
    private readonly UserByQueryQueryHandler _handler;

    public FindUsersTest(CoreDatabaseFixture database) : base(database)
    {
        Mock<IOptions<PaginationConfiguration>> pagination = new ();
        IUserRepository users = new UserRepository(database.Context);
        IOrderRepository orders = new OrderRepository(database.Context);
        IQueueRepository queues = new QueueRepository(database.Context);
        IQueueSubscriptionRepository queueSubscriptions = new QueueSubscriptionRepository(database.Context);
        IOrderSubscriptionRepository orderSubscriptions = new OrderSubscriptionRepository(database.Context);

        var persistenceContext = new PersistenceContext(
            queues,
            orders,
            users,
            orderSubscriptions,
            queueSubscriptions,
            database.Context);

        _handler = new UserByQueryQueryHandler(pagination.Object, persistenceContext);
    }

    [Fact]
    public Task FindUsersByNameFilter_WhenOneUserInserted()
    {
        // TODO: implement
        return Task.CompletedTask;
    }
}