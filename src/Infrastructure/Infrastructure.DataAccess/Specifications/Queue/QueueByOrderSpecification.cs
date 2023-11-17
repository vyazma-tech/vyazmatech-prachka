using Domain.Core.Order;
using Domain.Core.Queue;
using Domain.Kernel;

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
        => $"{typeof(OrderEntity)}: {_orderId}";
}