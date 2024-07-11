using Hangfire;
using Microsoft.Extensions.Logging;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Domain.Common.Errors;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
using VyazmaTech.Prachka.Domain.Core.Queues;
using VyazmaTech.Prachka.Domain.Core.ValueObjects;
using VyazmaTech.Prachka.Domain.Kernel;

namespace VyazmaTech.Prachka.Infrastructure.Jobs.Jobs;

[AutomaticRetry(Attempts = 3, DelaysInSeconds = [5, 25, 125])]
internal abstract class QueueStateModificationJob : IQueueJob
{
    protected QueueStateModificationJob(IPersistenceContext context, ILogger<QueueStateModificationJob> logger)
    {
        Context = context;
        Logger = logger;
    }

    protected ILogger<QueueStateModificationJob> Logger { get; }

    protected IPersistenceContext Context { get; }

    public async Task ExecuteAsync(DateOnly assignmentDate, CancellationToken stoppingToken)
    {
        if (stoppingToken.IsCancellationRequested)
            return;

        Queue? queue = await Context.Queues.FindByAssignmentDate(
            assignmentDate: AssignmentDate.Create(assignmentDate, DateTime.UtcNow.AsDateOnly()),
            token: stoppingToken);

        if (queue is null)
        {
            Logger.LogError(
                "Queue with assignment date {@AssignmentDate} not found, consider force performing your action it",
                assignmentDate);

            throw new NotFoundException(DomainErrors.Queue.NotFoundForRequest);
        }

        if (stoppingToken.IsCancellationRequested)
            return;

        await ModifyStateAsync(queue, stoppingToken);

        Logger.LogInformation("Queue with assignment date {@AssignmentDate} activation job finished", assignmentDate);
    }

    protected abstract Task ModifyStateAsync(Queue queue, CancellationToken token);
}