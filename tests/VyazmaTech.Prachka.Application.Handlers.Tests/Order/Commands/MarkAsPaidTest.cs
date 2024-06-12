using FluentAssertions;
using Moq;
using VyazmaTech.Prachka.Application.Contracts.Orders.Commands;
using VyazmaTech.Prachka.Application.Handlers.Order.Commands.MarkOrderAsPaid;
using VyazmaTech.Prachka.Application.Handlers.Tests.Fixtures;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
using VyazmaTech.Prachka.Domain.Kernel;
using Xunit;

namespace VyazmaTech.Prachka.Application.Handlers.Tests.Order.Commands;

public class MarkAsPaidTest : TestBase
{
    private readonly MarkOrderAsPaidCommandHandler _handler;

    public MarkAsPaidTest(CoreDatabaseFixture fixture) : base(fixture)
    {
        var dateTimeProvider = new Mock<IDateTimeProvider>();

        _handler = new MarkOrderAsPaidCommandHandler(
            dateTimeProvider.Object,
            fixture.PersistenceContext);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenOrderNotFound()
    {
        var orderId = Guid.NewGuid();
        var command = new MarkOrderAsPaid.Command(orderId);

        Func<Task<MarkOrderAsPaid.Response>>
            action = async () => await _handler.Handle(command, CancellationToken.None);
        await action.Should().ThrowAsync<NotFoundException>();
    }
}