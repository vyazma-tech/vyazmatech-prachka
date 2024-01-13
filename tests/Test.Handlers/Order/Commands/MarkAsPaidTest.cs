using Application.Handlers.Order.Commands.MarkOrderAsPaid;
using Application.Handlers.Order.Commands.MarkOrderAsReady;
using Domain.Common.Result;
using Domain.Core.Order;
using Domain.Core.Queue;
using Domain.Core.User;
using FluentAssertions;
using Infrastructure.DataAccess.Contexts;
using Infrastructure.DataAccess.Contracts;
using Infrastructure.DataAccess.Models;
using Infrastructure.DataAccess.Repositories;
using Infrastructure.Tools;
using Test.Core.Domain.Entities.ClassData;
using Test.Handlers.Fixtures;
using Xunit;

namespace Test.Handlers.Order.Commands;

public class MarkOrderAsPaidTest : TestBase
{
    private readonly MarkOrderAsPaidCommandHandler _handler;
    private readonly PersistenceContext _repository;

    public MarkOrderAsPaidTest(CoreDatabaseFixture database) : base(database)
    {
        var dateTimeProvider = new DateTimeProvider();
        var queues = new QueueRepository(database.Context);
        var users = new UserRepository(database.Context);
        var orders = new OrderRepository(database.Context);
        var orderSubscriptions = new OrderSubscriptionRepository(database.Context);
        var queueSubscriptions = new QueueSubscriptionRepository(database.Context);

        _repository = new PersistenceContext(
            queues,
            orders,
            users,
            orderSubscriptions,
            queueSubscriptions,
            database.Context);

        _handler = new MarkOrderAsPaidCommandHandler(
            database.Context,
            dateTimeProvider,
            _repository
            );
    }

    [Fact]
    public async Task MarkAsReadyOrder_WhenOrderNotFoundById()
    {
        var orderId = Guid.NewGuid();
        var command = new MarkOrderAsPaid.Command(orderId);

        Result<MarkOrderAsPaid.Response> response = await _handler.Handle(command, CancellationToken.None);

        response.Should().NotBeNull();
        response.Error.Should().NotBeNull();
    }

    [Theory]
    [ClassData(typeof(QueueClassData))]
    public async Task MarkAsReadyOrder_WhenOrderExistAndNotReadyBefore
        (
            QueueEntity queueEntity,
            UserEntity userEntity,
#pragma warning disable xUnit1026
            OrderEntity orderEntity
#pragma warning restore xUnit1026
        )
    {
        _repository.Users.Insert(userEntity);
        _repository.Queues.InsertRange(new[] { queueEntity });

        await Database.Context.SaveChangesAsync();

        OrderModel createdOrder = Database.Context.Orders.First();

        var command = new MarkOrderAsPaid.Command(createdOrder.Id);

        Result<MarkOrderAsPaid.Response> response = await _handler.Handle(command, CancellationToken.None);

        response.Should().NotBeNull();
        response.IsSuccess.Should().BeTrue();
        response.Value.Should().NotBeNull();
        response.Value.Paid.Should().BeTrue();
    }
}