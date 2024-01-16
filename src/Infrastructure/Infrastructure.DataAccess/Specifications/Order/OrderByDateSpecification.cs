using Domain.Common.Abstractions;
using Infrastructure.DataAccess.Contracts;
using Infrastructure.DataAccess.Models;

namespace Infrastructure.DataAccess.Specifications.Order;

public sealed class OrderByDateSpecification : Specification<OrderModel>
{
    private readonly SpbDateTime _creationDate;

    public OrderByDateSpecification(SpbDateTime creationDate)
        : base(order => order.CreationDate == creationDate)
    {
        _creationDate = creationDate;
    }

    public override string ToString()
        => $"OrderCreationDate = {_creationDate}";
}