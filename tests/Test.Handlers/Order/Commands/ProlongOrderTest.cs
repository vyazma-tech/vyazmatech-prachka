using Application.Core.Contracts.Orders.Commands;
using Application.Handlers.Order.Commands.ProlongOrder;
using Domain.Common.Errors;
using Domain.Common.Result;
using Domain.Core.Order;
using Domain.Kernel;
using FluentAssertions;
using Moq;
using Test.Handlers.Fixtures;
using Xunit;
using CancellationToken = System.Threading.CancellationToken;

namespace Test.Handlers.Order.Commands;

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
    public async Task Handle_ShouldReturnFailureResult_WhenOrderNotFound()
    {
        var orderId = Guid.NewGuid();
        var queueId = Guid.NewGuid();
        var command = new ProlongOrder.Command(orderId, queueId);

        Result<ProlongOrder.Response> response = await _handler.Handle(command, CancellationToken.None);

        response.IsFaulted.Should().BeTrue();
        response.Error.Should().Be(DomainErrors.Entity.NotFoundFor<OrderEntity>(orderId.ToString()));
    }
}