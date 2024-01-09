using Domain.Core.Subscription;
using Infrastructure.DataAccess.Contexts;
using Infrastructure.DataAccess.Contracts;
using Infrastructure.DataAccess.Models;

namespace Infrastructure.DataAccess.Repositories;

internal class QueueSubscriptionRepository : RepositoryBase<QueueSubscriptionEntity, QueueSubscriptionModel>, IQueueSubscriptionRepository
{
    /// <inheritdoc cref="RepositoryBase{TEntity,TModel}"/>
    public QueueSubscriptionRepository(DatabaseContext context)
        : base(context)
    {
    }

    protected override QueueSubscriptionModel MapFrom(QueueSubscriptionEntity entity)
    {
        throw new NotImplementedException();
    }

    protected override QueueSubscriptionEntity MapTo(QueueSubscriptionModel model)
    {
        throw new NotImplementedException();
    }

    protected override bool Equal(QueueSubscriptionEntity entity, QueueSubscriptionModel model)
    {
        return entity.Id.Equals(model.Id);
    }

    protected override void UpdateModel(QueueSubscriptionModel model, QueueSubscriptionEntity entity)
    {
        throw new NotImplementedException();
    }
}