using System.Diagnostics;
using Application.BackgroundWorkers.Configuration;
using Application.Core.Services;
using Domain.Common.Result;
using Domain.Core.Queue;
using Domain.Kernel;
using Infrastructure.DataAccess.Contracts;
using Infrastructure.DataAccess.Specifications.Queue;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Application.BackgroundWorkers.Queue;

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

            Result<QueueEntity> queueFindResult = await context.Queues
                .FindByAsync(new QueueByAssignmentDateSpecification(timeProvider.DateNow), stoppingToken);

            if (queueFindResult.IsFaulted)
            {
                _logger.LogInformation(
                    "Queue is not assigned on DateNow = {DateNow}. Attempt DateTime = {UtcNow}",
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