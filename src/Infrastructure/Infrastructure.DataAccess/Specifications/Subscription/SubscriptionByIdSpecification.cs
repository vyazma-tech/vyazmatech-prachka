using Domain.Core.Subscription;
using Domain.Kernel;

namespace Infrastructure.DataAccess.Specifications.Subscription;

public sealed class SubscriptionByIdSpecification : Specification<SubscriptionEntity>
{
    private readonly Guid _id;

    public SubscriptionByIdSpecification(Guid id)
        : base(subscription => subscription.Id == id)
    {
        _id = id;
    }

    public override string ToString()
        => $"{typeof(Guid)}: {_id}";
}