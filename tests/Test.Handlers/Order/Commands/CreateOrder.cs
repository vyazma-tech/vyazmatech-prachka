using Application.Handlers.Order.Commands.CreateOrder;
using Application.Handlers.Queue.Commands.CreateQueue;
using Domain.Core.Queue;
using Domain.Core.User;
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

public class CreateOrdersTests : TestBase
{
    private readonly CreateOrderCommandHandler _handler;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CreateOrdersTests(CoreDatabaseFixture database) : base(database)
    {
        ILogger<CreateOrderCommandHandler> loggerMock = new Mock<ILogger<CreateOrderCommandHandler>>().Object;
        var queueRepository = new QueueRepository(database.Context);
        var orderRepository = new OrderRepository(database.Context);
        var userRepository = new UserRepository(database.Context);
        
        _dateTimeProvider = new DateTimeProvider();

        _handler = new CreateOrderCommandHandler(
            userRepository,
            queueRepository,
            orderRepository,
            loggerMock,
            database.Context);
    }
}