using Application.Core.Common;
using Application.Core.Contracts;

namespace Application.Handlers.Queue.Queries;

public class QueueQuery : IQuery<QueueResponse>
{
    public QueueQuery(Guid? queueId, DateOnly? assignmentDate, int? page)
    {
        QueueId = queueId;
        AssignmentDate = assignmentDate;
        Page = page;
    }

    public Guid? QueueId { get; init; }
    public DateOnly? AssignmentDate { get; init; }
    public int? Page { get; init; }
}