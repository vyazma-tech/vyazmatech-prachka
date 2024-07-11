using VyazmaTech.Prachka.Domain.Core.ValueObjects;
using VyazmaTech.Prachka.Infrastructure.Jobs.Commands.EnclosingJobs;
using VyazmaTech.Prachka.Infrastructure.Jobs.Commands.EntryJobs;

namespace VyazmaTech.Prachka.Infrastructure.Jobs.Commands.Factories;

internal sealed class ActivationCommandFactory : SchedulingCommandFactory
{
    public override IEntryLifecycleCommand CreateEntryCommand(
        AssignmentDate assignmentDate,
        QueueActivityBoundaries activityBoundaries)
    {
        return new ActivationCommand(assignmentDate, activityBoundaries);
    }

    public override IEnclosingLifecycleCommand CreateEnclosingCommand(
        string jobId,
        AssignmentDate assignmentDate,
        QueueActivityBoundaries activityBoundaries)
    {
        return new RescheduleActivationCommand(jobId, assignmentDate, activityBoundaries);
    }
}