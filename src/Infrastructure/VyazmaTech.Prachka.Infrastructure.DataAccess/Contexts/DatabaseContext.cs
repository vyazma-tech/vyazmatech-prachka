using Microsoft.EntityFrameworkCore;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Models;

namespace VyazmaTech.Prachka.Infrastructure.DataAccess.Contexts;

public sealed class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options) { }

    public DbSet<OrderModel> Orders { get; private init; } = null!;

    public DbSet<UserModel> Users { get; private init; } = null!;

    public DbSet<QueueModel> Queues { get; private init; } = null!;

    public DbSet<QueueSubscriptionModel> QueueSubscriptions { get; private init; } = null!;

    public DbSet<OrderSubscriptionModel> OrderSubscriptions { get; private init; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(IDataAccessMarker.Assembly);
    }
}