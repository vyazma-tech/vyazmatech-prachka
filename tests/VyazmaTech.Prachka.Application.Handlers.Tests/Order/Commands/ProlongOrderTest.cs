using FluentAssertions;
using VyazmaTech.Prachka.Application.Contracts.Orders.Commands;
using VyazmaTech.Prachka.Application.Handlers.Order.Commands.ProlongOrder;
using VyazmaTech.Prachka.Application.Handlers.Tests.Fixtures;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
using VyazmaTech.Prachka.Domain.Core.Orders;
using VyazmaTech.Prachka.Domain.Kernel;
using VyazmaTech.Prachka.Tests.Tools.FluentBuilders;
using CancellationToken = System.Threading.CancellationToken;

namespace VyazmaTech.Prachka.Application.Handlers.Tests.Order.Commands;

public class ProlongOrderTest : TestBase
{
    private static readonly VerifySettings Settings;
    private readonly ProlongOrderCommandHandler _handler;

    static ProlongOrderTest()
    {
        Settings = new VerifySettings();
        Settings.UseDirectory("Contracts");
    }

    public ProlongOrderTest(CoreDatabaseFixture fixture) : base(fixture)
    {
        _handler = new ProlongOrderCommandHandler(fixture.PersistenceContext);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenOrderNotFound()
    {
        var orderId = Guid.NewGuid();
        var queueId = Guid.NewGuid();
        var command = new ProlongOrder.Command(orderId, queueId);

        Func<Task<ProlongOrder.Response>> action = async () => await _handler.Handle(command, CancellationToken.None);
        await action.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_Should_RemoveFromPreviousQueue_InsertIntoTargetQueue()
    {
        // Arrange
        var queueId = Guid.NewGuid();
        var targetId = Guid.NewGuid();
        Domain.Core.Queues.Queue queue = Create.Queue
            .WithId(queueId)
            .WithAssignmentDate(DateTime.UtcNow.AsDateOnly())
            .WithCapacity(1)
            .WithActivityBoundaries(TimeOnly.Parse("10:00"), TimeOnly.Parse("12:00"))
            .Build();

        Domain.Core.Queues.Queue target = Create.Queue
            .WithId(targetId)
            .WithAssignmentDate(DateTime.UtcNow.AddDays(1).AsDateOnly())
            .WithCapacity(1)
            .WithActivityBoundaries(TimeOnly.Parse("10:00"), TimeOnly.Parse("12:00"))
            .Build();

        Domain.Core.Users.User user = Create.User
            .WithFullname("Bobby Shmurda")
            .WithTelegramUsername("@bobster")
            .Build();

        Domain.Core.Orders.Order order = Create.Order
            .WithId(Guid.NewGuid())
            .WithUser(user)
            .WithQueue(queue)
            .WithStatus(OrderStatus.New)
            .Build();

        PersistenceContext.Queues.InsertRange([queue, target]);
        PersistenceContext.Users.Insert(user);
        PersistenceContext.Orders.InsertRange([order]);
        await PersistenceContext.SaveChangesAsync(CancellationToken.None);

        var command = new ProlongOrder.Command(order.Id, targetId);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        Domain.Core.Queues.Queue previousQueue =
            await PersistenceContext.Queues.GetByIdAsync(queueId, CancellationToken.None);
        Domain.Core.Queues.Queue currentQueue =
            await PersistenceContext.Queues.GetByIdAsync(targetId, CancellationToken.None);

        order = await PersistenceContext.Orders.GetByIdAsync(order.Id, CancellationToken.None);

        // Assert
        previousQueue.Orders.Should().NotContain(order);
        currentQueue.Orders.Should().Contain(order);
    }

    [Fact]
    public async Task Verify_Contract()
    {
        // Arrange
        var queueId = Guid.NewGuid();
        var targetId = Guid.NewGuid();
        Domain.Core.Queues.Queue queue = Create.Queue
            .WithId(queueId)
            .WithAssignmentDate(DateTime.UtcNow.AsDateOnly())
            .WithCapacity(1)
            .WithActivityBoundaries(TimeOnly.Parse("10:00"), TimeOnly.Parse("12:00"))
            .Build();

        Domain.Core.Queues.Queue target = Create.Queue
            .WithId(targetId)
            .WithAssignmentDate(DateTime.UtcNow.AddDays(1).AsDateOnly())
            .WithCapacity(1)
            .WithActivityBoundaries(TimeOnly.Parse("10:00"), TimeOnly.Parse("12:00"))
            .Build();

        Domain.Core.Users.User user = Create.User
            .WithFullname("Bobby Shmurda")
            .WithTelegramUsername("@bobster")
            .Build();

        Domain.Core.Orders.Order order = Create.Order
            .WithId(Guid.NewGuid())
            .WithUser(user)
            .WithQueue(queue)
            .WithStatus(OrderStatus.New)
            .Build();

        PersistenceContext.Queues.InsertRange([queue, target]);
        PersistenceContext.Users.Insert(user);
        PersistenceContext.Orders.InsertRange([order]);
        await PersistenceContext.SaveChangesAsync(CancellationToken.None);

        var command = new ProlongOrder.Command(order.Id, targetId);

        // Act
        ProlongOrder.Response response = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await Verify(response, Settings);
    }
}