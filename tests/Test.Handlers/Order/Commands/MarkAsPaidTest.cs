using Application.DataAccess.Contracts.Repositories;
using Application.Handlers.Order.Commands.MarkOrderAsPaid;
using Domain.Common.Errors;
using Domain.Common.Result;
using Domain.Core.Order;
using Domain.Kernel;
using FluentAssertions;
using Infrastructure.DataAccess.Contexts;
using Infrastructure.DataAccess.Models;
using Moq;
using Xunit;

namespace Test.Handlers.Order.Commands;

public class MarkAsPaidTest
{
    private readonly MarkOrderAsPaidCommandHandler _handler;
    private readonly Mock<IOrderRepository> _orderRepository;

    public MarkAsPaidTest()
    {
        var dateTimeProvider = new Mock<IDateTimeProvider>();
        var orders = new Mock<IOrderRepository>();
        IQueueRepository queues = Mock.Of<IQueueRepository>();
        IUserRepository users = Mock.Of<IUserRepository>();
        IOrderSubscriptionRepository orderSubscriptions = Mock.Of<IOrderSubscriptionRepository>();
        IQueueSubscriptionRepository queueSubscriptions = Mock.Of<IQueueSubscriptionRepository>();

        _orderRepository = orders;
        _handler = new MarkOrderAsPaidCommandHandler(
            dateTimeProvider.Object,
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
        var command = new MarkOrderAsPaid.Command(orderId);
        _orderRepository.Setup(x =>
                x.FindByAsync(
                    It.IsAny<Specification<OrderModel>>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new Result<OrderEntity>(
                    DomainErrors.Entity.NotFoundFor<OrderEntity>($"OrderId = {orderId}")));

        Result<MarkOrderAsPaid.Response> response = await _handler.Handle(command, CancellationToken.None);

        response.IsFaulted.Should().BeTrue();
        response.Error.Should().Be(DomainErrors.Entity.NotFoundFor<OrderEntity>($"OrderId = {orderId}"));
    }
}