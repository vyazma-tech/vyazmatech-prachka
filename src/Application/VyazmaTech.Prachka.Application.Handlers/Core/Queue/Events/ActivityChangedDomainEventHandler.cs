﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Domain.Core.Queues;
using VyazmaTech.Prachka.Domain.Core.Queues.Events;
using VyazmaTech.Prachka.Domain.Kernel;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Contexts;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Models;
using VyazmaTech.Prachka.Infrastructure.Jobs.Commands.Factories;
using VyazmaTech.Prachka.Infrastructure.Jobs.Jobs;

namespace VyazmaTech.Prachka.Application.Handlers.Core.Queue.Events;

internal sealed class ActivityChangedDomainEventHandler : IEventHandler<ActivityChangedDomainEvent>
{
    private readonly DatabaseContext _context;
    private readonly IDateTimeProvider _timeProvider;
    private readonly IServiceProvider _serviceProvider;

    public ActivityChangedDomainEventHandler(
        DatabaseContext context,
        IServiceProvider serviceProvider,
        IDateTimeProvider timeProvider)
    {
        _context = context;
        _serviceProvider = serviceProvider;
        _timeProvider = timeProvider;
    }

    public async ValueTask Handle(ActivityChangedDomainEvent notification, CancellationToken cancellationToken)
    {
        Domain.Core.Queues.Queue queue = await _context.Queues.FirstAsync(
            x => x.Id == notification.QueueId,
            cancellationToken);

        ChangeState(queue);

        List<QueueJobMessage> messages = await _context.QueueJobMessages
            .Where(x => x.QueueId == notification.QueueId)
            .ToListAsync(cancellationToken);

        RescheduleJobs(notification, messages);

        await _context.SaveChangesAsync(cancellationToken);
    }

    private void ChangeState(Domain.Core.Queues.Queue queue)
    {
        DateTime currentTimeUtc = _timeProvider.UtcNow;

        if (currentTimeUtc.AsTimeOnly() <= queue.ActivityBoundaries.ActiveFrom &&
            currentTimeUtc.AsDateOnly() <= queue.AssignmentDate)
        {
            queue.ModifyState(QueueState.Prepared);

            return;
        }

        if (currentTimeUtc.AsTimeOnly() >= queue.ActivityBoundaries.ActiveUntil &&
            currentTimeUtc.AsDateOnly() >= queue.AssignmentDate)
        {
            queue.ModifyState(QueueState.Closed);

            return;
        }

        if (currentTimeUtc.AsDateOnly() == queue.AssignmentDate)
            queue.ModifyState(QueueState.Active);
    }

    private void RescheduleJobs(
        ActivityChangedDomainEvent notification,
        List<QueueJobMessage> messages)
    {
        QueueJobScheduler scheduler = _serviceProvider.GetRequiredService<QueueJobScheduler>();
        var factories = _serviceProvider
            .GetServices<SchedulingCommandFactory>()
            .ToList();

        factories.Zip(messages)
            .Select(x => x.First.CreateEnclosingCommand(
                jobId: x.Second.JobId,
                assignmentDate: notification.AssignmentDate,
                activityBoundaries: notification.ActivityBoundaries))
            .ToList()
            .ForEach(scheduler.Reschedule);
    }
}