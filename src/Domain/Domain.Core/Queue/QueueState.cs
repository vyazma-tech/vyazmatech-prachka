namespace Domain.Core.Queue;

public enum QueueState
{
    Prepared = 0,
    Active = 1,
    Expired = 2,
}