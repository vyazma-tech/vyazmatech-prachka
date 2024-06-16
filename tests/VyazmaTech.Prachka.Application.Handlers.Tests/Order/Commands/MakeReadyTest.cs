using FluentAssertions;
using VyazmaTech.Prachka.Application.Contracts.Orders.Commands;
using VyazmaTech.Prachka.Application.Handlers.Order.Commands.MarkOrderAsReady;
using VyazmaTech.Prachka.Application.Handlers.Tests.Fixtures;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
using VyazmaTech.Prachka.Domain.Core.Orders;
using VyazmaTech.Prachka.Tests.Tools.FluentBuilders;
using CancellationToken = System.Threading.CancellationToken;

namespace VyazmaTech.Prachka.Application.Handlers.Tests.Order.Commands;

public class MakeReadyTest : TestBase
{
    private static readonly VerifySettings Settings;
    private readonly MarkOrderAsReadyCommandHandler _handler;

    static MakeReadyTest()
    {
        Settings = new VerifySettings();
        Settings.UseDirectory("Contracts");
    }

    public MakeReadyTest(CoreDatabaseFixture fixture) : base(fixture)
    {
        _handler = new MarkOrderAsReadyCommandHandler(fixture.PersistenceContext);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenOrderNotFound()
    {
        var orderId = Guid.NewGuid();
        var command = new MarkOrderAsReady.Command(orderId);

        Func<Task<MarkOrderAsReady.Response>> action = async () =>
            await _handler.Handle(command, CancellationToken.None);

        await action.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Verify_Contract()
    {
        // Arrange
        Domain.Core.Queues.Queue queue = Create.Queue
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

        PersistenceContext.Queues.InsertRange([queue]);
        PersistenceContext.Users.Insert(user);
        PersistenceContext.Orders.InsertRange([order]);
        await PersistenceContext.SaveChangesAsync(CancellationToken.None);

        var command = new MarkOrderAsReady.Command(order.Id);

        // Act
        MarkOrderAsReady.Response response = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await Verify(response, Settings);
    }
}