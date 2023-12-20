using Application.Handlers.Order.Commands.MarkOrderAsReady;
using Application.Handlers.Order.Queries;
using Domain.Common.Errors;
using Domain.Common.Result;
using Domain.Core.Order;
using Domain.Kernel;
using FluentAssertions;
using Infrastructure.DataAccess.Repositories;
using Infrastructure.Tools;
using Microsoft.Extensions.Logging;
using Moq;
using Test.Core.Domain.Entities.ClassData;
using Test.Handlers.Fixtures;
using Xunit;

namespace Test.Handlers.Order.Commands;

public class MakeReadyTest : TestBase
{
    private readonly MarkOrderAsReadyCommandHandler _handler;
    private readonly IDateTimeProvider _dateTimeProvider;
    
    public MakeReadyTest(CoreDatabaseFixture database) : base(database)
    {
        _dateTimeProvider = new DateTimeProvider();
        _handler = new MarkOrderAsReadyCommandHandler(_dateTimeProvider, database.Context);
    }
    
    [Fact]
    public async Task MarkAsReadyOrder_WhenOrderNotFoundById()
    {
        var orderId = Guid.NewGuid();
        var command = new MarkOrderAsReadyCommand(orderId);

        Result<OrderResponse> response = await _handler.Handle(command, CancellationToken.None);

        response.Should().NotBeNull();
    }

    [Theory]
    [ClassData(typeof(OrderClassData))]
    public async Task MarkAsReadyOrder_WhenOrderExistAndNotReadyBefore(OrderEntity order)
    {
        Database.Context.Orders.Add(order);

        await Database.Context.SaveChangesAsync();

        OrderEntity createdOrder = Database.Context.Orders.First();
        
        var command = new MarkOrderAsReadyCommand(createdOrder.Id);

        Result<OrderResponse> response = await _handler.Handle(command, CancellationToken.None);

        response.Should().NotBeNull();
        response.IsSuccess.Should().BeTrue();
        response.Value.Should().NotBeNull();
        response.Value.Order.Ready.Should().BeTrue();
    }
}