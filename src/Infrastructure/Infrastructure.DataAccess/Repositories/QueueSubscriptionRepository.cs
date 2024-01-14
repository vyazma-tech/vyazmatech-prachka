using Domain.Core.Subscription;
using Infrastructure.DataAccess.Contexts;
using Infrastructure.DataAccess.Contracts;
using Infrastructure.DataAccess.Mapping;
using Infrastructure.DataAccess.Models;
using Infrastructure.Tools;

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
        return QueueSubscriptionMapping.MapFrom(entity);
    }

    protected override QueueSubscriptionEntity MapTo(QueueSubscriptionModel model)
    {
        return QueueSubscriptionMapping.MapTo(model);
    }

    protected override bool Equal(QueueSubscriptionEntity entity, QueueSubscriptionModel model)
    {
        return entity.Id.Equals(model.Id);
    }

    protected override void UpdateModel(QueueSubscriptionModel model, QueueSubscriptionEntity entity)
    {
        model.ModifiedOn = entity.ModifiedOn;
    }
}