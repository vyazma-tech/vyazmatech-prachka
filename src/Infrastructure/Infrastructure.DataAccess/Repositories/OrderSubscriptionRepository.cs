using Domain.Core.Subscription;
using Infrastructure.DataAccess.Contexts;
using Infrastructure.DataAccess.Contracts;
using Infrastructure.DataAccess.Mapping;
using Infrastructure.DataAccess.Models;

namespace Infrastructure.DataAccess.Repositories;

internal class OrderSubscriptionRepository : RepositoryBase<OrderSubscriptionEntity, OrderSubscriptionModel>, IOrderSubscriptionRepository
{
    /// <inheritdoc cref="RepositoryBase{TEntity,TModel}"/>
    public OrderSubscriptionRepository(DatabaseContext context)
        : base(context)
    {
    }

    protected override OrderSubscriptionModel MapFrom(OrderSubscriptionEntity entity)
    {
        return OrderSubscriptionMapping.MapFrom(entity);
    }

    protected override OrderSubscriptionEntity MapTo(OrderSubscriptionModel model)
    {
        return OrderSubscriptionMapping.MapTo(model);
    }

    protected override bool Equal(OrderSubscriptionEntity entity, OrderSubscriptionModel model)
    {
        return entity.Id.Equals(model.Id);
    }

    protected override void UpdateModel(OrderSubscriptionModel model, OrderSubscriptionEntity entity)
    {
        model.ModifiedOn = entity.ModifiedOn;
    }
}