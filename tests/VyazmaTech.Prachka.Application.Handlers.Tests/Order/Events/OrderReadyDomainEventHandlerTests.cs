using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using VyazmaTech.Prachka.Application.Handlers.Core.Order.Events;
using VyazmaTech.Prachka.Application.Handlers.Tests.Fixtures;
using VyazmaTech.Prachka.Domain.Core.Orders;
using VyazmaTech.Prachka.Domain.Core.Orders.Events;
using VyazmaTech.Prachka.Domain.Core.Queues;
using VyazmaTech.Prachka.Domain.Kernel;
using VyazmaTech.Prachka.Tests.Tools.FluentBuilders;

namespace VyazmaTech.Prachka.Application.Handlers.Tests.Order.Events;

public sealed class OrderReadyDomainEventHandlerTests : TestBase
{
    private readonly OrderReadyDomainEventHandler _handler;

    public OrderReadyDomainEventHandlerTests(CoreDatabaseFixture fixture) : base(fixture)
    {
        _handler = new OrderReadyDomainEventHandler(fixture.PersistenceContext, fixture.UnitOfWork, default!, fixture.Context);
    }

    [Fact]
    public async Task Handle_ShouldRemoveAllOrdersFromFutureQueues_WhenAnyExists()
    {
        // Arrange
        var user = Create.User
            .WithFullname("Bobby Shmurda")
            .WithTelegramUsername("@bobster")
            .Build();

        var orders = Enumerable.Range(0, 1000)
            .Select(_ =>
                Create.Order
                    .WithUser(user)
                    .WithStatus(OrderStatus.New)
                    .Build());

        var today = DateTime.UtcNow.AsDateOnly();
        var tomorrow = today.AddDays(1);
        var activeFrom = DateTime.UtcNow.AsTimeOnly();
        var activeUntil = activeFrom.AddHours(1);

        var currentOrder = Create.Order.WithUser(user).WithStatus(OrderStatus.Ready).Build();
        var currentQueue = Create.Queue
            .WithAssignmentDate(today)
            .WithActivityBoundaries(activeFrom, activeUntil)
            .WithCapacity(1)
            .WithState(QueueState.Active)
            .WithOrders([currentOrder])
            .Build();

        var futureQueue = Create.Queue
            .WithAssignmentDate(tomorrow)
            .WithActivityBoundaries(activeFrom, activeUntil)
            .WithCapacity(1000)
            .WithState(QueueState.Prepared)
            .WithOrders([..orders])
            .Build();

        PersistenceContext.Queues.InsertRange([currentQueue, futureQueue]);
        await PersistenceContext.SaveChangesAsync(default);

        // Act
        var @event = new OrderReadyDomainEvent(currentOrder.Id, user.Fullname);
        await _handler.Handle(@event, default);

        // Assert
        var allOrders = await Context.Orders.ToListAsync();
        allOrders.Count.Should().Be(1);
        allOrders.First().Should().BeEquivalentTo(currentOrder);
    }
}