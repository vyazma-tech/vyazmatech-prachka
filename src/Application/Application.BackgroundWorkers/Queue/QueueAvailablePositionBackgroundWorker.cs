using System.Diagnostics;
using Application.BackgroundWorkers.Configuration;
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
        using IServiceScope scope = _scopeFactory.CreateScope();
        IQueueRepository queueRepository = scope.ServiceProvider.GetRequiredService<IQueueRepository>();
        IDateTimeProvider timeProvider = scope.ServiceProvider.GetRequiredService<IDateTimeProvider>();

        Result<QueueEntity> queueFindResult = await queueRepository
            .FindByAsync(new QueueByAssignmentDateSpecification(timeProvider.DateNow), stoppingToken);

        while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
        {
            _stopwatch.Restart();

            if (queueFindResult.IsFaulted)
            {
                _logger.LogInformation("Queue is not assigned on {Date}", timeProvider.DateNow);
                break;
            }

            IUnitOfWork eventPublisher = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            QueueEntity queue = queueFindResult.Value;

            bool shouldBeNotified = queue.TryNotifyAboutAvailablePosition();
            if (shouldBeNotified is false)
                continue;

            try
            {
                _logger.LogInformation(
                    "Queue assigned to date {Date} expired and not full. Going to notify about available positions",
                    timeProvider.DateNow);

                await eventPublisher.SaveChangesAsync(stoppingToken);

                _logger.LogInformation(
                    "{Worker} finished within {Time} ms",
                    nameof(QueueAvailablePositionBackgroundWorker),
                    _stopwatch.Elapsed.TotalMilliseconds);

                _stopwatch.Stop();
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