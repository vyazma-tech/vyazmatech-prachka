using Application.DataAccess.Contracts;
using Domain.Core.Order;
using Domain.Core.Queue;
using Domain.Core.Subscription;
using Domain.Core.User;
using Infrastructure.DataAccess.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess.Contexts;

internal sealed class PersistenceContext : IPersistenceContext
{
    private readonly DatabaseContext _context;

    public PersistenceContext(
        IQueueRepository queues,
        IOrderRepository orders,
        IUserRepository users,
        IOrderSubscriptionRepository orderSubscriptions,
        IQueueSubscriptionRepository queueSubscriptions,
        DatabaseContext context)
    {
        Queues = queues;
        Orders = orders;
        Users = users;
        _context = context;
        OrderSubscriptions = orderSubscriptions;
        QueueSubscriptions = queueSubscriptions;
    }

    public IQueueRepository Queues { get; }

    public IOrderRepository Orders { get; }

    public IUserRepository Users { get; }

    public IOrderSubscriptionRepository OrderSubscriptions { get; }

    public IQueueSubscriptionRepository QueueSubscriptions { get; }

    public DbSet<TEntity> Entities<TEntity>()
        where TEntity : class => _context.Set<TEntity>();
}