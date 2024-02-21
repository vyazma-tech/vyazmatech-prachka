using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using VyazmaTech.Prachka.Domain.Kernel;
using VyazmaTech.Prachka.Infrastructure.Authentication;
using VyazmaTech.Prachka.Infrastructure.Authentication.Outbox;
using VyazmaTech.Prachka.Infrastructure.Tools;

namespace VyazmaTech.Prachka.Application.BackgroundWorkers.Integration;

internal sealed class OutboxMessagesProcessingBackgroundWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<OutboxMessagesProcessingBackgroundWorker> _logger;

    public OutboxMessagesProcessingBackgroundWorker(
        IServiceScopeFactory scopeFactory,
        ILogger<OutboxMessagesProcessingBackgroundWorker> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(10));
        using IServiceScope scope = _scopeFactory.CreateScope();

        while (stoppingToken.IsCancellationRequested is false && await timer.WaitForNextTickAsync(stoppingToken))
        {
            VyazmaTechIdentityContext context = scope.ServiceProvider.GetRequiredService<VyazmaTechIdentityContext>();
            IPublisher publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();

            List<OutboxMessage> messages = await context.OutboxMessages
                .Where(m => m.ProcessedOnUtc == null)
                .Take(1)
                .ToListAsync(stoppingToken);

            foreach (OutboxMessage message in messages)
            {
                var integrationEvent = JsonConvert
                    .DeserializeObject<IIntegrationEvent>(message.Content, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects,
                        ContractResolver = new IgnoreEventsCollectionResolver(new[] { "IntegrationEvents" })
                    });

                if (integrationEvent is null)
                {
                    _logger.LogWarning(
                        @"Integration event is null during deserialization of
                        MessageId = {OutboxMessageId}, Type = {OutboxMessageType}",
                        message.Id,
                        message.Type);

                    continue;
                }

                try
                {
                    await publisher.Publish(integrationEvent, stoppingToken);
                    message.ProcessedOnUtc = DateTime.UtcNow;
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Exception occured during integration event publishing");
                }
            }

            await context.SaveChangesAsync(stoppingToken);
        }
    }
}