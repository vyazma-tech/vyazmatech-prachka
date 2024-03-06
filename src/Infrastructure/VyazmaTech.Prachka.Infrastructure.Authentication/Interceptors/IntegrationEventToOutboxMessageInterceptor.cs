using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;
using VyazmaTech.Prachka.Domain.Kernel;
using VyazmaTech.Prachka.Infrastructure.Authentication.Models;
using VyazmaTech.Prachka.Infrastructure.Authentication.Outbox;

namespace VyazmaTech.Prachka.Infrastructure.Authentication.Interceptors;

internal sealed class IntegrationEventToOutboxMessageInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        DbContext? context = eventData.Context;

        if (context is null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        var messages = context.ChangeTracker
            .Entries<VyazmaTechIdentityUser>()
            .Select(entry => entry.Entity)
            .SelectMany(user =>
            {
                IReadOnlyCollection<IIntegrationEvent> integrationEvents = user.IntegrationEvents;
                user.ClearIntegrationEvents();

                return integrationEvents;
            })
            .Select(@event => new OutboxMessage
            {
                Id = Guid.NewGuid(),
                OccuredOnUtc = DateTime.UtcNow,
                Type = @event.GetType().Name,
                Content = JsonConvert.SerializeObject(@event, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                })
            })
            .ToList();

        context.Set<OutboxMessage>().AddRange(messages);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}