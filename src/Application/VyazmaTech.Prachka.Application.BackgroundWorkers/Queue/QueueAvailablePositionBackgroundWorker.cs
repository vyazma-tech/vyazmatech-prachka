using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VyazmaTech.Prachka.Application.BackgroundWorkers.Configuration;
using VyazmaTech.Prachka.Application.Core.Services;
using VyazmaTech.Prachka.Application.Core.Specifications;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Domain.Common.Result;
using VyazmaTech.Prachka.Domain.Core.Queue;
using VyazmaTech.Prachka.Domain.Kernel;

namespace VyazmaTech.Prachka.Application.BackgroundWorkers.Queue;

public class QueueAvailablePositionBackgroundWorker : BackgroundService
{
    private readonly TimeSpan _delayBetweenChecks;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly Stopwatch _stopwatch;
    private readonly ILogger<QueueAvailablePositionBackgroundWorker> _logger;

    public QueueAvailablePositionBackgroundWorker(
        IServiceScopeFactory scopeFactory,
        ILogger<QueueAvailablePositionBackgroundWorker> logger,
        IOptions<QueueWorkerConfiguration> workerConfiguration)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
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

            Result<QueueEntity> queueFindResult = await context.Queues
                .FindByAssignmentDateAsync(timeProvider.DateNow, stoppingToken);

            if (queueFindResult.IsFaulted)
            {
                _logger.LogInformation(
                    "Queue is not assigned on DateNow = {DateNow}. Attempt DateTime = {DateTime}",
                    timeProvider.DateNow,
                    timeProvider.UtcNow);

                continue;
            }

            QueueEntity queue = queueFindResult.Value;

            try
            {
                UpdateResult updated = await updater.UpdateQueueAsync(
                    queue,
                    timeProvider,
                    stoppingToken,
                    context);

                if (updated is { Modified: true, PreviousState: QueueState.Expired })
                {
                    _logger.LogInformation(
                        "Queue assigned to AssignmentDate = {AssignmentDate} expired and not full. Worker notified about available positions",
                        queue.CreationDate);

                    _logger.LogInformation(
                        "{Worker} finished within {Time} ms",
                        nameof(QueueAvailablePositionBackgroundWorker),
                        _stopwatch.Elapsed.TotalMilliseconds);

                    _stopwatch.Stop();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(
                    e,
                    "Error occured while notifying about available positions for QueueId = {QueueId}",
                    queue.Id);
            }
        }
    }
}