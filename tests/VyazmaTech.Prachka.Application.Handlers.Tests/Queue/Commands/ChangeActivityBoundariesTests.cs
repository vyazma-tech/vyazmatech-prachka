using FluentAssertions;
using Moq;
using VyazmaTech.Prachka.Application.Contracts.Queues.Commands;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Application.DataAccess.Contracts.Repositories;
using VyazmaTech.Prachka.Application.Handlers.Queue.Commands.ChangeQueueActivityBoundaries;
using VyazmaTech.Prachka.Application.Handlers.Tests.Fixtures;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
using VyazmaTech.Prachka.Domain.Kernel;
using VyazmaTech.Prachka.Tests.Tools.FluentBuilders;

namespace VyazmaTech.Prachka.Application.Handlers.Tests.Queue.Commands;

public class ChangeActivityBoundariesTests : TestBase
{
    private static readonly VerifySettings Settings;
    private readonly ChangeQueueActivityBoundariesCommandHandler _handler;

    static ChangeActivityBoundariesTests()
    {
        Settings = new VerifySettings();
        Settings.UseDirectory("Contracts");
    }

    public ChangeActivityBoundariesTests(CoreDatabaseFixture fixture) : base(fixture)
    {
        var dateTimeProvider = new Mock<IDateTimeProvider>();
        var queues = new Mock<IQueueRepository>();
        var persistenceContext = new Mock<IPersistenceContext>();
        persistenceContext.Setup(x => x.Queues).Returns(queues.Object);

        _handler = new ChangeQueueActivityBoundariesCommandHandler(
            dateTimeProvider.Object,
            fixture.PersistenceContext);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenQueueIsNotFound()
    {
        var queueId = Guid.NewGuid();
        var command = new ChangeQueueActivityBoundaries.Command(queueId, TimeOnly.MinValue, TimeOnly.MaxValue);

        Func<Task<ChangeQueueActivityBoundaries.Response>> action = async () =>
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

        var from = TimeOnly.Parse("10:00");
        var to = TimeOnly.Parse("18:00");

        // Act
        var command = new ChangeQueueActivityBoundaries.Command(id, from, to);
        ChangeQueueActivityBoundaries.Response response = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await Verify(response, Settings);
    }
}