using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VyazmaTech.Prachka.Application.BackgroundWorkers.Configuration;
using VyazmaTech.Prachka.Application.BackgroundWorkers.Extensions;
using VyazmaTech.Prachka.Application.BackgroundWorkers.Queues.Jobs;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Domain.Core.Queues;
using VyazmaTech.Prachka.Domain.Core.ValueObjects;
using VyazmaTech.Prachka.Domain.Kernel;

namespace VyazmaTech.Prachka.Application.BackgroundWorkers.Queues;

internal sealed class QueueSchedulingBackgroundWorker : RestartableBackgroundWorker
{
    private readonly IOptionsMonitor<QueueSchedulingConfiguration> _configuration;
    private readonly IDateTimeProvider _timeProvider;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<QueueSchedulingBackgroundWorker> _logger;

    public QueueSchedulingBackgroundWorker(
        IOptionsMonitor<QueueSchedulingConfiguration> configuration,
        IDateTimeProvider timeProvider,
        IServiceScopeFactory scopeFactory,
        ILogger<QueueSchedulingBackgroundWorker> logger) : base(logger)
    {
        _configuration = configuration;
        _timeProvider = timeProvider;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationTokenSource cts)
    {
        using var timer = new PeriodicTimer(_configuration.CurrentValue.Delay);
        using IDisposable? delayChange = _configuration.OnValueChange(
            nameof(QueueSchedulingConfiguration.Delay),
            cts.Cancel);

        while (cts.IsCancellationRequested is false && await timer.WaitForNextTickAsync(cts.Token))
        {
            using IServiceScope scope = _scopeFactory.CreateScope();
            IServiceProvider sp = scope.ServiceProvider;

            IPersistenceContext context = sp.GetRequiredService<IPersistenceContext>();
            IUnitOfWork unitOfWork = sp.GetRequiredService<IUnitOfWork>();
            QueueSchedulingConfiguration configuration = _configuration.CurrentValue;

            _logger.LogInformation(
                "Starting queues seeding with configuration {@Configuration}",
                configuration);

            await using IDbContextTransaction transaction = await unitOfWork.BeginTransactionAsync(cts.Token);

            try
            {
                List<Queue> queues = await SeedQueuesAsync(cts.Token, configuration, context);
                ScheduleQueueJobs(sp, queues);

                await transaction.CommitAsync(cts.Token);
            }
            catch (Exception e) when (e is not OperationCanceledException or TaskCanceledException)
            {
                _logger.LogError(e, "Exception occured during queue seeding");
                await transaction.RollbackAsync();
                throw;
            }

            _logger.LogInformation("Finished queue seeding for the next {Days} days", configuration.SeedingInterval);
        }
    }

    private async Task<List<Queue>> SeedQueuesAsync(
        CancellationToken stoppingToken,
        QueueSchedulingConfiguration configuration,
        IPersistenceContext context)
    {
        var queues = CreateQueues(configuration).ToList();
        context.Queues.InsertRange(queues);

        await context.SaveChangesAsync(stoppingToken);
        return queues;
    }

    private void ScheduleQueueJobs(IServiceProvider sp, IReadOnlyList<Queue> queues)
    {
        QueueJobScheduler scheduler = sp.GetRequiredService<QueueJobScheduler>();
        foreach (Queue queue in queues)
        {
            scheduler.ScheduleActivation(queue, _timeProvider.UtcNow);
            scheduler.ScheduleExpiration(queue, _timeProvider.UtcNow);
        }
    }

    private IEnumerable<Queue> CreateQueues(QueueSchedulingConfiguration configuration)
    {
        for (int day = 1; day <= configuration.SeedingInterval; day++)
        {
            var capacity = Capacity.Create(configuration.DefaultCapacity);
            AssignmentDate assignmentDate = CreateAssignmentDate(day);
            QueueActivityBoundaries createActivity = CreateActivity(day, configuration);

            yield return new Queue(
                id: Guid.NewGuid(),
                capacity: capacity,
                assignmentDate: assignmentDate,
                activityBoundaries: createActivity,
                state: QueueState.Prepared,
                orders: []);
        }
    }

    private AssignmentDate CreateAssignmentDate(int day)
    {
        DateOnly dateTime = _timeProvider.UtcNow.Date.AddDays(day).AsDateOnly();

        return AssignmentDate.Create(dateTime, _timeProvider.DateNow);
    }

    private QueueActivityBoundaries CreateActivity(int day, QueueSchedulingConfiguration configuration)
    {
        DayOfWeek dayOfWeek = _timeProvider.DateNow.AddDays(day).DayOfWeek;

        return dayOfWeek switch
        {
            DayOfWeek.Saturday or DayOfWeek.Sunday => QueueActivityBoundaries.Create(
                configuration.DayOfActiveFrom,
                configuration.DayOfActiveUntil),

            _ => QueueActivityBoundaries.Create(configuration.WeekdayActiveFrom, configuration.WeekdayActiveUntil)
        };
    }
}