using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
using VyazmaTech.Prachka.Domain.Core.Queues;
using VyazmaTech.Prachka.Domain.Core.ValueObjects;
using VyazmaTech.Prachka.Domain.Kernel;
using VyazmaTech.Prachka.Infrastructure.Jobs.Configuration;

namespace VyazmaTech.Prachka.Infrastructure.Jobs.Jobs;

internal sealed class QueueSeedingJob
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<QueueSeedingJob> _logger;
    private readonly IOptionsMonitor<QueueSeedingConfiguration> _configuration;
    private readonly IDateTimeProvider _timeProvider;

    public QueueSeedingJob(
        IServiceScopeFactory scopeFactory,
        ILogger<QueueSeedingJob> logger,
        IOptionsMonitor<QueueSeedingConfiguration> configuration,
        IDateTimeProvider timeProvider)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _configuration = configuration;
        _timeProvider = timeProvider;
    }

    public async Task Execute()
    {
        using IServiceScope scope = _scopeFactory.CreateScope();
        IServiceProvider sp = scope.ServiceProvider;

        IPersistenceContext context = sp.GetRequiredService<IPersistenceContext>();
        IUnitOfWork unitOfWork = sp.GetRequiredService<IUnitOfWork>();
        QueueSeedingConfiguration configuration = _configuration.CurrentValue;

        _logger.LogInformation(
            "Starting queues seeding with configuration {@Configuration}",
            configuration);

        await using IDbContextTransaction transaction = await unitOfWork.BeginTransactionAsync();

        try
        {
            await SeedQueuesAsync(configuration, context);
            await transaction.CommitAsync();

            _logger.LogInformation(
                "Finished queue seeding for the next {Days} days",
                configuration.SeedingInterval);
        }
        catch (DomainException e)
        {
            _logger.LogError(e, e.Message);
            await transaction.RollbackAsync();
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(
                "Worker attempted to seed queue with duplicate date with entries {@Entries}",
                e.Entries.Select(x => x.Entity));

            await transaction.RollbackAsync();
        }
    }

    private async Task SeedQueuesAsync(
        QueueSeedingConfiguration configuration,
        IPersistenceContext context)
    {
        var queues = CreateQueues(configuration).ToList();
        context.Queues.InsertRange(queues);

        await context.SaveChangesAsync(default);
    }

    private IEnumerable<Queue> CreateQueues(QueueSeedingConfiguration configuration)
    {
        for (int day = 1; day <= configuration.SeedingInterval; day++)
        {
            var capacity = Capacity.Create(configuration.DefaultCapacity);
            AssignmentDate assignmentDate = CreateAssignmentDate(day);
            QueueActivityBoundaries createActivity = CreateActivity(day, configuration);

            yield return Queue.Create(
                capacity: capacity,
                assignmentDate: assignmentDate,
                activityBoundaries: createActivity);
        }
    }

    private AssignmentDate CreateAssignmentDate(int day)
    {
        DateOnly dateTime = _timeProvider.UtcNow.Date.AddDays(day).AsDateOnly();

        return AssignmentDate.Create(dateTime, _timeProvider.DateNow);
    }

    private QueueActivityBoundaries CreateActivity(int day, QueueSeedingConfiguration configuration)
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