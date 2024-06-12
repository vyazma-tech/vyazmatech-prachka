﻿using FluentAssertions;
using Moq;
using VyazmaTech.Prachka.Application.Core.Services;
using VyazmaTech.Prachka.Application.DataAccess.Contracts.Repositories;
using VyazmaTech.Prachka.Domain.Common.Errors;
using VyazmaTech.Prachka.Domain.Common.Result;
using VyazmaTech.Prachka.Domain.Core.Order;
using VyazmaTech.Prachka.Domain.Core.Queue;
using VyazmaTech.Prachka.Domain.Kernel;
using Xunit;

namespace VyazmaTech.Prachka.Domain.Tests.UseCases;

public class OrderServiceTests
{
    private readonly Mock<IOrderRepository> _orderRepository = new();
    private readonly Mock<IDateTimeProvider> _timeProvider = new();
    private readonly OrderService _service;

    public OrderServiceTests()
    {
        _timeProvider.Setup(x => x.UtcNow).Returns(DateTime.UtcNow);
        _timeProvider.Setup(x => x.DateNow).Returns(DateOnly.FromDateTime(DateTime.UtcNow));

        _service = new OrderService(_orderRepository.Object, _timeProvider.Object);
    }

    [Fact]
    public void ProlongOrder_ShouldReturnFailureResult_WhenTransferringIntoSameQueue()
    {
        var order = new OrderEntity(
            Guid.Empty,
            Guid.Empty,
            null!,
            OrderStatus.New,
            default);

        var queue = new QueueEntity(
            Guid.NewGuid(),
            1,
            default,
            default,
            default,
            QueueState.Active,
            Array.Empty<OrderInfo>().ToHashSet(new OrderByIdComparer()));

        Result<OrderEntity> prolongationResult = _service.ProlongOrder(order, queue, queue);

        prolongationResult.IsFaulted.Should().BeTrue();
        prolongationResult.Error.Should().Be(DomainErrors.Order.UnableToTransferIntoSameQueue);
    }

    [Fact]
    public void ProlongOrder_ShouldReturnFailureResult_WhenTransferringIntoFullQueue()
    {
        var order = new OrderEntity(
            Guid.Empty,
            Guid.Empty,
            null!,
            OrderStatus.New,
            default);

        var targetQueue = new QueueEntity(
            Guid.NewGuid(),
            1,
            default,
            default,
            default,
            QueueState.Active,
            new HashSet<OrderInfo>(new OrderByIdComparer()) { new(Guid.NewGuid(), default!, default, default) });

        var queue = new QueueEntity(
            Guid.NewGuid(),
            1,
            default,
            default,
            default,
            QueueState.Active,
            Array.Empty<OrderInfo>().ToHashSet(new OrderByIdComparer()));

        Result<OrderEntity> prolongationResult =
            _service.ProlongOrder(order, queue, targetQueue);

        prolongationResult.IsFaulted.Should().BeTrue();
        prolongationResult.Error.Should().Be(DomainErrors.Order.UnableToTransferIntoFullQueue);
    }
}