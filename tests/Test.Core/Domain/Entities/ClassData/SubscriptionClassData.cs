using System.Collections;
using Domain.Common.Result;
using Domain.Core.Order;
using Domain.Core.Queue;
using Domain.Core.Subscription;
using Domain.Core.User;
using Domain.Core.ValueObjects;
using Infrastructure.Tools;

namespace Test.Core.Domain.Entities.ClassData;

public sealed class SubscriptionClassData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        var dateTimeProvider = new DateTimeProvider();
        UserEntity user = UserClassData.Create();

        var queue = new QueueEntity(
            Capacity.Create(10).Value,
            QueueDate.Create(DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)), dateTimeProvider).Value,
            QueueActivityBoundaries.Create(
                TimeOnly.FromDateTime(DateTime.UtcNow),
                TimeOnly.FromDateTime(DateTime.UtcNow.AddSeconds(10))).Value);

        Result<OrderEntity> order = OrderEntity.Create(
            user,
            queue,
            DateOnly.FromDateTime(DateTime.UtcNow));

        var subscription = new OrderSubscriptionEntity(
            user,
            DateOnly.FromDateTime(DateTime.UtcNow));

        yield return new object[] { subscription, order.Value };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}