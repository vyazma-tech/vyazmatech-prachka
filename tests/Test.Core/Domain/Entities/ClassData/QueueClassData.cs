using System.Collections;
using Domain.Common.Result;
using Domain.Core.Order;
using Domain.Core.Queue;
using Domain.Core.User;
using Domain.Core.ValueObjects;
using Infrastructure.Tools;

namespace Test.Core.Domain.Entities.ClassData;

public sealed class QueueClassData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        var user = new UserEntity(
            TelegramId.Create("1").Value,
            DateTime.UtcNow);

        var queue = new QueueEntity(
            Capacity.Create(1).Value,
            QueueDate.Create(DateTime.UtcNow.AddSeconds(5), new DateTimeProvider()).Value,
            QueueActivityBoundaries.Create(
                TimeOnly.FromDateTime(DateTime.UtcNow),
                TimeOnly.FromDateTime(DateTime.UtcNow.AddSeconds(1))).Value);

        Result<OrderEntity> order = OrderEntity.Create(
            user,
            queue,
            DateTime.UtcNow);

        yield return new object[] { queue, user, order.Value };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}