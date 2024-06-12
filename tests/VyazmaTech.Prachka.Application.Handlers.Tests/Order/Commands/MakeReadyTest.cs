using FluentAssertions;
using Moq;
using VyazmaTech.Prachka.Application.Contracts.Orders.Commands;
using VyazmaTech.Prachka.Application.Handlers.Order.Commands.MarkOrderAsReady;
using VyazmaTech.Prachka.Application.Handlers.Tests.Fixtures;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
using VyazmaTech.Prachka.Domain.Kernel;
using Xunit;
using CancellationToken = System.Threading.CancellationToken;

namespace VyazmaTech.Prachka.Application.Handlers.Tests.Order.Commands;

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
    public async Task Handle_ShouldThrow_WhenOrderNotFound()
    {
        var orderId = Guid.NewGuid();
        var command = new MarkOrderAsReady.Command(orderId);

        Func<Task<MarkOrderAsReady.Response>> action = async () =>
            await _handler.Handle(command, CancellationToken.None);

        await action.Should().ThrowAsync<NotFoundException>();
    }
}