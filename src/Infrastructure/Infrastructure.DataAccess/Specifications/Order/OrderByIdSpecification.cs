using Application.DataAccess.Contracts;
using Domain.Core.Order;
using Domain.Kernel;
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
        => $"{typeof(Guid)}: {_orderId}";
}