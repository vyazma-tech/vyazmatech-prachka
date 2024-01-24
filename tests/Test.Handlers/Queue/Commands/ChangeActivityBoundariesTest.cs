using Application.Core.Contracts.Queues.Commands;
using Application.DataAccess.Contracts;
using Application.DataAccess.Contracts.Repositories;
using Application.Handlers.Queue.Commands.ChangeQueueActivityBoundaries;
using Domain.Common.Errors;
using Domain.Common.Result;
using Domain.Core.Queue;
using Domain.Kernel;
using FluentAssertions;
using Moq;
using Test.Handlers.Fixtures;
using Xunit;

namespace Test.Handlers.Queue.Commands;

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
    public async Task Handle_ShouldReturnFailureResult_WhenQueueIsNotFound()
    {
        var queueId = Guid.NewGuid();
        var command = new ChangeQueueActivityBoundaries.Command(queueId, TimeOnly.MinValue, TimeOnly.MaxValue);

        Result<ChangeQueueActivityBoundaries.Response>
            response = await _handler.Handle(command, CancellationToken.None);

        response.IsFaulted.Should().BeTrue();
        response.Error.Should().Be(DomainErrors.Entity.NotFoundFor<QueueEntity>(queueId.ToString()));
    }
}