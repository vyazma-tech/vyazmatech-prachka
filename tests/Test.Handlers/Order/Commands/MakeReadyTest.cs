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
using Test.Handlers.Fixtures;
using Xunit;

namespace Test.Handlers.Order.Commands;

public class MakeReadyTest : TestBase
{
    private readonly MarkOrderAsReadyCommandHandler _handler;
    private readonly IDateTimeProvider _dateTimeProvider;
    
    public MakeReadyTest(CoreDatabaseFixture database) : base(database)
    {
        var orderRepository = new OrderRepository(database.Context);
        _dateTimeProvider = new DateTimeProvider();
        ILogger<MarkOrderAsReadyCommandHandler> loggerMock = new Mock<ILogger<MarkOrderAsReadyCommandHandler>>().Object;
        _handler = new MarkOrderAsReadyCommandHandler(orderRepository, loggerMock, _dateTimeProvider);
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