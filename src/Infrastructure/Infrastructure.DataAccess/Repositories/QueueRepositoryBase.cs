using Domain.Core.Queue;
using Infrastructure.DataAccess.Contexts;
using Infrastructure.DataAccess.Contracts;
using Infrastructure.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess.Repositories;

internal sealed class QueueRepositoryBase : RepositoryBase<QueueSubscriptionEntity, QueueSubscriptionModel>, IQueueRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="QueueRepositoryBase" /> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public QueueRepositoryBase(DatabaseContext context)
        : base(context)
    {
    }

    public async Task<long> CountAsync(CancellationToken cancellationToken)
    {
        return await DbSet.CountAsync(cancellationToken);
    }
}