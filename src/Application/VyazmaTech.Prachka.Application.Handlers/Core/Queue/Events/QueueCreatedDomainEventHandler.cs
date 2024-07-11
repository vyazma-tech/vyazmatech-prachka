using Microsoft.Extensions.DependencyInjection;
using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Domain.Core.Queues.Events;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Contexts;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Models;
using VyazmaTech.Prachka.Infrastructure.Jobs.Commands.Factories;
using VyazmaTech.Prachka.Infrastructure.Jobs.Jobs;

namespace VyazmaTech.Prachka.Application.Handlers.Core.Queue.Events;

internal sealed class QueueCreatedDomainEventHandler : IEventHandler<QueueCreatedDomainEvent>
{
    private readonly QueueJobScheduler _scheduler;
    private readonly DatabaseContext _context;
    private readonly IServiceProvider _serviceProvider;

    public QueueCreatedDomainEventHandler(
        QueueJobScheduler scheduler,
        DatabaseContext context,
        IServiceProvider serviceProvider)
    {
        _scheduler = scheduler;
        _context = context;
        _serviceProvider = serviceProvider;
    }

    public async ValueTask Handle(QueueCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var factories = _serviceProvider
            .GetServices<SchedulingCommandFactory>()
            .ToList();

        IEnumerable<string> jobIds = factories.Select(
                factory => factory.CreateEntryCommand(notification.AssignmentDate, notification.ActivityBoundaries))
            .Select(command => _scheduler.Schedule(command));

        IEnumerable<QueueJobMessage> messages = CreateJobMessages(notification, jobIds);

        _context.QueueJobMessages.AddRange(messages);
        await _context.SaveChangesAsync(cancellationToken);
    }

    private IEnumerable<QueueJobMessage> CreateJobMessages(
        QueueCreatedDomainEvent notification,
        IEnumerable<string> jobIds)
    {
        foreach (string jobId in jobIds)
        {
            yield return new QueueJobMessage
            {
                QueueId = notification.QueueId,
                JobId = jobId,
            };
        }
    }
}