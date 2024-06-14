using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using VyazmaTech.Prachka.Domain.Common.Abstractions;
using VyazmaTech.Prachka.Domain.Kernel;

namespace VyazmaTech.Prachka.Infrastructure.DataAccess.Interceptors;

public sealed class AuditableEntityUpdatingInterceptor : SaveChangesInterceptor
{
    private readonly IDateTimeProvider _provider;

    public AuditableEntityUpdatingInterceptor(IDateTimeProvider provider)
    {
        _provider = provider;
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        await UpdateAuditableEntityAsync(eventData.Context, cancellationToken);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private Task UpdateAuditableEntityAsync(DbContext? context, CancellationToken token)
    {
        if (context is null)
            return Task.CompletedTask;

        IEnumerable<EntityEntry<IAuditableEntity>> entries = context.ChangeTracker
            .Entries<IAuditableEntity>();

        foreach (EntityEntry<IAuditableEntity> entityEntry in entries)
        {
            if (entityEntry.State == EntityState.Added)
                entityEntry.Property(entity => entity.CreationDate).CurrentValue = _provider.DateNow;

            if (entityEntry.State == EntityState.Modified)
                entityEntry.Property(entity => entity.ModifiedOnUtc).CurrentValue = _provider.UtcNow;
        }

        return Task.CompletedTask;
    }
}