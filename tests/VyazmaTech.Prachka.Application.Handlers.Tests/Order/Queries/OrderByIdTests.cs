using VyazmaTech.Prachka.Application.Contracts.Core.Orders.Queries;
using VyazmaTech.Prachka.Application.Handlers.Core.Order.Queries;
using VyazmaTech.Prachka.Application.Handlers.Tests.Fixtures;
using VyazmaTech.Prachka.Domain.Kernel;
using VyazmaTech.Prachka.Tests.Tools.FluentBuilders;

namespace VyazmaTech.Prachka.Application.Handlers.Tests.Order.Queries;

public sealed class OrderByIdTests : TestBase
{
    private static readonly VerifySettings Settings;
    private readonly OrderByIdQueryHandler _handler;

    static OrderByIdTests()
    {
        Settings = new VerifySettings();
        Settings.UseDirectory("Contracts");
    }

    public OrderByIdTests(CoreDatabaseFixture fixture) : base(fixture)
    {
        _handler = new OrderByIdQueryHandler(fixture.PersistenceContext);
    }

    [Fact]
    public async Task Verify_Contract()
    {
        // Arrange
        var user = Create.User
            .WithFullname("Bobby Shmurda")
            .WithTelegramUsername("@bobster")
            .Build();

        var queue = Create.Queue
            .WithCapacity(1)
            .WithAssignmentDate(DateTime.UtcNow.AsDateOnly())
            .WithActivityBoundaries(TimeOnly.MinValue, TimeOnly.MaxValue)
            .Build();

        var order = Create.Order
            .WithId(Guid.NewGuid())
            .WithQueue(queue)
            .WithUser(user)
            .Build();

        PersistenceContext.Users.Insert(user);
        PersistenceContext.Queues.InsertRange([queue]);
        PersistenceContext.Orders.InsertRange([order]);
        await PersistenceContext.SaveChangesAsync(CancellationToken.None);

        // Act
        var query = new OrderById.Query(order.Id);
        var response = await _handler.Handle(query, CancellationToken.None);

        // Assert
        await Verify(response, Settings);
    }
}