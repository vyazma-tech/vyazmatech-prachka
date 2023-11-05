using Domain.Common.Abstractions;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure.DataAccess.Interceptors;

public sealed class PublishDomainEventsInterceptor : SaveChangesInterceptor
{
    private readonly IPublisher _publisher;

    public PublishDomainEventsInterceptor(IPublisher publisher)
    {
        _publisher = publisher;
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        await PublishDomainEvents(eventData.Context, cancellationToken);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private async Task PublishDomainEvents(DbContext? context, CancellationToken cancellationToken)
    {
        if (context is null)
        {
            return;
        }

        var entities = context.ChangeTracker
            .Entries<Entity>()
            .Select(entry => entry.Entity)
            .Where(entity => entity.DomainEvents.Any())
            .ToList();

        var domainEvents = entities
            .SelectMany(entity => entity.DomainEvents)
            .ToList();

        entities.ForEach(entity => entity.ClearDomainEvents());

        IEnumerable<ValueTask> tasks = domainEvents.Select(e => _publisher.Publish(e, cancellationToken));

        foreach (ValueTask task in tasks)
        {
            await task;
        }
        
        await Task.CompletedTask;
    }
}