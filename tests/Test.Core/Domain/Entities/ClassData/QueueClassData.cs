using System.Collections;
using Domain.Common.Abstractions;
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
        UserEntity user = UserClassData.Create();

        var queue = new QueueEntity(
            Guid.NewGuid(),
            Capacity.Create(1).Value,
            QueueDate.Create(DateOnly.FromDateTime(DateTime.Today), new DateTimeProvider()).Value,
            QueueActivityBoundaries.Create(
                TimeOnly.FromDateTime(DateTime.UtcNow.AddHours(5)),
                TimeOnly.FromDateTime(DateTime.UtcNow.AddHours(7))).Value,
            QueueState.Active);

        Result<OrderEntity> order = OrderEntity.Create(
            Guid.NewGuid(),
            user,
            queue,
            OrderStatus.New,
            new SpbDateTime(DateTime.UtcNow));

        yield return new object[] { queue, user, order.Value };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}