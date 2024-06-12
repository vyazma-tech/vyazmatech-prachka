using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Application.DataAccess.Contracts.Repositories;

namespace VyazmaTech.Prachka.Infrastructure.DataAccess.Contexts;

internal sealed class PersistenceContext : IPersistenceContext, IUnitOfWork
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
    {
        return _context.SaveChangesAsync(cancellationToken);
    }

    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
    {
        return _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public DbSet<TModel> Entities<TModel>()
        where TModel : class
    {
        return _context.Set<TModel>();
    }
}