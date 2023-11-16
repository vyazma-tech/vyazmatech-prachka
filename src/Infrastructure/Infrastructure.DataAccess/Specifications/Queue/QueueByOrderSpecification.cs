using Domain.Common.Abstractions;
using Domain.Core.Order;
using Domain.Core.Queue;

namespace Infrastructure.DataAccess.Specifications.Queue;

public sealed class QueueByOrderSpecification : Specification<QueueEntity>
{
    private readonly Guid _orderId;

    public QueueByOrderSpecification(OrderEntity order)
        : base(queue => queue.Items.Contains(order))
    {
        _orderId = order.Id;
    }

    public override string ToString()
    {
        return $"{typeof(OrderEntity)}: {_orderId}";
    }
}