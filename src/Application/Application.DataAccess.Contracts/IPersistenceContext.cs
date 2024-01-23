using Application.DataAccess.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Application.DataAccess.Contracts;

public interface IPersistenceContext
{
    IQueueRepository Queues { get; }

    IOrderRepository Orders { get; }

    IUserRepository Users { get; }

    IOrderSubscriptionRepository OrderSubscriptions { get; }

    IQueueSubscriptionRepository QueueSubscriptions { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    DbSet<TModel> Entities<TModel>()
        where TModel : class;
}