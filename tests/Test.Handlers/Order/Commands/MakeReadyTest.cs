using Application.Handlers.Order.Commands.MarkOrderAsReady;
using Application.Handlers.Order.Queries;
using Domain.Common.Errors;
using Domain.Common.Result;
using Domain.Core.Order;
using FluentAssertions;
using Infrastructure.DataAccess.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Test.Handlers.Fixtures;
using Xunit;

namespace Test.Handlers.Order.Commands;

public class MakeReadyTest : TestBase
{
    private readonly MarkOrderAsReadyCommandHandler _handler;
    
    public MakeReadyTest(CoreDatabaseFixture database) : base(database)
    {
        var orderRepository = new OrderRepository(database.Context);
        ILogger<MarkOrderAsReadyCommandHandler> loggerMock = new Mock<ILogger<MarkOrderAsReadyCommandHandler>>().Object;
        _handler = new MarkOrderAsReadyCommandHandler(orderRepository, loggerMock);
    }
    
    [Fact]
    public async Task CreateOneQueue_Should_ReturnCreatedQueue_WhenDataIsValid()
    {
        var orderId = Guid.NewGuid();
        var command = new MarkOrderAsReadyCommand(orderId);

        Task response = await _handler.Handle(command, CancellationToken.None);

        response.Should().NotBeNull();
    }    
}