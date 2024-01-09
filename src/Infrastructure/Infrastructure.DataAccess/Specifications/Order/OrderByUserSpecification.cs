using Application.DataAccess.Contracts;
using Domain.Core.Order;
using Domain.Core.User;
using Domain.Kernel;
using Infrastructure.DataAccess.Contracts;
using Infrastructure.DataAccess.Models;

namespace Infrastructure.DataAccess.Specifications.Order;

public sealed class OrderByUserSpecification : Specification<OrderModel>
{
    private readonly Guid _userId;

    public OrderByUserSpecification(UserModel user)
        : base(order => order.User == user)
    {
        _userId = user.Id;
    }

    public override string ToString()
        => $"{typeof(UserModel)}: {_userId}";
}