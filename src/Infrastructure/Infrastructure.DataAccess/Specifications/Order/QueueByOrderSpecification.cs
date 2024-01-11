using Infrastructure.DataAccess.Contracts;
using Infrastructure.DataAccess.Models;

namespace Infrastructure.DataAccess.Specifications.Order;

public sealed class QueueByOrderSpecification : Specification<QueueModel>
{
    private readonly Guid _orderId;

    public QueueByOrderSpecification(OrderModel order)
        : base(queue => queue.Orders.Contains(order))
    {
        _orderId = order.Id;
    }

    public override string ToString()
        => $"OrderId = {_orderId}";
}