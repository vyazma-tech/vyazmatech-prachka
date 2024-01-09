using Application.DataAccess.Contracts;
using Domain.Core.Order;
using Domain.Kernel;
using Infrastructure.DataAccess.Contracts;
using Infrastructure.DataAccess.Models;

namespace Infrastructure.DataAccess.Specifications.Order;

public sealed class OrderByDateSpecification : Specification<OrderModel>
{
    public OrderByDateSpecification(DateOnly creationDate)
        : base(order => order.CreationDate == creationDate)
    {
    }

    public override string ToString()
        => string.Empty;
}