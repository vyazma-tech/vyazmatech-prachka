using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;
using VyazmaTech.Prachka.Domain.Kernel;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Models;

namespace VyazmaTech.Prachka.Infrastructure.DataAccess.Interceptors;

public sealed class DomainEventsToOutboxMessageInterceptor : SaveChangesInterceptor
{
    private static readonly JsonSerializerSettings SerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.All
    };

    private readonly IDateTimeProvider _timeProvider;

    public DomainEventsToOutboxMessageInterceptor(IDateTimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
            StoreDomainEventsAsOutbox(eventData.Context, cancellationToken);

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void StoreDomainEventsAsOutbox(DbContext context, CancellationToken cancellationToken)
    {
        DateTime utcNow = _timeProvider.UtcNow;

        var messages = context.ChangeTracker
            .Entries<Entity>()
            .Select(entry => entry.Entity)
            .Where(entity => entity.DomainEvents.Any())
            .SelectMany(
                entity =>
                {
                    IReadOnlyCollection<IDomainEvent> domainEvents = entity.DomainEvents;
                    entity.ClearDomainEvents();

                    return domainEvents;
                })
            .Select(
                domainEvent => new OutboxMessage
                {
                    Type = domainEvent.GetType().Name,
                    Content = JsonConvert.SerializeObject(domainEvent, SerializerSettings),
                    OccuredOnUtc = utcNow
                })
            .ToList();

        context.Set<OutboxMessage>().AddRange(messages);
    }
}