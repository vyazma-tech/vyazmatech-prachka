using Hangfire;
using VyazmaTech.Prachka.Domain.Core.ValueObjects;
using VyazmaTech.Prachka.Domain.Kernel;
using VyazmaTech.Prachka.Infrastructure.Jobs.Commands.EntryJobs;
using VyazmaTech.Prachka.Infrastructure.Jobs.Jobs;

namespace VyazmaTech.Prachka.Infrastructure.Jobs.Commands.EnclosingJobs;

internal sealed class ExpirationJob : IEntryLifecycleCommand
{
    private readonly AssignmentDate _assignmentDate;
    private readonly QueueActivityBoundaries _activityBoundaries;

    public ExpirationJob(AssignmentDate assignmentDate, QueueActivityBoundaries activityBoundaries)
    {
        _assignmentDate = assignmentDate;
        _activityBoundaries = activityBoundaries;
    }

    public string Execute(IBackgroundJobClient client, IDateTimeProvider timeProvider)
    {
        DateTime executionDate = IEntryLifecycleCommand.GetDateTimeFromAssignment(
            _assignmentDate,
            _activityBoundaries.ActiveUntil);

        string jobId = client.Schedule<QueueActivationJob>(
            job =>
                job.ExecuteAsync(_assignmentDate, CancellationToken.None),
            delay: executionDate - timeProvider.UtcNow);

        return jobId;
    }
}