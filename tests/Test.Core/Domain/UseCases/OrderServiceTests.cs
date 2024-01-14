using System.Diagnostics.CodeAnalysis;
using Application.Core.Services;
using Domain.Common.Errors;
using Domain.Common.Result;
using Domain.Core.Order;
using Domain.Core.Queue;
using Domain.Core.ValueObjects;
using Domain.Kernel;
using FluentAssertions;
using Infrastructure.DataAccess.Contracts;
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
    private readonly Mock<IDateTimeProvider> _timeProvider = new ();
    private readonly OrderService _service;

    public OrderServiceTests()
    {
        _timeProvider.Setup(x => x.UtcNow).Returns(DateTime.UtcNow);
        _timeProvider.Setup(x => x.DateNow).Returns(DateOnly.FromDateTime(DateTime.UtcNow));

        _service = new OrderService(_orderRepository.Object, _queueRepository.Object, _timeProvider.Object);
    }

    [Theory]
    [ClassData(typeof(OrderServiceClassData))]
    public void ProlongOrder_ShouldReturnFailureResult_WhenTransferringIntoSameQueue(
        QueueEntity queue,
        OrderEntity order)
    {
        Result<OrderEntity> prolongationResult = _service.ProlongOrder(order, queue);

        prolongationResult.IsFaulted.Should().BeTrue();
        prolongationResult.Error.Should().Be(DomainErrors.Order.UnableToTransferIntoSameQueue);
    }

    [Theory]
    [ClassData(typeof(OrderServiceClassData))]
    public void ProlongOrder_ShouldReturnFailureResult_WhenTransferringIntoFullQueue(
        QueueEntity queue,
        OrderEntity order)
    {
        var newQueue = new QueueEntity(
            Guid.NewGuid(),
            Capacity.Create(0).Value,
            QueueDate.Create(DateOnly.FromDateTime(DateTime.Today.AddDays(1)), new DateTimeProvider()).Value,
            QueueActivityBoundaries.Create(
                TimeOnly.FromDateTime(DateTime.Now).AddHours(1),
                TimeOnly.FromDateTime(DateTime.Now).AddHours(2)).Value,
            QueueState.Active);

        Result<OrderEntity> prolongationResult = _service.ProlongOrder(order, newQueue);

        prolongationResult.IsFaulted.Should().BeTrue();
        prolongationResult.Error.Should().Be(DomainErrors.Order.UnableToTransferIntoFullQueue);
    }

    [Theory]
    [ClassData(typeof(OrderServiceClassData))]
    public void ProlongOrder_ShouldReturnFailureResult_WhenTransferringIntoExpiredQueue(
        QueueEntity queue,
        OrderEntity order)
    {
        var timeProvider = new Mock<IDateTimeProvider>();
        timeProvider.Setup(x => x.DateNow).Returns(DateOnly.FromDateTime(DateTime.Now));
        timeProvider.Setup(x => x.UtcNow).Returns(DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(1)));
            
        var newQueue = new QueueEntity(
            Guid.NewGuid(),
            Capacity.Create(10).Value,
            QueueDate.Create(timeProvider.Object.DateNow, timeProvider.Object).Value,
            QueueActivityBoundaries.Create(
                TimeOnly.FromDateTime(timeProvider.Object.UtcNow),
                TimeOnly.FromDateTime(timeProvider.Object.UtcNow.AddSeconds(1))).Value,
            QueueState.Active);

        Result<OrderEntity> prolongationResult = _service.ProlongOrder(order, newQueue);

        prolongationResult.IsFaulted.Should().BeTrue();
        prolongationResult.Error.Should().Be(DomainErrors.Queue.Expired);
    }
}