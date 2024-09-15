using Microsoft.Extensions.Logging;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Domain.Core.Queues;

namespace VyazmaTech.Prachka.Infrastructure.Jobs.Jobs;

internal sealed class QueueExpirationJob : QueueStateModificationJob
{
    public QueueExpirationJob(IPersistenceContext context, ILogger<QueueExpirationJob> logger)
        : base(context, logger) { }

    protected override async Task ModifyStateAsync(Queue queue, CancellationToken token)
    {
        Logger.LogInformation(
            "Queue with assignment date {@AssignmentDate} expiration job started",
            queue.AssignmentDate);

        // publish domain events before closing a queue
        queue.ModifyState(QueueState.Expired);
        queue.ModifyState(QueueState.Closed);
        await Context.SaveChangesAsync(token);

        Logger.LogInformation(
            "Queue with assignment date {@AssignmentDate} published their domain events and closed",
            queue.AssignmentDate);
    }
}