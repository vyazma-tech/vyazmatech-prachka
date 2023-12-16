using Domain.Core.Order;
using Domain.Kernel;

namespace Infrastructure.DataAccess.Specifications.Order;

public sealed class OrderByDateSpecification : Specification<OrderEntity>
{
    public OrderByDateSpecification(DateOnly creationDate)
        : base(order => order.CreationDate == creationDate)
    {
    }

    public override string ToString()
        => string.Empty;
}