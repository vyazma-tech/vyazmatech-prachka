using Application.DataAccess.Contracts;
using Domain.Core.Order;
using Domain.Core.Queue;
using Domain.Core.Subscription;
using Domain.Core.User;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess.Contexts;

internal sealed class PersistenceContext : IPersistenceContext
{
    private readonly DatabaseContext _context;

    public PersistenceContext(
        IQueueRepository queues,
        IOrderRepository orders,
        IUserRepository users,
        ISubscriptionRepository subscriptions,
        DatabaseContext context)
    {
        Queues = queues;
        Orders = orders;
        Users = users;
        Subscriptions = subscriptions;
        _context = context;
    }

    public IQueueRepository Queues { get; }
    public IOrderRepository Orders { get; }
    public IUserRepository Users { get; }
    public ISubscriptionRepository Subscriptions { get; }

    public DbSet<TEntity> Entities<TEntity>()
        where TEntity : class => _context.Set<TEntity>();
}