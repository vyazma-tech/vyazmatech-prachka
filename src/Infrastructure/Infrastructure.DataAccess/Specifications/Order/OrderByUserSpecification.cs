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
        => $"UserId = {_userId}";
}