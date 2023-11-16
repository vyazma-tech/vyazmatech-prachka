using Domain.Core.Order;
using Domain.Core.User;

namespace Infrastructure.DataAccess.Specifications.Order;

public sealed class OrderByUserSpecification : Specification<OrderEntity>
{
    private readonly Guid _userId;
    
    public OrderByUserSpecification(UserEntity user) 
        : base(order => order.User == user)
    {
        _userId = user.Id;
    }

    public override string ToString()
        => $"{typeof(UserEntity)}: {_userId}";
}