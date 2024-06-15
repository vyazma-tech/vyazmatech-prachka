using FluentAssertions;
using VyazmaTech.Prachka.Domain.Core.Orders;
using VyazmaTech.Prachka.Domain.Core.Orders.Events;
using Xunit;

namespace VyazmaTech.Prachka.Domain.Core.Tests.Entities;

public class OrderTests
{
    [Fact]
    public void MakeReady_ShouldRaiseDomainEvent_WhenOrderIsNotAlreadyReady()
    {
        var order = new Order(
            Guid.Empty,
            default!,
            null!,
            OrderStatus.Paid,
            default);

        order.MakeReady();

        order.Status.Should().Be(OrderStatus.Ready);
        order.DomainEvents.Should()
            .ContainSingle()
            .Which.Should()
            .BeOfType<OrderReadyDomainEvent>();
    }

    [Fact]
    public void MakePayment_ShouldRaiseDomainEvent_WhenOrderIsNotAlreadyPaid()
    {
        var order = new Order(
            Guid.Empty,
            default!,
            null!,
            OrderStatus.New,
            default);

        order.MakePayment();

        order.Status.Should().Be(OrderStatus.Paid);
        order.DomainEvents.Should()
            .ContainSingle()
            .Which.Should()
            .BeOfType<OrderPaidDomainEvent>();
    }
}