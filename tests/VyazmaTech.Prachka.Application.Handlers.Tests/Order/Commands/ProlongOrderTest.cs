using FluentAssertions;
using Moq;
using VyazmaTech.Prachka.Application.Contracts.Orders.Commands;
using VyazmaTech.Prachka.Application.Handlers.Order.Commands.ProlongOrder;
using VyazmaTech.Prachka.Application.Handlers.Tests.Fixtures;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
using VyazmaTech.Prachka.Domain.Kernel;
using Xunit;
using CancellationToken = System.Threading.CancellationToken;

namespace VyazmaTech.Prachka.Application.Handlers.Tests.Order.Commands;

public class ProlongOrderTest : TestBase
{
    private readonly ProlongOrderCommandHandler _handler;

    public ProlongOrderTest(CoreDatabaseFixture fixture) : base(fixture)
    {
        var dateTimeProvider = new Mock<IDateTimeProvider>();

        _handler = new ProlongOrderCommandHandler(
            dateTimeProvider.Object,
            fixture.PersistenceContext);
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
}