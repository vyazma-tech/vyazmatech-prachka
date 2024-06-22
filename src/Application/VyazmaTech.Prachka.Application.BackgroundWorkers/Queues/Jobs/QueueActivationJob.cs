using Microsoft.Extensions.Logging;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Domain.Core.Queues;

namespace VyazmaTech.Prachka.Application.BackgroundWorkers.Queues.Jobs;

internal sealed class QueueActivationJob : QueueStateModificationJob
{
    public QueueActivationJob(IPersistenceContext context, ILogger<QueueActivationJob> logger)
        : base(context, logger) { }

    protected override async Task ModifyStateAsync(Queue queue, CancellationToken token)
    {
        Logger.LogInformation(
            "Queue with assignment date {@AssignmentDate} activation job started",
            queue.AssignmentDate);

        queue.ModifyState(QueueState.Active);
        await Context.SaveChangesAsync(token);

        Logger.LogInformation(
            "Queue with assignment date {@AssignmentDate} activated",
            queue.AssignmentDate);
    }
}