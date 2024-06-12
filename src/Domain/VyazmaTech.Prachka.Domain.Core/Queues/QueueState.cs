namespace VyazmaTech.Prachka.Domain.Core.Queues;

public enum QueueState
{
    Prepared = 0,
    Active = 1,
    Expired = 2,
    Closed = 3,
}