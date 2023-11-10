using Domain.Common.Errors;
using Domain.Common.Exceptions;
using Domain.Common.Result;
using Domain.Core.Order;
using Domain.Core.Queue;
using Domain.Core.User;
using Infrastructure.DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess.Repositories;

internal sealed class QueueRepository : GenericRepository, IQueueRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="QueueRepository"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public QueueRepository(DatabaseContext dbContext)
        : base(dbContext)
    {
    }

    public async Task<Result<QueueEntity>> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        QueueEntity? entity = await DbContext.Queues.
            FirstOrDefaultAsync(u => u.Id == id, cancellationToken: cancellationToken);
        
        if (entity is null)
        {
            var exception = new DomainException(DomainErrors.Queue.NotFound);
            return new Result<QueueEntity>(exception);
        }

        return entity;
    }

    public async Task<Result<QueueEntity>> FindByOrderAsync(OrderEntity order, CancellationToken cancellationToken)
    {
        QueueEntity? entity = await DbContext.Queues.
            FirstOrDefaultAsync(u => u.Items.Contains(order), cancellationToken: cancellationToken);
        
        if (entity is null)
        {
            var exception = new DomainException(DomainErrors.Queue.NotFoundForRequest);
            return new Result<QueueEntity>(exception);
        }

        return entity;
    }

    public async Task<Result<QueueEntity>> FindByCreationDate(DateTime creationDateUtc, CancellationToken cancellationToken)
    {
        QueueEntity? entity = await DbContext.Queues.
            FirstOrDefaultAsync(u => u.CreationDate == creationDateUtc, cancellationToken: cancellationToken);
        
        if (entity is null)
        {
            var exception = new DomainException(DomainErrors.Queue.NotFoundForRequest);
            return new Result<QueueEntity>(exception);
        }

        return entity;
    }

    public async Task InsertRangeAsync(IReadOnlyCollection<QueueEntity> queues, CancellationToken cancellationToken)
    {
        await DbContext.Queues.AddRangeAsync(queues, cancellationToken);
    }

    public void Update(QueueEntity queue)
    {
        DbContext.Queues.Update(queue);
    }

    public async Task<long> CountAsync(CancellationToken cancellationToken)
    {
        return await DbContext.Queues.CountAsync(cancellationToken: cancellationToken);
    }
}