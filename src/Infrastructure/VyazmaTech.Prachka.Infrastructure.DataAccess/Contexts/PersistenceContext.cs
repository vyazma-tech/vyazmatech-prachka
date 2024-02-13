using Microsoft.EntityFrameworkCore;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Application.DataAccess.Contracts.Repositories;

namespace VyazmaTech.Prachka.Infrastructure.DataAccess.Contexts;

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
        OrderSubscriptions = orderSubscriptions;
        QueueSubscriptions = queueSubscriptions;
        _context = context;
    }

    public IQueueRepository Queues { get; }

    public IOrderRepository Orders { get; }

    public IUserRepository Users { get; }

    public IOrderSubscriptionRepository OrderSubscriptions { get; }

    public IQueueSubscriptionRepository QueueSubscriptions { get; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        => _context.SaveChangesAsync(cancellationToken);

    public DbSet<TModel> Entities<TModel>()
        where TModel : class => _context.Set<TModel>();
}