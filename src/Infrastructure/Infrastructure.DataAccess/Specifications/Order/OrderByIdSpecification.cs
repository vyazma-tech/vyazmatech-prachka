using Infrastructure.DataAccess.Contracts;
using Infrastructure.DataAccess.Models;

namespace Infrastructure.DataAccess.Specifications.Order;

public sealed class OrderByIdSpecification : Specification<OrderModel>
{
    private readonly Guid _orderId;

    public OrderByIdSpecification(Guid id)
        : base(order => order.Id == id)
    {
        _orderId = id;
    }

    public override string ToString()
        => $"OrderId = {_orderId}";
}