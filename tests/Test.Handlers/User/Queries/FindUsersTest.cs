using Application.Core.Common;
using Application.Core.Configuration;
using Application.Handlers.User.Queries.UserByQuery;
using Domain.Core.User;
using FluentAssertions;
using Infrastructure.DataAccess.Contexts;
using Infrastructure.DataAccess.Contracts;
using Infrastructure.DataAccess.Repositories;
using Microsoft.Extensions.Options;
using Test.Core.Domain.Entities.ClassData;
using Test.Handlers.Fixtures;
using Xunit;

namespace Test.Handlers.User.Queries;

public class FindUsersTest : TestBase
{
    private readonly UserByQueryQueryHandler _handler;
    private readonly PersistenceContext _persistenceContext;

    public FindUsersTest(CoreDatabaseFixture database) : base(database)
    {
        var paginationConfiguration = new PaginationConfiguration
        {
            RecordsPerPage = 1
        };
        IOptions<PaginationConfiguration> pagination = Options.Create(paginationConfiguration);
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

        _handler = new UserByQueryQueryHandler(pagination, _persistenceContext);
    }

    [Fact]
    public async Task FindUsersByNameFilter_WhenOneUserInserted()
    {
        // TODO: implement

        UserEntity user = UserClassData.Create();

        await Database.ResetAsync();
        _persistenceContext.Users.Insert(user);
        await Database.Context.SaveChangesAsync();
        
        var cmd = new UserByQueryQuery.Query(
                user.TelegramId.Value,
                null,
                null,
                1
            );

        PagedResponse<UserByQueryQuery.Response> response = await _handler.Handle(cmd, CancellationToken.None);
        UserByQueryQuery.Response first = response.Bunch.First();

        response.Bunch.Should().NotBeNull();
        response.Bunch.Count.Should().Be(1);
        first.Should().Be(user);
    }
}