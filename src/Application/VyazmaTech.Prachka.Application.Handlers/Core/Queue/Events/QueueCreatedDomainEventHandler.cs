using Microsoft.Extensions.DependencyInjection;
using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Domain.Core.Queues.Events;
using VyazmaTech.Prachka.Domain.Kernel;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Contexts;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Outbox;
using VyazmaTech.Prachka.Infrastructure.Jobs.Commands.Factories;
using VyazmaTech.Prachka.Infrastructure.Jobs.Jobs;

namespace VyazmaTech.Prachka.Application.Handlers.Core.Queue.Events;

internal sealed class QueueCreatedDomainEventHandler : IEventHandler<QueueCreatedDomainEvent>
{
    private readonly QueueJobScheduler _scheduler;
    private readonly IDateTimeProvider _timeProvider;
    private readonly DatabaseContext _context;
    private readonly IServiceProvider _serviceProvider;

    public QueueCreatedDomainEventHandler(
        QueueJobScheduler scheduler,
        IDateTimeProvider timeProvider,
        DatabaseContext context,
        IServiceProvider serviceProvider)
    {
        _scheduler = scheduler;
        _timeProvider = timeProvider;
        _context = context;
        _serviceProvider = serviceProvider;
    }

    public async ValueTask Handle(QueueCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var factories = _serviceProvider
            .GetKeyedServices<SchedulingCommandFactory>(nameof(SchedulingCommandFactory))
            .ToList();

        IEnumerable<string> jobIds = factories.Select(
                factory => factory.CreateEntryCommand(notification.AssignmentDate, notification.ActivityBoundaries))
            .Select(command => _scheduler.Schedule(command));

        IEnumerable<QueueJobOutboxMessage> outboxMessages = CreateOutbox(notification, jobIds);

        _context.QueueJobOutboxMessages.AddRange(outboxMessages);
        await _context.SaveChangesAsync(cancellationToken);
    }

    private IEnumerable<QueueJobOutboxMessage> CreateOutbox(
        QueueCreatedDomainEvent notification,
        IEnumerable<string> jobIds)
    {
        foreach (string jobId in jobIds)
        {
            yield return new QueueJobOutboxMessage
            {
                QueueId = notification.QueueId,
                JobId = jobId,
                OccuredOnUtc = _timeProvider.UtcNow
            };
        }
    }
}