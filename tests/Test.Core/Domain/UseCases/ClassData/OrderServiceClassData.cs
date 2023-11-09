using System.Collections;
using Domain.Common.Result;
using Domain.Core.Order;
using Domain.Core.Queue;
using Domain.Core.User;
using Domain.Core.ValueObjects;
using Infrastructure.Tools;

namespace Test.Core.Domain.UseCases.ClassData;

public sealed class OrderServiceClassData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        var queue = new QueueEntity(
            Capacity.Create(1).Value,
            QueueDate.Create(DateTime.Now, new DateTimeProvider()).Value,
            QueueActivityBoundaries.Create(
                TimeOnly.FromDateTime(DateTime.Now).AddHours(1),
                TimeOnly.FromDateTime(DateTime.Now).AddHours(2)).Value);

        var user = new UserEntity(
            TelegramId.Create("1").Value,
            DateTime.Today);

        Result<OrderEntity> orderCreationResult = OrderEntity.Create(
            user,
            queue,
            DateTime.Now);

        yield return new object[] { queue, orderCreationResult.Value };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}