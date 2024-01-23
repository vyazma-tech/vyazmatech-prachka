namespace Domain.Core.Queue;

public sealed class QueueInfo
{
    public QueueInfo(
        Guid id,
        int capacity,
        DateOnly assignmentDate,
        TimeOnly activeFrom,
        TimeOnly activeUntil,
        QueueState state)
    {
        Id = id;
        Capacity = capacity;
        AssignmentDate = assignmentDate;
        ActiveFrom = activeFrom;
        ActiveUntil = activeUntil;
        State = state;
    }

    public Guid Id { get; }

    public int Capacity { get; set; }

    public DateOnly AssignmentDate { get; set; }

    public TimeOnly ActiveFrom { get; set; }

    public TimeOnly ActiveUntil { get; set; }

    public QueueState State { get; set; }
}