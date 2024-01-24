using Application.Core.Contracts.Orders.Commands;
using Application.Handlers.Order.Commands.MarkOrderAsReady;
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

public class MakeReadyTest : TestBase
{
    private readonly MarkOrderAsReadyCommandHandler _handler;

    public MakeReadyTest(CoreDatabaseFixture fixture) : base(fixture)
    {
        var dateTimeProvider = new Mock<IDateTimeProvider>();

        _handler = new MarkOrderAsReadyCommandHandler(
            dateTimeProvider.Object,
            fixture.PersistenceContext);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResult_WhenOrderNotFound()
    {
        var orderId = Guid.NewGuid();
        var command = new MarkOrderAsReady.Command(orderId);

        Result<MarkOrderAsReady.Response> response = await _handler.Handle(command, CancellationToken.None);

        response.IsFaulted.Should().BeTrue();
        response.Error.Should().Be(DomainErrors.Entity.NotFoundFor<OrderEntity>(orderId.ToString()));
    }
}