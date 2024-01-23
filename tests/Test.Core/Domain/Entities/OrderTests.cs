using Domain.Common.Abstractions;
using Domain.Common.Result;
using Domain.Core.Order;
using Domain.Core.Order.Events;
using FluentAssertions;
using Infrastructure.Tools;
using Xunit;

namespace Test.Core.Domain.Entities;

public class OrderTests
{
    [Fact]
    public void MakeReady_ShouldRaiseDomainEvent_WhenOrderIsNotAlreadyReady()
    {
        var order = new OrderEntity(
            Guid.Empty,
            Guid.Empty,
            Guid.Empty,
            OrderStatus.Paid,
            SpbDateTimeProvider.CurrentDateTime);

        SpbDateTime modificationDate = SpbDateTimeProvider.CurrentDateTime;
        Result<OrderEntity> actionResult = order.MakeReady(modificationDate);

        actionResult.IsSuccess.Should().BeTrue();
        actionResult.Value.ModifiedOn.Should().Be(modificationDate);
        actionResult.Value.Status.Should().Be(OrderStatus.Ready);
        actionResult.Value.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<OrderReadyDomainEvent>();
    }

    [Fact]
    public void MakePayment_ShouldRaiseDomainEvent_WhenOrderIsNotAlreadyPaid()
    {
        var order = new OrderEntity(
            Guid.Empty,
            Guid.Empty,
            Guid.Empty,
            OrderStatus.New,
            SpbDateTimeProvider.CurrentDateTime);
        
        SpbDateTime modificationDate = SpbDateTimeProvider.CurrentDateTime;
        Result<OrderEntity> actionResult = order.MakePayment(modificationDate);

        actionResult.IsSuccess.Should().BeTrue();
        actionResult.Value.ModifiedOn.Should().Be(modificationDate);
        actionResult.Value.Status.Should().Be(OrderStatus.Paid);
        actionResult.Value.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<OrderPaidDomainEvent>();
    }
}