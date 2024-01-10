using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess.Contracts;

public interface IPersistenceContext
{
    IQueueRepository Queues { get; }

    IOrderRepository Orders { get; }

    IUserRepository Users { get; }

    IOrderSubscriptionRepository OrderSubscriptions { get; }

    IQueueSubscriptionRepository QueueSubscriptions { get; }

    DbSet<TModel> Entities<TModel>()
        where TModel : class;
}