using FluentAssertions;
using Moq;
using VyazmaTech.Prachka.Application.Contracts.Queues.Commands;
using VyazmaTech.Prachka.Application.Handlers.Queue.Commands.IncreaseQueueCapacity;
using VyazmaTech.Prachka.Application.Handlers.Tests.Fixtures;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
using VyazmaTech.Prachka.Domain.Kernel;
using Xunit;

namespace VyazmaTech.Prachka.Application.Handlers.Tests.Queue.Commands;

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
    public async Task Handle_ShouldThrow_WhenQueueIsNotFound()
    {
        var queueId = Guid.NewGuid();
        var command = new IncreaseQueueCapacity.Command(queueId, 2);

        Func<Task<IncreaseQueueCapacity.Response>> action = async () =>
            await _handler.Handle(command, CancellationToken.None);
        await action.Should().ThrowAsync<NotFoundException>();
    }
}