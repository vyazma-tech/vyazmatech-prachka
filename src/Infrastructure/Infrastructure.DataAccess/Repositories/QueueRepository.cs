using Domain.Core.Queue;
using Infrastructure.DataAccess.Contexts;
using Infrastructure.DataAccess.Contracts;
using Infrastructure.DataAccess.Mapping;
using Infrastructure.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess.Repositories;

internal sealed class QueueRepository : RepositoryBase<QueueEntity, QueueModel>, IQueueRepository
{
    /// <inheritdoc cref="RepositoryBase{TEntity,TModel}"/>
    public QueueRepository(DatabaseContext context)
        : base(context)
    {
    }

    public Task<long> CountAsync(Specification<QueueModel> specification, CancellationToken cancellationToken)
    {
        IQueryable<QueueModel> queryable = ApplySpecification(specification);
        return queryable.LongCountAsync(cancellationToken);
    }

    protected override QueueModel MapFrom(QueueEntity entity)
    {
        return QueueMapping.MapFrom(entity);
    }

    protected override QueueEntity MapTo(QueueModel model)
    {
        return QueueMapping.MapTo(model);
    }

    protected override bool Equal(QueueEntity entity, QueueModel model)
    {
        return entity.Id.Equals(model.Id);
    }

    protected override void UpdateModel(QueueModel model, QueueEntity entity)
    {
        model.ActiveFrom = entity.ActivityBoundaries.ActiveFrom;
        model.ActiveUntil = entity.ActivityBoundaries.ActiveUntil;
        model.MaxCapacityReached = entity.MaxCapacityReachedOnce;
        model.State = entity.State.ToString();
        model.Capacity = entity.Capacity.Value;
        model.ModifiedOn = entity.ModifiedOn;
    }
}