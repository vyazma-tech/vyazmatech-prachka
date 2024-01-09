using Domain.Core.Order;
using Infrastructure.DataAccess.Contexts;
using Infrastructure.DataAccess.Contracts;
using Infrastructure.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess.Repositories;

internal class OrderRepositoryBase : RepositoryBase<OrderEntity, OrderModel>, IOrderRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OrderRepositoryBase" /> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public OrderRepositoryBase(DatabaseContext context)
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
        throw new NotImplementedException();
    }

    protected override OrderEntity MapTo(OrderModel model)
    {
        throw new NotImplementedException();
    }

    protected override bool Equal(OrderEntity entity, OrderModel model)
    {
        return entity.Id.Equals(model.Id);
    }

    protected override void UpdateModel(OrderModel model, OrderEntity entity)
    {
        throw new NotImplementedException();
    }
}