using Domain.Common.Result;
using Domain.Core.Order;
using Domain.Core.Order.Events;
using Domain.Core.Queue;
using Domain.Core.User;
using Domain.Core.ValueObjects;
using Domain.Kernel;
using FluentAssertions;
using Moq;
using Test.Core.Domain.Entities.ClassData;
using Xunit;

namespace Test.Core.Domain.Entities;

public class OrderTests
{
    private readonly Mock<IDateTimeProvider> _dateTimeProvider = new ();

    [Fact]
    public void CreateOrder_Should_ReturnNotNullOrder()
    {
        _dateTimeProvider.Setup(x => x.DateNow).Returns(DateOnly.FromDateTime(DateTime.UtcNow));

        UserEntity user = UserClassData.Create();

        DateTime queueDate = DateTime.UtcNow.AddDays(1);
        var queue = new QueueEntity(
            Capacity.Create(10).Value,
            QueueDate.Create(DateOnly.FromDateTime(queueDate), _dateTimeProvider.Object).Value,
            QueueActivityBoundaries.Create(
                TimeOnly.FromDateTime(queueDate),
                TimeOnly.FromDateTime(queueDate).AddHours(5)).Value);

        Result<OrderEntity> orderCreationResult = OrderEntity.Create(user, queue, _dateTimeProvider.Object.DateNow);

        orderCreationResult.IsSuccess.Should().BeTrue();
        orderCreationResult.Value.Should().NotBeNull();
        orderCreationResult.Value.Queue.Should().Be(queue);
        orderCreationResult.Value.User.Should().Be(user);
        orderCreationResult.Value.Paid.Should().BeFalse();
        orderCreationResult.Value.Ready.Should().BeFalse();
        orderCreationResult.Value.CreationDate.Should().Be(_dateTimeProvider.Object.DateNow);
        orderCreationResult.Value.ModifiedOn.Should().BeNull();
    }

    [Theory]
    [ClassData(typeof(OrderClassData))]
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
    [ClassData(typeof(OrderClassData))]
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
}