using FluentAssertions;
using VyazmaTech.Prachka.Application.Contracts.Core.Queues.Commands;
using VyazmaTech.Prachka.Application.Handlers.Core.Queue.Commands.IncreaseQueueCapacity;
using VyazmaTech.Prachka.Application.Handlers.Tests.Fixtures;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
using VyazmaTech.Prachka.Domain.Kernel;
using VyazmaTech.Prachka.Tests.Tools.FluentBuilders;

namespace VyazmaTech.Prachka.Application.Handlers.Tests.Queue.Commands;

public class IncreaseCapacityTests : TestBase
{
    private static readonly VerifySettings Settings;
    private readonly IncreaseQueueCapacityCommandHandler _handler;

    static IncreaseCapacityTests()
    {
        Settings = new VerifySettings();
        Settings.UseDirectory("Contracts");
    }

    public IncreaseCapacityTests(CoreDatabaseFixture fixture) : base(fixture)
    {
        _handler = new IncreaseQueueCapacityCommandHandler(fixture.PersistenceContext);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenQueueIsNotFound()
    {
        var queueId = Guid.NewGuid();
        var command = new IncreaseQueueCapacity.Command(queueId, 2);

        Func<Task<IncreaseQueueCapacity.Response>> action = async () =>
            await _handler.Handle(command, CancellationToken.None);
        await action.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Verify_Contract()
    {
        // Arrange
        var id = Guid.NewGuid();
        Domain.Core.Users.User user = Create.User
            .WithFullname("Bobby Shmurda")
            .WithTelegramUsername("@bobby")
            .Build();

        Domain.Core.Queues.Queue queue = Create.Queue
            .WithId(id)
            .WithAssignmentDate(DateTime.UtcNow.AsDateOnly())
            .WithCapacity(0)
            .WithActivityBoundaries(TimeOnly.MinValue, TimeOnly.MaxValue)
            .Build();

        Domain.Core.Orders.Order order = Create.Order
            .WithQueue(queue)
            .WithUser(user)
            .Build();

        PersistenceContext.Queues.InsertRange([queue]);
        PersistenceContext.Orders.InsertRange([order]);
        PersistenceContext.Users.Insert(user);
        await PersistenceContext.SaveChangesAsync(CancellationToken.None);

        // Act
        var command = new IncreaseQueueCapacity.Command(id, 2);
        IncreaseQueueCapacity.Response response = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await Verify(response, Settings);
    }
}