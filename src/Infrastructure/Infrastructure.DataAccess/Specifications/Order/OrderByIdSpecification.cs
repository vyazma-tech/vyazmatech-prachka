using Domain.Common.Abstractions;
using Domain.Core.Order;

namespace Infrastructure.DataAccess.Specifications.Order;

public sealed class OrderByIdSpecification : Specification<OrderEntity>
{
    private readonly Guid _orderId;

    public OrderByIdSpecification(Guid id)
        : base(order => order.Id == id)
    {
        _orderId = id;
    }

    public override string ToString()
    {
        return $"{typeof(Guid)}: {_orderId}";
    }
}