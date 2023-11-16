using Domain.Common.Errors;
using Domain.Common.Exceptions;
using Domain.Common.Result;
using Domain.Core.Order;
using Domain.Core.Queue;
using Infrastructure.DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess.Repositories;

internal sealed class QueueRepository : GenericRepository<QueueEntity>, IQueueRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="QueueRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public QueueRepository(DatabaseContext context)
        : base(context)
    {
    }

    public async Task<Result<QueueEntity>> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        QueueEntity? entity = await DbSet
            .FirstOrDefaultAsync(
                u => u.Id == id,
                cancellationToken: cancellationToken);
        
        if (entity is null)
        {
            var exception = new DomainException(DomainErrors.Queue.NotFound);
            return new Result<QueueEntity>(exception);
        }

        return entity;
    }

    public async Task<Result<QueueEntity>> FindByOrderAsync(OrderEntity order, CancellationToken cancellationToken)
    {
        QueueEntity? entity = await DbSet
            .FirstOrDefaultAsync(
                u => u.Items.Contains(order),
                cancellationToken: cancellationToken);
        
        if (entity is null)
        {
            var exception = new DomainException(DomainErrors.Queue.NotFoundForRequest);
            return new Result<QueueEntity>(exception);
        }

        return entity;
    }

    public async Task<Result<QueueEntity>> FindByCreationDate(
        DateTime creationDateUtc,
        CancellationToken cancellationToken)
    {
        QueueEntity? entity = await DbSet
            .FirstOrDefaultAsync(
                u => u.CreationDate == creationDateUtc,
                cancellationToken: cancellationToken);
        
        if (entity is null)
        {
            var exception = new DomainException(DomainErrors.Queue.NotFoundForRequest);
            return new Result<QueueEntity>(exception);
        }

        return entity;
    }

    public async Task<long> CountAsync(CancellationToken cancellationToken)
    {
        return await DbSet.CountAsync(cancellationToken: cancellationToken);
    }
}