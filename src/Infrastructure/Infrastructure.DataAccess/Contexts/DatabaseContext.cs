using Application.DataAccess.Contracts;
using Domain.Common.Abstractions;
using Domain.Core.ValueObjects;
using Infrastructure.DataAccess.Models;
using Infrastructure.DataAccess.ValueConverters;
using Infrastructure.Tools;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.DataAccess.Contexts;

public sealed class DatabaseContext : DbContext, IUnitOfWork
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    public DbSet<OrderModel> Orders { get; private init; } = null!;

    public DbSet<UserModel> Users { get; private init; } = null!;

    public DbSet<QueueModel> Queues { get; private init; } = null!;

    public DbSet<QueueSubscriptionModel> QueueSubscriptions { get; private init; } = null!;

    public DbSet<OrderSubscriptionModel> OrderSubscriptions { get; private init; } = null!;

    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return Database.BeginTransactionAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(IDataAccessMarker.Assembly);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<Capacity>().HaveConversion<CapacityValueConverter>();
        configurationBuilder.Properties<TelegramId>().HaveConversion<TelegramIdValueConverter>();
        configurationBuilder.Properties<Fullname>().HaveConversion<FullnameValueConverter>();
        configurationBuilder.Properties<SpbDateTime>().HaveConversion<SpbDateTimeValueConverter>();
    }
}