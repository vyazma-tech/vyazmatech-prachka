using Domain.Core.Order;
using Infrastructure.DataAccess.Contexts;
using Infrastructure.DataAccess.Contracts;
using Infrastructure.DataAccess.Mapping;
using Infrastructure.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess.Repositories;

internal class OrderRepository : RepositoryBase<OrderEntity, OrderModel>, IOrderRepository
{
    /// <inheritdoc cref="RepositoryBase{TEntity,TModel}"/>
    public OrderRepository(DatabaseContext context)
        : base(context)
    {
    }

    public Task<long> CountAsync(Specification<OrderModel> specification, CancellationToken cancellationToken)
    {
        IQueryable<OrderModel> queryable = ApplySpecification(specification);
        return queryable.LongCountAsync(cancellationToken);
    }

    protected override OrderModel MapFrom(OrderEntity entity)
    {
        return OrderMapping.MapFrom(entity);
    }

    protected override OrderEntity MapTo(OrderModel model)
    {
        return OrderMapping.MapTo(model);
    }

    protected override bool Equal(OrderEntity entity, OrderModel model)
    {
        return entity.Id.Equals(model.Id);
    }

    protected override void UpdateModel(OrderModel model, OrderEntity entity)
    {
        model.Status = entity.Status.ToString();
        model.ModifiedOn = entity.ModifiedOn;
    }
}