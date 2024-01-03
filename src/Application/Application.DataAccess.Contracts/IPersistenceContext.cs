using Domain.Core.Order;
using Domain.Core.Queue;
using Domain.Core.Subscription;
using Domain.Core.User;

namespace Application.DataAccess.Contracts;

public interface IPersistenceContext
{
    IQueueRepository Queues { get; }

    IOrderRepository Orders { get; }

    IUserRepository Users { get; }

    ISubscriptionRepository Subscriptions { get; }
}