using Domain.Common.Abstractions;
using Domain.Common.Result;
using Domain.Core.Order;
using Domain.Core.Order.Events;
using Domain.Core.Queue;
using Domain.Core.User;
using Domain.Core.ValueObjects;
using FluentAssertions;
using Infrastructure.Tools;
using Moq;
using Xunit;

namespace Test.Core.Domain.Entities;

public class OrderTests
{
    private readonly Mock<IDateTimeProvider> _dateTimeProvider = new Mock<IDateTimeProvider>();

    [Fact]
    public void CreateOrder_ShouldReturnNotNullOrderAndRaiseDomainEvent()
    {
        _dateTimeProvider.Setup(x => x.UtcNow).Returns(DateTime.UtcNow);

        var user = new UserEntity(
            TelegramId.Create("1").Value,
            _dateTimeProvider.Object.UtcNow);

        DateTime queueDate = DateTime.UtcNow.AddDays(1);
        var queue = new QueueEntity(
            Capacity.Create(10).Value,
            QueueDate.Create(queueDate, _dateTimeProvider.Object).Value);

        var order = new OrderEntity(user, queue, _dateTimeProvider.Object.UtcNow);

        order.Should().NotBeNull();
        order.Queue.Should().Be(queue);
        order.User.Should().Be(user);
        order.Paid.Should().BeFalse();
        order.Ready.Should().BeFalse();
        order.CreationDate.Should().Be(_dateTimeProvider.Object.UtcNow);
        order.ModifiedOn.Should().BeNull();
        order.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<OrderCreatedDomainEvent>();
    }

    [Theory]
    [MemberData(nameof(OrderMemberData))]
    public void MakeOrderReady_ShouldRaiseDomainEvent_WhenOrderIsNotAlreadyReady(OrderEntity order)
    {
        DateTime modificationDate = DateTime.UtcNow;
        Result<OrderEntity> actionResult = order.MakeReady(modificationDate);

        actionResult.IsSuccess.Should().BeTrue();
        actionResult.Value.ModifiedOn.Should().Be(modificationDate);
        actionResult.Value.Ready.Should().BeTrue();
        actionResult.Value.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<OrderReadyDomainEvent>();
    }
    
    [Theory]
    [MemberData(nameof(OrderMemberData))]
    public void MakeOrderPayment_ShouldRaiseDomainEvent_WhenOrderIsNotAlreadyPaid(OrderEntity order)
    {
        DateTime modificationDate = DateTime.UtcNow;
        Result<OrderEntity> actionResult = order.MakePayment(modificationDate);

        actionResult.IsSuccess.Should().BeTrue();
        actionResult.Value.ModifiedOn.Should().Be(modificationDate);
        actionResult.Value.Paid.Should().BeTrue();
        actionResult.Value.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<OrderPaidDomainEvent>();
    }
    
    [Theory]
    [MemberData(nameof(OrderMemberData))]
    public void ProlongOrder_ShouldRaiseDomainEvent(OrderEntity order)
    {
        DateTime modificationDate = DateTime.UtcNow;
        Result<OrderEntity> actionResult = order.Prolong(modificationDate);

        actionResult.IsSuccess.Should().BeTrue();
        actionResult.Value.ModifiedOn.Should().Be(modificationDate);
        actionResult.Value.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<OrderProlongedDomainEvent>();
    }
    public static IEnumerable<object[]> OrderMemberData()
    {
        var dateTimeProvider = new DateTimeProvider();
        var user = new UserEntity(
            TelegramId.Create("1").Value,
            dateTimeProvider.UtcNow);

        DateTime queueDate = DateTime.UtcNow.AddDays(1);
        var queue = new QueueEntity(
            Capacity.Create(10).Value,
            QueueDate.Create(queueDate, dateTimeProvider).Value);

        var order = new OrderEntity(user, queue, dateTimeProvider.UtcNow);
        order.ClearDomainEvents();
        yield return new object[] { order };
    }
}