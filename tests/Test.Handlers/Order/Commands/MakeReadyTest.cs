using Application.DataAccess.Contracts;
using Application.Handlers.Order.Commands.MarkOrderAsReady;
using Domain.Common.Errors;
using Domain.Common.Result;
using Domain.Core.Order;
using Domain.Kernel;
using FluentAssertions;
using Infrastructure.DataAccess.Contexts;
using Infrastructure.DataAccess.Contracts;
using Infrastructure.DataAccess.Models;
using Moq;
using Xunit;
using CancellationToken = System.Threading.CancellationToken;

namespace Test.Handlers.Order.Commands;

public class MakeReadyTest
{
    private readonly MarkOrderAsReadyCommandHandler _handler;
    private readonly Mock<IOrderRepository> _orderRepository;

    public MakeReadyTest()
    {
        var dateTimeProvider = new Mock<IDateTimeProvider>();
        var orders = new Mock<IOrderRepository>();
        IQueueRepository queues = Mock.Of<IQueueRepository>();
        IUserRepository users = Mock.Of<IUserRepository>();
        IOrderSubscriptionRepository orderSubscriptions = Mock.Of<IOrderSubscriptionRepository>();
        IQueueSubscriptionRepository queueSubscriptions = Mock.Of<IQueueSubscriptionRepository>();
        IUnitOfWork context = Mock.Of<IUnitOfWork>();

        _orderRepository = orders;
        _handler = new MarkOrderAsReadyCommandHandler(
            dateTimeProvider.Object,
            context,
            new PersistenceContext(
                queues,
                orders.Object,
                users,
                orderSubscriptions,
                queueSubscriptions,
                null!));
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResult_WhenOrderNotFound()
    {
        var orderId = Guid.NewGuid();
        var command = new MarkOrderAsReady.Command(orderId);
        _orderRepository.Setup(x => x.FindByAsync(
                It.IsAny<Specification<OrderModel>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Result<OrderEntity>(DomainErrors.Entity.NotFoundFor<OrderEntity>($"OrderId = {orderId}")));

        Result<MarkOrderAsReady.Response> response = await _handler.Handle(command, CancellationToken.None);

        response.IsFaulted.Should().BeTrue();
        response.Error.Should().Be(DomainErrors.Entity.NotFoundFor<OrderEntity>($"OrderId = {orderId}"));
    }
}