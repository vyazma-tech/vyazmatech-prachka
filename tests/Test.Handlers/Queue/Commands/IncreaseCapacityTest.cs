using Application.Core.Contracts.Queues.Commands;
using Application.Handlers.Queue.Commands.IncreaseQueueCapacity;
using Domain.Common.Errors;
using Domain.Common.Result;
using Domain.Core.Queue;
using Domain.Kernel;
using FluentAssertions;
using Moq;
using Test.Handlers.Fixtures;
using Xunit;

namespace Test.Handlers.Queue.Commands;

public class IncreaseCapacityTest : TestBase
{
    private readonly IncreaseQueueCapacityCommandHandler _handler;

    public IncreaseCapacityTest(CoreDatabaseFixture fixture) : base(fixture)
    {
        var dateTimeProvider = new Mock<IDateTimeProvider>();

        _handler = new IncreaseQueueCapacityCommandHandler(
            dateTimeProvider.Object,
            fixture.PersistenceContext);
    }


    [Fact]
    public async Task Handle_ShouldReturnFailureResult_WhenQueueIsNotFound()
    {
        var queueId = Guid.NewGuid();
        var command = new IncreaseQueueCapacity.Command(queueId, 2);

        Result<IncreaseQueueCapacity.Response> response = await _handler.Handle(command, CancellationToken.None);

        response.IsFaulted.Should().BeTrue();
        response.Error.Should().Be(DomainErrors.Entity.NotFoundFor<QueueEntity>(queueId.ToString()));
    }
}