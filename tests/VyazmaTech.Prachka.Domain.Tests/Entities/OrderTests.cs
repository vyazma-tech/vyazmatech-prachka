using FluentAssertions;
using VyazmaTech.Prachka.Domain.Core.Order;
using VyazmaTech.Prachka.Domain.Core.Order.Events;
using Xunit;

namespace VyazmaTech.Prachka.Domain.Tests.Entities;

public class OrderTests
{
    [Fact]
    public void MakeReady_ShouldRaiseDomainEvent_WhenOrderIsNotAlreadyReady()
    {
        var order = new OrderEntity(
            Guid.Empty,
            Guid.Empty,
            null!,
            OrderStatus.Paid,
            default);

        order.MakeReady(default);

        order.Status.Should().Be(OrderStatus.Ready);
        order.DomainEvents.Should()
            .ContainSingle()
            .Which.Should()
            .BeOfType<OrderReadyDomainEvent>();
    }

    [Fact]
    public void MakePayment_ShouldRaiseDomainEvent_WhenOrderIsNotAlreadyPaid()
    {
        var order = new OrderEntity(
            Guid.Empty,
            Guid.Empty,
            null!,
            OrderStatus.New,
            default);

        order.MakePayment(default);

        order.Status.Should().Be(OrderStatus.Paid);
        order.DomainEvents.Should()
            .ContainSingle()
            .Which.Should()
            .BeOfType<OrderPaidDomainEvent>();
    }
}