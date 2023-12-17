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

            bool expiredFirstTime = queue.TryExpire();

            if (expiredFirstTime is false)
                continue;

            try
            {
                _logger.LogInformation(
                    "Queue assigned to {Date} has expired. Going to remove not paid orders and reset subscriptions",
                    queue.CreationDate);

                await eventPublisher.SaveChangesAsync(stoppingToken);

                _logger.LogInformation(
                    "{Worker} finished within {Time} ms",
                    nameof(QueueActivityBackgroundWorker),
                    _stopwatch.Elapsed.TotalMilliseconds);

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