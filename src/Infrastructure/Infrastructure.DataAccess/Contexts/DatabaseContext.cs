using Domain.Core.Order;
using Domain.Core.Queue;
using Domain.Core.Subscription;
using Domain.Core.User;
using Domain.Core.ValueObjects;
using Domain.Kernel;
using Infrastructure.DataAccess.Contracts;
using Infrastructure.DataAccess.Repositories;
using Infrastructure.DataAccess.ValueConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.DataAccess.Contexts;

public sealed class DatabaseContext : DbContext, IUnitOfWork
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    public DbSet<OrderEntity> Orders { get; private init; } = null!;

    public DbSet<UserEntity> Users { get; private init; } = null!;

    public DbSet<QueueEntity> Queues { get; private init; } = null!;

    public DbSet<OrderSubscriptionEntity> OrderSubscriptions { get; private init; } = null!;

    public DbSet<QueueSubscriptionEntity> QueueSubscriptions { get; private init; } = null!;

    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        => Database.BeginTransactionAsync(cancellationToken);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(IDataAccessMarker.Assembly);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<Capacity>().HaveConversion<CapacityValueConverter>();
        configurationBuilder.Properties<TelegramId>().HaveConversion<TelegramIdValueConverter>();
        configurationBuilder.Properties<Fullname>().HaveConversion<FullnameValueConverter>();
    }
}