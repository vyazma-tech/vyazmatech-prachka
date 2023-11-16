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

    public async Task<long> CountAsync(CancellationToken cancellationToken)
    {
        return await DbSet.CountAsync(cancellationToken: cancellationToken);
    }
}