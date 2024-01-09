using Domain.Core.Queue;
using Infrastructure.DataAccess.Contexts;
using Infrastructure.DataAccess.Contracts;
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
        throw new NotImplementedException();
    }

    protected override QueueEntity MapTo(QueueModel model)
    {
        throw new NotImplementedException();
    }

    protected override bool Equal(QueueEntity entity, QueueModel model)
    {
        return entity.Id.Equals(model.Id);
    }

    protected override void UpdateModel(QueueModel model, QueueEntity entity)
    {
        throw new NotImplementedException();
    }
}