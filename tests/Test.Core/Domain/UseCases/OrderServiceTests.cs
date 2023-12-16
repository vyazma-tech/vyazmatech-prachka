using System.Diagnostics.CodeAnalysis;
using Domain.Common.Errors;
using Domain.Common.Result;
using Domain.Core.Order;
using Domain.Core.Queue;
using Domain.Core.ValueObjects;
using FluentAssertions;
using Infrastructure.Tools;
using Moq;
using Test.Core.Domain.UseCases.ClassData;
using Xunit;

namespace Test.Core.Domain.UseCases;

[SuppressMessage("Usage", "xUnit1026:Theory methods should use all of their parameters")]
public class OrderServiceTests
{
    private readonly Mock<IOrderRepository> _orderRepository = new ();
    private readonly Mock<IQueueRepository> _queueRepository = new ();

    [Theory]
    [ClassData(typeof(OrderServiceClassData))]
    public void ProlongOrder_ShouldReturnFailureResult_WhenTransferringIntoSameQueue(
        QueueEntity queue,
        OrderEntity order)
    {
        var service = new OrderService(_orderRepository.Object, _queueRepository.Object);

        Result<OrderEntity> prolongationResult = service.ProlongOrder(
            order,
            queue,
            DateTime.Now);

        prolongationResult.IsFaulted.Should().BeTrue();
        prolongationResult.Error.Message.Should().Be(DomainErrors.Order.UnableToTransferIntoSameQueue.Message);
    }

    [Theory]
    [ClassData(typeof(OrderServiceClassData))]
    public void ProlongOrder_ShouldReturnFailureResult_WhenTransferringIntoFullQueue(
        QueueEntity queue,
        OrderEntity order)
    {
        var service = new OrderService(_orderRepository.Object, _queueRepository.Object);

        var newQueue = new QueueEntity(
            Capacity.Create(0).Value,
            QueueDate.Create(DateOnly.FromDateTime(DateTime.Today.AddDays(1)), new DateTimeProvider()).Value,
            QueueActivityBoundaries.Create(
                TimeOnly.FromDateTime(DateTime.Now).AddHours(1),
                TimeOnly.FromDateTime(DateTime.Now).AddHours(2)).Value);

        Result<OrderEntity> prolongationResult = service.ProlongOrder(
            order,
            newQueue,
            DateTime.Now);

        prolongationResult.IsFaulted.Should().BeTrue();
        prolongationResult.Error.Message.Should().Be(DomainErrors.Order.UnableToTransferIntoFullQueue.Message);
    }

    [Theory]
    [ClassData(typeof(OrderServiceClassData))]
    public async Task ProlongOrder_ShouldReturnFailureResult_WhenTransferringIntoExpiredQueue(
        QueueEntity queue,
        OrderEntity order)
    {
        var service = new OrderService(_orderRepository.Object, _queueRepository.Object);

        var newQueue = new QueueEntity(
            Capacity.Create(10).Value,
            QueueDate.Create(DateOnly.FromDateTime(DateTime.Today.AddDays(1)), new DateTimeProvider()).Value,
            QueueActivityBoundaries.Create(
                TimeOnly.FromDateTime(DateTime.UtcNow),
                TimeOnly.FromDateTime(DateTime.UtcNow.AddSeconds(1))).Value);

        await Task.Delay(1000);
        Result<OrderEntity> prolongationResult = service.ProlongOrder(
            order,
            newQueue,
            DateTime.Now);

        prolongationResult.IsFaulted.Should().BeTrue();
        prolongationResult.Error.Message.Should().Be(DomainErrors.Queue.Expired.Message);
    }
}