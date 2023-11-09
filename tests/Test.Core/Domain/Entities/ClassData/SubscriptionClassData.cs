using System.Collections;
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
        var user = new UserEntity(
            TelegramId.Create("1").Value,
            DateTime.UtcNow);
        
        var queue = new QueueEntity(
            Capacity.Create(10).Value,
            QueueDate.Create(DateTime.UtcNow.AddDays(1), dateTimeProvider).Value,
            QueueActivityBoundaries.Create(
                TimeOnly.FromDateTime(DateTime.UtcNow),
                TimeOnly.FromDateTime(DateTime.UtcNow.AddSeconds(10))).Value);

        var order = OrderEntity.Create(
            user,
            queue,
            DateTime.UtcNow);

        var subscription = new SubscriptionEntity(
            user,
            DateTime.UtcNow);

        yield return new object[] { subscription, order.Value };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}