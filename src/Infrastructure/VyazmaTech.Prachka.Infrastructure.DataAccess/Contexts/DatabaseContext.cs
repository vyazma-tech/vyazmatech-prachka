using Microsoft.EntityFrameworkCore;
using VyazmaTech.Prachka.Domain.Common.Abstractions;
using VyazmaTech.Prachka.Domain.Core.ValueObjects;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Models;
using VyazmaTech.Prachka.Infrastructure.DataAccess.ValueConverters;

namespace VyazmaTech.Prachka.Infrastructure.DataAccess.Contexts;

public sealed class DatabaseContext : DbContext
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