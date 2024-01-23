using Application.DataAccess.Contracts.Querying.Queue;
using Application.DataAccess.Contracts.Repositories;
using Domain.Core.Queue;
using Infrastructure.DataAccess.Contexts;
using Infrastructure.DataAccess.Mapping;
using Infrastructure.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess.Repositories;

internal sealed class QueueRepository : RepositoryBase<QueueEntity, QueueModel>, IQueueRepository
{
    public QueueRepository(DatabaseContext context)
        : base(context)
    {
    }

    public IAsyncEnumerable<QueueEntity> QueryAsync(QueueQuery specification, CancellationToken cancellationToken)
    {
        IQueryable<QueueModel> queryable = ApplyQuery(specification);

        var finalQueryable = queryable.Select(queue => new
        {
            queue,
            orders = queue.Orders.Select(x => x.Id)
        });

        return finalQueryable.AsAsyncEnumerable().Select(x => MapTo(x.queue, x.orders));
    }

    public Task<long> CountAsync(QueueQuery specification, CancellationToken cancellationToken)
    {
        IQueryable<QueueModel> queryable = ApplyQuery(specification);
        return queryable.LongCountAsync(cancellationToken);
    }

    protected override QueueModel MapFrom(QueueEntity entity)
    {
        return QueueMapping.MapFrom(entity);
    }

    protected override bool Equal(QueueEntity entity, QueueModel model)
    {
        return entity.Id.Equals(model.Id);
    }

    protected override void UpdateModel(QueueModel model, QueueEntity entity)
    {
        model.ActiveFrom = entity.ActiveFrom;
        model.ActiveUntil = entity.ActiveUntil;
        model.MaxCapacityReached = entity.MaxCapacityReached;
        model.State = entity.State.ToString();
        model.Capacity = entity.Capacity;
        model.ModifiedOn = entity.ModifiedOn;
    }

    private static QueueEntity MapTo(QueueModel model, IEnumerable<Guid> orderIds)
    {
        return QueueMapping.MapTo(model, orderIds.ToHashSet());
    }

    private IQueryable<QueueModel> ApplyQuery(QueueQuery specification)
    {
        IQueryable<QueueModel> queryable = DbSet;

        queryable = queryable
            .Include(x => x.Orders)
            .ThenInclude(x => x.User);

        if (specification.Id is not null)
        {
            queryable = queryable.Where(x => x.Id == specification.Id);
        }

        if (specification.AssignmentDate is not null)
        {
            queryable = queryable.Where(x => x.AssignmentDate == specification.AssignmentDate);
        }

        if (specification.OrderId is not null)
        {
            queryable = queryable.Where(x => x.Orders.Any(model => model.Id == specification.OrderId));
        }

        if (specification.Limit is not null)
        {
            if (specification.Page is not null)
            {
                queryable = queryable
                    .Skip(specification.Page.Value * specification.Limit.Value)
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