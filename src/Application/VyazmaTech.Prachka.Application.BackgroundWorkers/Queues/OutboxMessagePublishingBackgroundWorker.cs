using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using VyazmaTech.Prachka.Application.BackgroundWorkers.Configuration;
using VyazmaTech.Prachka.Application.BackgroundWorkers.Extensions;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
using VyazmaTech.Prachka.Domain.Kernel;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Contexts;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Models;

namespace VyazmaTech.Prachka.Application.BackgroundWorkers.Queues;

internal sealed class OutboxMessagePublishingBackgroundWorker : RestartableBackgroundWorker
{
    private static readonly JsonSerializerSettings SerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.All,
        ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
    };

    private readonly IOptionsMonitor<OutboxConfiguration> _configuration;
    private readonly IDateTimeProvider _timeProvider;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<OutboxMessagePublishingBackgroundWorker> _logger;

    public OutboxMessagePublishingBackgroundWorker(
        IOptionsMonitor<OutboxConfiguration> configuration,
        IDateTimeProvider timeProvider,
        IServiceScopeFactory scopeFactory,
        ILogger<OutboxMessagePublishingBackgroundWorker> logger) : base(logger)
    {
        _configuration = configuration;
        _timeProvider = timeProvider;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationTokenSource cts)
    {
        using var timer = new PeriodicTimer(_configuration.CurrentValue.Delay);
        using IDisposable? delayChange = _configuration.OnValueChange(nameof(OutboxConfiguration.Delay), cts.Cancel);
        using IDisposable? batchChange = _configuration.OnValueChange(
            nameof(OutboxConfiguration.BatchSize),
            cts.Cancel);

        while (cts.IsCancellationRequested is false && await timer.WaitForNextTickAsync(cts.Token))
        {
            using IServiceScope scope = _scopeFactory.CreateScope();
            IServiceProvider sp = scope.ServiceProvider;

            DatabaseContext context = sp.GetRequiredService<DatabaseContext>();
            IPublisher publisher = sp.GetRequiredService<IPublisher>();
            OutboxConfiguration configuration = _configuration.CurrentValue;

            List<OutboxMessage> messages = await context.OutboxMessages
                .Take(_configuration.CurrentValue.BatchSize)
                .OrderBy(x => x.Id)
                .ToListAsync(cts.Token);

            _logger.LogInformation(
                "Starting processing outbox messages with configuration {@Configuration}",
                configuration);

            int processedMessages = 0;
            foreach (OutboxMessage message in messages)
            {
                IDomainEvent? domainEvent;
                try
                {
                    domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(
                        message.Content,
                        SerializerSettings);
                }
                catch (JsonSerializationException e)
                {
                    _logger.LogWarning("Failed to deserialize message with id {Id}", message.Id);

                    message.Error = $"Failed to deserialize message: {e.Message}";
                    await context.SaveChangesAsync();

                    continue;
                }

                if (domainEvent is null)
                {
                    _logger.LogWarning("Failed to deserialize message with id {Id}", message.Id);

                    message.Error = $"Failed to deserialize message: null";
                    await context.SaveChangesAsync();

                    continue;
                }

                try
                {
                    await publisher.Publish(domainEvent, cts.Token);
                    processedMessages += 1;
                    message.ProcessedOnUtc = _timeProvider.UtcNow;
                    await context.SaveChangesAsync();
                }
                catch (DomainException e)
                {
                    _logger.LogError(e, "Failed to publish domain event with outbox message id {Id}", message.Id);
                    message.Error = e.Message;
                    await context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Unhandled exception occured with outbox message id {Id}", message.Id);
                    message.Error = e.Message;
                    await context.SaveChangesAsync();
                }
            }

            _logger.LogInformation(
                "Finished processing outbox messages. Processed {ProcessedMessages} messages",
                processedMessages);
        }
    }
}