using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using VyazmaTech.Prachka.Application.BackgroundWorkers.Configuration;
using VyazmaTech.Prachka.Application.BackgroundWorkers.Extensions;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Domain.Kernel;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Contexts;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Outbox;
using VyazmaTech.Prachka.Infrastructure.Jobs.Commands.Factories;
using VyazmaTech.Prachka.Infrastructure.Jobs.Jobs;

namespace VyazmaTech.Prachka.Application.BackgroundWorkers.Queues;

internal sealed class QueueSchedulingBackgroundWorker : RestartableBackgroundWorker
{
    private readonly IOptionsMonitor<QueueJobOutboxConfiguration> _configuration;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<QueueSeedingBackgroundWorker> _logger;

    public QueueSchedulingBackgroundWorker(
        ILogger<QueueSeedingBackgroundWorker> logger,
        IOptionsMonitor<QueueJobOutboxConfiguration> configuration,
        IServiceScopeFactory scopeFactory) : base(logger)
    {
        _configuration = configuration;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationTokenSource cts)
    {
        using var timer = new PeriodicTimer(_configuration.CurrentValue.Delay);
        using IDisposable? delayChange = _configuration.OnValueChange(
            nameof(QueueJobOutboxConfiguration.Delay),
            cts.Cancel);

        using IDisposable? batchSizeChange = _configuration.OnValueChange(
            nameof(QueueJobOutboxConfiguration.BatchSize),
            cts.Cancel);

        while (cts.IsCancellationRequested is false && await timer.WaitForNextTickAsync(cts.Token))
        {
            using IServiceScope scope = _scopeFactory.CreateScope();
            IServiceProvider sp = scope.ServiceProvider;
            QueueJobOutboxConfiguration configuration = _configuration.CurrentValue;

            _logger.LogInformation(
                "Starting queues seeding with configuration {@Configuration}",
                configuration);

            try
            {
                int processedMessages = await ProcessOutboxMessagesAsync(sp, configuration, cts.Token);

                _logger.LogInformation(
                    "Worker processed {OutboxProcessed} queue job outbox messages",
                    processedMessages);
            }
            catch (Exception e) when (e is not OperationCanceledException or TaskCanceledException)
            {
                _logger.LogError(e, "Exception occured during job outbox processing");
                throw;
            }
        }
    }

    private async Task<int> ProcessOutboxMessagesAsync(
        IServiceProvider provider,
        QueueJobOutboxConfiguration configuration,
        CancellationToken token)
    {
        DatabaseContext context = provider.GetRequiredService<DatabaseContext>();
        IPersistenceContext persistenceContext = provider.GetRequiredService<IPersistenceContext>();
        QueueJobScheduler scheduler = provider.GetRequiredService<QueueJobScheduler>();
        IDateTimeProvider timeProvider = provider.GetRequiredService<IDateTimeProvider>();

        var factories = provider
            .GetKeyedServices<SchedulingCommandFactory>(nameof(SchedulingCommandFactory))
            .ToList();

        List<QueueJobOutboxMessage> messages = await context.QueueJobOutboxMessages
            .Take(configuration.BatchSize)
            .ToListAsync(token);

        int processedMessages = 0;
        foreach (QueueJobOutboxMessage message in messages)
        {
            try
            {
                Domain.Core.Queues.Queue queue = await persistenceContext.Queues.GetByIdAsync(message.QueueId, token);

                factories.Select(
                        factory => factory.CreateEnclosingCommand(
                            message.JobId,
                            queue.AssignmentDate,
                            queue.ActivityBoundaries))
                    .ToList()
                    .ForEach(command => scheduler.Reschedule(command));

                message.ProcessedOnUtc = timeProvider.UtcNow;

                await context.SaveChangesAsync(token);
                processedMessages += 1;
            }
            catch (Exception e)
            {
                message.Error = JsonConvert.SerializeObject(e);
                await context.SaveChangesAsync(token);
            }
        }

        return processedMessages;
    }
}