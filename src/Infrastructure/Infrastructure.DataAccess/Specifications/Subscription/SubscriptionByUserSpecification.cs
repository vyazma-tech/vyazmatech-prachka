using Domain.Core.Subscription;
using Domain.Core.User;
using Domain.Kernel;

namespace Infrastructure.DataAccess.Specifications.Subscription;

public sealed class SubscriptionByUserSpecification : Specification<SubscriptionEntity>
{
    private readonly Guid _userId;

    public SubscriptionByUserSpecification(UserEntity user)
        : base(subscription => subscription.User == user)
    {
        _userId = user.Id;
    }

    public override string ToString()
        => $"{typeof(UserEntity)}: {_userId}";
}