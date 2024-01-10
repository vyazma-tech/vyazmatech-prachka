using Infrastructure.DataAccess.Contracts;
using Infrastructure.DataAccess.Models;

namespace Infrastructure.DataAccess.Specifications.Subscription;

public sealed class OrderSubscriptionByIdSpecification : Specification<OrderSubscriptionModel>
{
    private readonly Guid _id;

    public OrderSubscriptionByIdSpecification(Guid id)
        : base(subscription => subscription.Id == id)
    {
        _id = id;
    }

    public override string ToString()
        => $"{typeof(Guid)}: {_id}";
}