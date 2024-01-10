using Domain.Core.User;
using Infrastructure.DataAccess.Contracts;
using Infrastructure.DataAccess.Models;

namespace Infrastructure.DataAccess.Specifications.Subscription;

public sealed class OrderSubscriptionByUserSpecification : Specification<OrderSubscriptionModel>
{
    private readonly Guid _userId;

    public OrderSubscriptionByUserSpecification(UserEntity user)
        : base(subscription => subscription.UserId == user.Id)
    {
        _userId = user.Id;
    }

    public override string ToString()
        => $"{typeof(UserEntity)}: {_userId}";
}