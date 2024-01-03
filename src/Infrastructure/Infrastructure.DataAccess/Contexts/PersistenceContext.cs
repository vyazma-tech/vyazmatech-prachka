using Application.DataAccess.Contracts;
using Domain.Core.Order;
using Domain.Core.Queue;
using Domain.Core.Subscription;
using Domain.Core.User;

namespace Infrastructure.DataAccess.Contexts;

internal sealed class PersistenceContext : IPersistenceContext
{
    public PersistenceContext(
        IQueueRepository queues,
        IOrderRepository orders,
        IUserRepository users,
        ISubscriptionRepository subscriptions)
    {
        Queues = queues;
        Orders = orders;
        Users = users;
        Subscriptions = subscriptions;
    }

    public IQueueRepository Queues { get; }
    public IOrderRepository Orders { get; }
    public IUserRepository Users { get; }
    public ISubscriptionRepository Subscriptions { get; }
}