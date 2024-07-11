using Hangfire;
using VyazmaTech.Prachka.Domain.Core.ValueObjects;
using VyazmaTech.Prachka.Domain.Kernel;

namespace VyazmaTech.Prachka.Infrastructure.Jobs.Commands.EnclosingJobs;

public interface IEnclosingLifecycleCommand
{
    void Execute(IBackgroundJobClient client, IDateTimeProvider timeProvider);

    static DateTime GetDateTimeFromAssignment(AssignmentDate assignmentDate, TimeOnly time)
    {
        return assignmentDate.Value.ToDateTime(time, DateTimeKind.Utc);
    }
}