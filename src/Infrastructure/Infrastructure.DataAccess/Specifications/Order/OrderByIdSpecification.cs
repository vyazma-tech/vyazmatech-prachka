using Domain.Core.Order;
using Domain.Kernel;

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
        => $"{typeof(Guid)}: {_orderId}";
}