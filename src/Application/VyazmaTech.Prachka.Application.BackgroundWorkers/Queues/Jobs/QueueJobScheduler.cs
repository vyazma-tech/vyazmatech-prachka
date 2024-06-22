using Hangfire;
using VyazmaTech.Prachka.Domain.Core.Queues;
using VyazmaTech.Prachka.Domain.Core.ValueObjects;

namespace VyazmaTech.Prachka.Application.BackgroundWorkers.Queues.Jobs;

internal sealed class QueueJobScheduler
{
    private readonly IBackgroundJobClient _client;

    public QueueJobScheduler(IBackgroundJobClient client)
    {
        _client = client;
    }

    public string ScheduleActivation(Queue queue, DateTime currentDateTimeUtc)
    {
        DateTime executionDate = GetDateTimeFromAssignment(queue.AssignmentDate, queue.ActivityBoundaries.ActiveFrom);

        string jobId = _client.Schedule<QueueActivationJob>(
            job =>
                job.ExecuteAsync(queue.AssignmentDate, CancellationToken.None),
            delay: executionDate - currentDateTimeUtc);

        return jobId;
    }

    public string ScheduleExpiration(Queue queue, DateTime currentDateTimeUtc)
    {
        DateTime expirationDate = GetDateTimeFromAssignment(queue.AssignmentDate, queue.ActivityBoundaries.ActiveUntil);

        string jobId = _client.Schedule<QueueExpirationJob>(
            job => job.ExecuteAsync(queue.AssignmentDate, CancellationToken.None),
            delay: expirationDate - currentDateTimeUtc);

        return jobId;
    }

    private DateTime GetDateTimeFromAssignment(AssignmentDate assignmentDate, TimeOnly time)
    {
        return assignmentDate.Value.ToDateTime(time, DateTimeKind.Utc);
    }
}