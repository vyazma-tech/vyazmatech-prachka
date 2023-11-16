using Domain.Core.Order;

namespace Infrastructure.DataAccess.Specifications.Order;

public sealed class OrderByDateSpecification : Specification<OrderEntity>
{
    private readonly DateTime _creationDate;
    
    public OrderByDateSpecification(DateTime creationDate)
        : base(order => order.CreationDate == creationDate)
    {
        _creationDate = creationDate;
    }

    public override string ToString()
        => string.Empty;
}