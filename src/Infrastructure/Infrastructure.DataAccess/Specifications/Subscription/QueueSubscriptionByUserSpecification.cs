using Domain.Core.User;
using Infrastructure.DataAccess.Contracts;
using Infrastructure.DataAccess.Models;

namespace Infrastructure.DataAccess.Specifications.Subscription;

public sealed class QueueSubscriptionByUserSpecification : Specification<QueueSubscriptionModel>
{
    private readonly Guid _userId;

    public QueueSubscriptionByUserSpecification(UserEntity user)
        : base(subscription => subscription.UserId == user.Id)
    {
        _userId = user.Id;
    }

    public override string ToString()
        => $"UserId = {_userId}";
}