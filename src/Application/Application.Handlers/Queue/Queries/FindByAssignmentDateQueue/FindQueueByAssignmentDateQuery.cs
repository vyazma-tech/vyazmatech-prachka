using Application.Core.Contracts;
using Domain.Common.Result;

namespace Application.Handlers.Queue.Queries.FindByAssignmentDateQueue;

public sealed class FindQueueByAssignmentDateQuery : IQuery<Result<QueueResponse>>
{
    public FindQueueByAssignmentDateQuery(DateTime assignmentDate)
    {
        AssignmentDate = assignmentDate;
    }

    public DateTime AssignmentDate { get; set; }
}