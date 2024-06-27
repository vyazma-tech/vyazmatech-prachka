using VyazmaTech.Prachka.Domain.Core.ValueObjects;
using VyazmaTech.Prachka.Infrastructure.Jobs.Commands.EnclosingJobs;
using VyazmaTech.Prachka.Infrastructure.Jobs.Commands.EntryJobs;

namespace VyazmaTech.Prachka.Infrastructure.Jobs.Commands.Factories;

public abstract class SchedulingCommandFactory
{
    public abstract IEntryLifecycleCommand CreateEntryCommand(
        AssignmentDate assignmentDate,
        QueueActivityBoundaries activityBoundaries);

    public abstract IEnclosingLifecycleCommand CreateEnclosingCommand(
        string jobId,
        AssignmentDate assignmentDate,
        QueueActivityBoundaries activityBoundaries);
}