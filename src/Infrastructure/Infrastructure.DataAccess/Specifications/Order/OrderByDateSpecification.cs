using Infrastructure.DataAccess.Contracts;
using Infrastructure.DataAccess.Models;

namespace Infrastructure.DataAccess.Specifications.Order;

public sealed class OrderByDateSpecification : Specification<OrderModel>
{
    private readonly DateOnly _creationDate;

    public OrderByDateSpecification(DateOnly creationDate)
        : base(order => DateOnly.FromDateTime(order.CreationDate) == creationDate)
    {
        _creationDate = creationDate;
    }

    public override string ToString()
        => $"OrderCreationDate = {_creationDate}";
}