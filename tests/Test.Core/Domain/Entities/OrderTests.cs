using Domain.Common.Abstractions;
using Domain.Common.Result;
using Domain.Core.Order;
using Domain.Core.Order.Events;
using Domain.Core.Queue;
using Domain.Core.User;
using Domain.Core.ValueObjects;
using Domain.Kernel;
using FluentAssertions;
using Infrastructure.Tools;
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
        _dateTimeProvider.Setup(x => x.SpbDateOnlyNow).Returns(DateOnly.FromDateTime(DateTime.UtcNow));
        _dateTimeProvider.Setup(x => x.SpbDateTimeNow).Returns(SpbDateTimeProvider.CurrentDateTime);

        UserEntity user = UserClassData.Create();

        DateTime queueDate = DateTime.UtcNow.AddDays(1);
        var queue = new QueueEntity(
            Guid.NewGuid(),
            Capacity.Create(10).Value,
            QueueDate.Create(DateOnly.FromDateTime(queueDate), _dateTimeProvider.Object).Value,
            QueueActivityBoundaries.Create(
                TimeOnly.FromDateTime(queueDate),
                TimeOnly.FromDateTime(queueDate).AddHours(5)).Value,
            QueueState.Active);

        Result<OrderEntity> orderCreationResult = OrderEntity.Create(
            Guid.NewGuid(),
            user,
            queue,
            OrderStatus.New,
            _dateTimeProvider.Object.SpbDateTimeNow);

        orderCreationResult.IsSuccess.Should().BeTrue();
        orderCreationResult.Value.Should().NotBeNull();
        orderCreationResult.Value.Queue.Should().Be(queue);
        orderCreationResult.Value.User.Should().Be(user);
        orderCreationResult.Value.Status.Should().Be(OrderStatus.New);
        orderCreationResult.Value.CreationDate.Should().Be(_dateTimeProvider.Object.SpbDateOnlyNow);
        orderCreationResult.Value.ModifiedOn.Should().BeNull();
    }

    [Theory]
    [ClassData(typeof(OrderClassData))]
    public void MakeReady_ShouldRaiseDomainEvent_WhenOrderIsNotAlreadyReady(OrderEntity order)
    {
        var modificationDate = SpbDateTimeProvider.CurrentDateTime;
        Result<OrderEntity> actionResult = order.MakeReady(modificationDate);

        actionResult.IsSuccess.Should().BeTrue();
        actionResult.Value.ModifiedOn.Should().Be(modificationDate);
        actionResult.Value.Status.Should().Be(OrderStatus.Ready);
        actionResult.Value.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<OrderReadyDomainEvent>();
    }

    [Theory]
    [ClassData(typeof(OrderClassData))]
    public void MakePayment_ShouldRaiseDomainEvent_WhenOrderIsNotAlreadyPaid(OrderEntity order)
    {
        var modificationDate = SpbDateTimeProvider.CurrentDateTime;
        Result<OrderEntity> actionResult = order.MakePayment(modificationDate);

        actionResult.IsSuccess.Should().BeTrue();
        actionResult.Value.ModifiedOn.Should().Be(modificationDate);
        actionResult.Value.Status.Should().Be(OrderStatus.Paid);
        actionResult.Value.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<OrderPaidDomainEvent>();
    }
}