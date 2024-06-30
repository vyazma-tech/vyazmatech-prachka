using Hangfire;
using VyazmaTech.Prachka.Domain.Core.ValueObjects;
using VyazmaTech.Prachka.Domain.Kernel;
using VyazmaTech.Prachka.Infrastructure.Jobs.Commands.EnclosingJobs;

namespace VyazmaTech.Prachka.Infrastructure.Jobs.Commands.EntryJobs;

internal sealed class RescheduleActivationCommand : IEnclosingLifecycleCommand
{
    private readonly AssignmentDate _assignmentDate;
    private readonly QueueActivityBoundaries _activityBoundaries;
    private readonly string _jobId;

    public RescheduleActivationCommand(
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
            _activityBoundaries.ActiveFrom);

        DateTime utcNow = timeProvider.UtcNow;

        if (executionDate > utcNow)
            client.Reschedule(_jobId, executionDate - utcNow);
        else
            client.Reschedule(_jobId, utcNow);
    }
}