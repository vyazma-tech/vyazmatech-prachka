using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VyazmaTech.Prachka.Application.BackgroundWorkers.Configuration;
using VyazmaTech.Prachka.Application.Core.Services;
using VyazmaTech.Prachka.Application.Core.Specifications;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
using VyazmaTech.Prachka.Domain.Core.Queue;
using VyazmaTech.Prachka.Domain.Kernel;

namespace VyazmaTech.Prachka.Application.BackgroundWorkers.Queue;

public class QueueActivityBackgroundWorker : BackgroundService
{
    private readonly TimeSpan _delayBetweenChecks;
    private readonly ILogger<QueueActivityBackgroundWorker> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly Stopwatch _stopwatch;

    public QueueActivityBackgroundWorker(
        ILogger<QueueActivityBackgroundWorker> logger,
        IServiceScopeFactory scopeFactory,
        IOptions<QueueWorkerConfiguration> workerConfiguration)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
        _delayBetweenChecks = workerConfiguration.Value.SharedDelay;
        _stopwatch = new Stopwatch();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(_delayBetweenChecks);
        using var updater = new QueueUpdatingService();

        using IServiceScope scope = _scopeFactory.CreateScope();
        IDateTimeProvider timeProvider = scope.ServiceProvider.GetRequiredService<IDateTimeProvider>();

        while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
        {
            _stopwatch.Restart();

            IPersistenceContext context = scope.ServiceProvider.GetRequiredService<IPersistenceContext>();

            QueueEntity queue;

            try
            {
                queue = await context.Queues
                    .FindByAssignmentDateAsync(timeProvider.DateNow, stoppingToken);
            }
            catch (NotFoundException)
            {
                _logger.LogInformation(
                    "Queue is not assigned on DateNow = {DateNow}. Attempt DateTime = {UtcNow}",
                    timeProvider.DateNow,
                    timeProvider.UtcNow);

                continue;
            }

            try
            {
                UpdateResult updated = await updater.UpdateQueueAsync(
                    queue,
                    timeProvider,
                    stoppingToken,
                    context);

                if (updated is { Modified: true, PreviousState: QueueState.Active })
                {
                    _logger.LogInformation(
                        "Queue assigned to AssignmentDate = {AssignmentDate} has expired. Worker removed not paid orders and reset subscriptions",
                        queue.CreationDate);

                    _logger.LogInformation(
                        "{Worker} finished within {Time} ms",
                        nameof(QueueActivityBackgroundWorker),
                        _stopwatch.Elapsed.TotalMilliseconds);
                }

                _stopwatch.Stop();
            }
            catch (Exception e)
            {
                _logger.LogError(
                    e,
                    "Error occured while removing not paid orders and resetting subscriptions for QueueId = {QueueId}",
                    queue.Id);
            }
        }
    }
}