using Hangfire;
using VyazmaTech.Prachka.Domain.Core.ValueObjects;
using VyazmaTech.Prachka.Domain.Kernel;

namespace VyazmaTech.Prachka.Infrastructure.Jobs.Commands.EnclosingJobs;

internal sealed class RescheduleExpirationCommand : IEnclosingLifecycleCommand
{
    private readonly AssignmentDate _assignmentDate;
    private readonly QueueActivityBoundaries _activityBoundaries;
    private readonly string _jobId;

    public RescheduleExpirationCommand(
        string jobId,
        AssignmentDate assignmentDate,
        QueueActivityBoundaries activityBoundaries)
    {
        _assignmentDate = assignmentDate;
        _activityBoundaries = activityBoundaries;
        _jobId = jobId;
    }

    public void Execute(IBackgroundJobClient client, IDateTimeProvider timeProvider)
    {
        DateTime executionDate = IEnclosingLifecycleCommand.GetDateTimeFromAssignment(
            _assignmentDate,
            _activityBoundaries.ActiveUntil);

        DateTime utcNow = timeProvider.UtcNow;

        if (executionDate > utcNow)
            client.Reschedule(_jobId, executionDate - utcNow);
        else
            client.Reschedule(_jobId, utcNow);
    }
}