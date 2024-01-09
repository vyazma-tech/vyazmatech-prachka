using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess.Contracts;

public interface IPersistenceContext
{
    IQueueRepository Queues { get; }

    IOrderRepository Orders { get; }

    IUserRepository Users { get; }

    IOrderSubscriptionRepository OrderSubscriptions { get; }

    IQueueSubscriptionRepository QueueSubscriptions { get; }

    DbSet<TEntity> Entities<TEntity>()
        where TEntity : class;
}