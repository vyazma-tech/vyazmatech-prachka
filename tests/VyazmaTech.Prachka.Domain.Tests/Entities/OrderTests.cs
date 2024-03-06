using FluentAssertions;
using VyazmaTech.Prachka.Domain.Common.Result;
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
            id: Guid.Empty,
            queueId: Guid.Empty,
            user: null!,
            status: OrderStatus.Paid,
            creationDateTimeUtc: default);

        Result<OrderEntity> actionResult = order.MakeReady(default);

        actionResult.IsSuccess.Should().BeTrue();
        actionResult.Value.Status.Should().Be(OrderStatus.Ready);
        actionResult.Value.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<OrderReadyDomainEvent>();
    }

    [Fact]
    public void MakePayment_ShouldRaiseDomainEvent_WhenOrderIsNotAlreadyPaid()
    {
        var order = new OrderEntity(
            id: Guid.Empty,
            queueId: Guid.Empty,
            user: null!,
            status: OrderStatus.New,
            creationDateTimeUtc: default);

        Result<OrderEntity> actionResult = order.MakePayment(default);

        actionResult.IsSuccess.Should().BeTrue();
        actionResult.Value.Status.Should().Be(OrderStatus.Paid);
        actionResult.Value.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<OrderPaidDomainEvent>();
    }
}