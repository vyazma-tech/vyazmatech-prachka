using Microsoft.EntityFrameworkCore;
using VyazmaTech.Prachka.Application.Abstractions.Querying.Queue;
using VyazmaTech.Prachka.Application.DataAccess.Contracts.Repositories;
using VyazmaTech.Prachka.Domain.Core.Orders;
using VyazmaTech.Prachka.Domain.Core.Queues;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Contexts;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Mapping;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Models;

namespace VyazmaTech.Prachka.Infrastructure.DataAccess.Repositories;

internal sealed class QueueRepository : RepositoryBase<Queue, QueueModel>, IQueueRepository
{
    public QueueRepository(DatabaseContext context)
        : base(context) { }

    public IAsyncEnumerable<Queue> QueryAsync(QueueQuery specification, CancellationToken cancellationToken)
    {
        IQueryable<QueueModel> queryable = ApplyQuery(specification);

        var finalQueryable = queryable.Select(
            queue => new
            {
                queue,
                orders = queue.Orders.Select(
                    x => new OrderInfo(
                        x.Id,
                        new UserInfo(
                            x.UserId,
                            x.User.TelegramUsername,
                            x.User.Fullname),
                        queue.Id,
                        Enum.Parse<OrderStatus>(x.Status))),
            });

        return finalQueryable.AsAsyncEnumerable().Select(x => MapTo(x.queue, x.orders));
    }

    public Task<long> CountAsync(QueueQuery specification, CancellationToken cancellationToken)
    {
        IQueryable<QueueModel> queryable = ApplyQuery(specification);
        return queryable.LongCountAsync(cancellationToken);
    }

    protected override QueueModel MapFrom(Queue entity)
    {
        return QueueMapping.MapFrom(entity);
    }

    protected override bool Equal(Queue entity, QueueModel model)
    {
        return entity.Id.Equals(model.Id);
    }

    protected override void UpdateModel(QueueModel model, Queue entity)
    {
        model.ActiveFrom = entity.ActiveFrom;
        model.ActiveUntil = entity.ActiveUntil;
        model.MaxCapacityReached = entity.MaxCapacityReached;
        model.State = entity.State.ToString();
        model.Capacity = entity.Capacity;
        model.ModifiedOn = entity.ModifiedOnUtc;
    }

    private static Queue MapTo(QueueModel model, IEnumerable<OrderInfo> orderIds)
    {
        return QueueMapping.MapTo(model, orderIds.ToHashSet(new OrderByIdComparer()));
    }

    private IQueryable<QueueModel> ApplyQuery(QueueQuery specification)
    {
        IQueryable<QueueModel> queryable = DbSet;

        queryable = queryable
            .AsSplitQuery()
            .Include(x => x.Orders)
            .ThenInclude(x => x.User)
            .OrderBy(x => x.Id);

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