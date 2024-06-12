using FluentAssertions;
using Moq;
using VyazmaTech.Prachka.Application.Contracts.Queues.Commands;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Application.DataAccess.Contracts.Repositories;
using VyazmaTech.Prachka.Application.Handlers.Queue.Commands.ChangeQueueActivityBoundaries;
using VyazmaTech.Prachka.Application.Handlers.Tests.Fixtures;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
using VyazmaTech.Prachka.Domain.Kernel;
using Xunit;

namespace VyazmaTech.Prachka.Application.Handlers.Tests.Queue.Commands;

public class ChangeActivityBoundariesTest : TestBase
{
    private readonly ChangeQueueActivityBoundariesCommandHandler _handler;

    public ChangeActivityBoundariesTest(CoreDatabaseFixture fixture) : base(fixture)
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
}