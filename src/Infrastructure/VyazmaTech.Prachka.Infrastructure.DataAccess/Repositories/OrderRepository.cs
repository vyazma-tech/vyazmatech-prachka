using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using VyazmaTech.Prachka.Application.Abstractions.Querying.Order;
using VyazmaTech.Prachka.Application.DataAccess.Contracts.Repositories;
using VyazmaTech.Prachka.Domain.Core.Orders;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Contexts;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Mapping;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Models;

namespace VyazmaTech.Prachka.Infrastructure.DataAccess.Repositories;

internal class OrderRepository : RepositoryBase<Order>, IOrderRepository
{
    public OrderRepository(DatabaseContext context)
        : base(context) { }

    public IAsyncEnumerable<Order> QueryAsync(OrderQuery query, CancellationToken cancellationToken)
    {
        IQueryable<OrderModel> queryable = ApplyQuery(query);

        return queryable.AsAsyncEnumerable().Select(MapTo);
    }

    public void RemoveRange(IReadOnlyCollection<Order> orders)
    {
        IEnumerable<OrderModel> model = orders.Select(GetEntry).Select(x => x.Entity);
        DbSet.RemoveRange(model);
    }

    public Task<long> CountAsync(OrderQuery specification, CancellationToken cancellationToken)
    {
        IQueryable<OrderModel> queryable = ApplyQuery(specification);
        return queryable.LongCountAsync(cancellationToken);
    }

    protected override OrderModel MapFrom(Order entity)
    {
        return OrderMapping.MapFrom(entity);
    }

    protected override bool Equal(Order entity, OrderModel model)
    {
        return entity.Id.Equals(model.Id);
    }

    protected override void UpdateModel(OrderModel model, Order entity)
    {
        model.Status = entity.Status.ToString();
        model.ModifiedOn = entity.ModifiedOnUtc;
    }

    private static Order MapTo(OrderModel model)
    {
        return OrderMapping.MapTo(model);
    }

    private EntityEntry<OrderModel> GetEntry(Order entity)
    {
        OrderModel? existing = DbSet.Local
            .FirstOrDefault(x => x.Id.Equals(entity.Id));

        return existing is not null
            ? DbSet.Entry(existing)
            : DbSet.Attach(MapFrom(entity));
    }

    private IQueryable<OrderModel> ApplyQuery(OrderQuery specification)
    {
        IQueryable<OrderModel> queryable = DbSet;

        if (specification.Id is not null)
        {
            queryable = queryable.Where(x => x.Id == specification.Id);
        }

        if (specification.UserId is not null)
        {
            queryable = queryable.Where(x => x.UserId == specification.UserId);
        }

        if (specification.QueueId is not null)
        {
            queryable = queryable.Where(x => x.QueueId == specification.QueueId);
        }

        if (specification.Status is not null)
        {
            queryable = queryable.Where(x => EF.Functions.ILike(x.Status, specification.Status));
        }

        if (specification.CreationDate is not null)
        {
            queryable = queryable.Where(x => x.CreationDate == specification.CreationDate);
        }

        if (specification.Limit is not null)
        {
            if (specification.Page is not null)
            {
                queryable = queryable.Skip(specification.Page.Value * specification.Limit.Value)
                    .Take(specification.Limit.Value);
            }
            else
            {
                queryable = queryable.Take(specification.Limit.Value);
            }
        }

        return queryable;
    }
}