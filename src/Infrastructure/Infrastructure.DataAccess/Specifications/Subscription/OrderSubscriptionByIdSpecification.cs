using Application.DataAccess.Contracts;
using Domain.Core.Subscription;
using Domain.Kernel;
using Infrastructure.DataAccess.Contracts;
using Infrastructure.DataAccess.Models;

namespace Infrastructure.DataAccess.Specifications.Subscription;

public sealed class QueueSubscriptionByIdSpecification : Specification<QueueSubscriptionModel>
{
    private readonly Guid _id;

    public QueueSubscriptionByIdSpecification(Guid id)
        : base(subscription => subscription.Id == id)
    {
        _id = id;
    }

    public override string ToString()
        => $"{typeof(Guid)}: {_id}";
}