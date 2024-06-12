using FluentAssertions;
using VyazmaTech.Prachka.Application.Contracts.Orders.Commands;
using VyazmaTech.Prachka.Application.Handlers.Order.Commands.MarkOrderAsPaid;
using VyazmaTech.Prachka.Application.Handlers.Tests.Fixtures;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
using Xunit;

namespace VyazmaTech.Prachka.Application.Handlers.Tests.Order.Commands;

public class MarkAsPaidTest : TestBase
{
    private readonly MarkOrderAsPaidCommandHandler _handler;

    public MarkAsPaidTest(CoreDatabaseFixture fixture) : base(fixture)
    {
        _handler = new MarkOrderAsPaidCommandHandler(fixture.PersistenceContext);
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