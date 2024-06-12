using Microsoft.EntityFrameworkCore;
using VyazmaTech.Prachka.Application.Abstractions.Querying.QueueSubscription;
using VyazmaTech.Prachka.Application.DataAccess.Contracts.Repositories;
using VyazmaTech.Prachka.Domain.Core.Subscription;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Contexts;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Mapping;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Models;

namespace VyazmaTech.Prachka.Infrastructure.DataAccess.Repositories;

internal class QueueSubscriptionRepository : RepositoryBase<QueueSubscriptionEntity, QueueSubscriptionModel>,
    IQueueSubscriptionRepository
{
    public QueueSubscriptionRepository(DatabaseContext context)
        : base(context) { }

    public IAsyncEnumerable<QueueSubscriptionEntity> QueryAsync(
        QueueSubscriptionQuery specification,
        CancellationToken cancellationToken)
    {
        IQueryable<QueueSubscriptionModel> queryable = DbSet;

        queryable = queryable
            .AsSplitQuery()
            .Include(x => x.User)
            .ThenInclude(x => x.Orders);

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
            queryable = queryable.Where(x => x.Queues.Any(queue => queue.Id == specification.QueueId));
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

        var finalQueryable = queryable.Select(
            subscription => new
            {
                subscription,
                queues = subscription.Queues.Select(x => x.Id),
            });

        return finalQueryable.AsAsyncEnumerable().Select(x => MapTo(x.subscription, x.queues));
    }

    protected override QueueSubscriptionModel MapFrom(QueueSubscriptionEntity entity)
    {
        return QueueSubscriptionMapping.MapFrom(entity);
    }

    protected override bool Equal(QueueSubscriptionEntity entity, QueueSubscriptionModel model)
    {
        return entity.Id.Equals(model.Id);
    }

    protected override void UpdateModel(QueueSubscriptionModel model, QueueSubscriptionEntity entity)
    {
        model.ModifiedOn = entity.ModifiedOn;
    }

    private static QueueSubscriptionEntity MapTo(QueueSubscriptionModel model, IEnumerable<Guid> queueIds)
    {
        return QueueSubscriptionMapping.MapTo(model, queueIds.ToHashSet());
    }
}